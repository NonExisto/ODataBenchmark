using System;
using System.Linq;
using System.Threading.Tasks;
using TestRunner.Model;
using TestRunner.Model.TestCases;
using System.IO;
using System.Text.Json;

namespace TestRunner
{
	internal static class Program
	{
		private const int _runCount = 1000;
		public async static Task Main()
		{
			IHostingConfiguration hostingConfiguration = new HostingConfiguration();
			Console.Write("Preparing test server...");
			var seedSize = await InitTests(hostingConfiguration).ConfigureAwait(false);
			Console.WriteLine($"Done with seed {seedSize}");
			(ITestCase TestCase, ITestCaseSource TestSource, ITestCaseSourceItem Item)[] tests = PrepareTestCases(seedSize);
			Console.WriteLine($"Generated {tests.Length} tests. Starting benchmark");
			await RunAllTests(hostingConfiguration, tests).ConfigureAwait(false);
			Console.WriteLine("All Done");
		}

		private static async Task RunAllTests(IHostingConfiguration hostingConfiguration, (ITestCase TestCase, ITestCaseSource TestSource, ITestCaseSourceItem Item)[] tests)
		{
			var split = Environment.ProcessorCount >> 2 + Environment.ProcessorCount >> 3; // 6 out 8
			var blockLen = tests.Length / split;
			var tasks = Enumerable.Range(0, split).Select(idx =>
				new ArraySegment<(ITestCase TestCase, ITestCaseSource TestSource, ITestCaseSourceItem Item)>(tests, idx * blockLen, blockLen))
					.Select(segment => RunTestGroup(segment, hostingConfiguration)).ToArray();
			await Task.WhenAll(tasks).ConfigureAwait(false);

			var benchMark = tasks.SelectMany(t => t.Result).GroupBy(r => r.TestCase.Name)
					.Select(gr => new
					{
						Name = gr.Key,
						Types = gr.GroupBy(t => t.TestSource.TestCaseType)
							.Select(tg => new
							{
								TestType = tg.Key.ToString(),
								Stats = tg.Aggregate((duration: 0L, payload: 0L),
							(cur, sg) => (duration: cur.duration + sg.Duration, payload: cur.payload + sg.PayloadSize))
							})
							.Select(v => new { v.TestType, AvgDurationMs = v.Stats.duration / (double)_runCount, AvgPayloadSizeBytes = v.Stats.payload / (double)_runCount })
							.ToArray()
					});
			using (var stream = File.Open("..\\..\\run.json", FileMode.Create, FileAccess.Write, FileShare.None))
			{
				await JsonSerializer.SerializeAsync(stream, benchMark, new JsonSerializerOptions { WriteIndented = true }).ConfigureAwait(false);
			}
		}

		private static (ITestCase TestCase, ITestCaseSource TestSource, ITestCaseSourceItem Item)[] PrepareTestCases(int seedSize)
		{
			ITestCase[] testCases = new ITestCase[] { new AllScopes(), new AllCustomers(), new AllProjects(), new SingleProject(), new AllWorkItems()};

			var tests = testCases.SelectMany(tc =>
					tc.GetTestSources()
					.SelectMany(ts => ts.GetTestCaseItems(_runCount, seedSize)
					.Select(item => (TestCase: tc, TestSource: ts, Item: item))))
				.OrderBy(tsi => tsi.Item.Order).ToArray();
			return tests;
		}

		private static async Task<int> InitTests(IHostingConfiguration hostingConfiguration)
		{
			await Task.Delay(2500).ConfigureAwait(false);

			var resp = await hostingConfiguration.SendAsync("api/test/StartTest", "plain/text").ConfigureAwait(false);
			resp.EnsureSuccessStatusCode();
			var lenValue = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

			return int.Parse(lenValue);
		}

		private static async Task<TestCaseSourceItemResult[]> RunTestGroup(ArraySegment<(ITestCase TestCase, ITestCaseSource TestSource, ITestCaseSourceItem Item)> items, IHostingConfiguration hostingConfiguration)
		{
			TestCaseSourceItemResult[] results = new TestCaseSourceItemResult[items.Count];
			for (int i = 0; i < items.Count; i++)
			{
				var (TestCase, TestSource, Item) = items[i];
				var (duration, payloadSize) = await Item.RunTest(hostingConfiguration).ConfigureAwait(false);
				results[i] = new TestCaseSourceItemResult { Duration = duration, PayloadSize = payloadSize, TestCase = TestCase, TestCaseSourceItem = Item, TestSource = TestSource };
			}

			return results;
		}
	}
}
