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
		public async static Task Main()
		{
			IHostingConfiguration hostingConfiguration = new HostingConfiguration();
			var seedSize = await InitTests(hostingConfiguration).ConfigureAwait(false);

			ITestCase[] testCases = new[] { new AllScopes() };

			var tests = testCases.SelectMany(tc =>
					tc.GetTestSources()
					.SelectMany(ts => ts.GetTestCaseItems(1000, seedSize)
					.Select(item => (TestCase: tc, TestSource: ts, Item: item))))
				.OrderBy(tsi => tsi.Item.Order).ToArray();
			var split = Environment.ProcessorCount >> 2;

			var blockLen = tests.Length / split;
			var tasks = Enumerable.Range(0, split).Select(idx =>
				new ArraySegment<(ITestCase TestCase, ITestCaseSource TestSource, ITestCaseSourceItem Item)>(tests, idx * blockLen, (idx + 1) * blockLen))
					.Select(segment => RunTests(segment, hostingConfiguration)).ToArray();
			await Task.WhenAll(tasks).ConfigureAwait(false);

			var benchMark = tasks.SelectMany(t => t.Result).GroupBy(r => r.TestCase.Name)
					.Select(gr => new
					{
						Name = gr.Key,
						Types = gr.GroupBy(t => t.TestSource.TestCaseType)
							.Select(tg => new
							{
								TypeName = tg.Key,
								Stats = tg.Aggregate((duration: 0L, payload: 0L),
							(cur, sg) => (duration: cur.duration + sg.Duration, payload: cur.payload + sg.PayloadSize))
							})
							.Select(v => new { v.TypeName, AvgDuration = v.Stats.duration / (double)tests.Length, AvgPayloadSize = v.Stats.payload / (double)tests.Length })
							.ToArray()
					});

			using var stream = File.Open("..\\run.json", FileMode.Create, FileAccess.Write, FileShare.None);
			await JsonSerializer.SerializeAsync(stream, benchMark, new JsonSerializerOptions { WriteIndented = true }).ConfigureAwait(false);
		}

		private static async Task<int> InitTests(IHostingConfiguration hostingConfiguration)
		{
			await Task.Delay(2500).ConfigureAwait(false);

			var resp = await hostingConfiguration.SendAsync("api/test/StartTest", "plain/text").ConfigureAwait(false);
			resp.EnsureSuccessStatusCode();
			var lenValue = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

			return int.Parse(lenValue);
		}

		private static async Task<TestCaseSourceItemResult[]> RunTests(ArraySegment<(ITestCase TestCase, ITestCaseSource TestSource, ITestCaseSourceItem Item)> items, IHostingConfiguration hostingConfiguration)
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
