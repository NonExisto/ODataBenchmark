using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TestRunner.Model;
using TestRunner.Model.TestCases;
using System.Net.Http.Headers;
using System.IO;
using System.Text.Json;

namespace TestRunner
{
	internal static class Program
	{
		private const string _host = "http://localhost:26649/";
		public async static Task Main()
		{
			HttpClient client = new();
			var seedSize = await InitTests(client).ConfigureAwait(false);

			ITestCase[] testCases = new[] { new AllScopes() };

			var tests = testCases.SelectMany(tc => tc.GetTestSources().SelectMany(ts => ts.GetTestCaseItems(1000, seedSize))).OrderBy(tsi => tsi.Order).ToArray();
			var split = Environment.ProcessorCount >> 2;

			var blockLen = tests.Length / split;
			var tasks = Enumerable.Range(0, split).Select(idx => new ArraySegment<ITestCaseSourceItem>(tests, idx * blockLen, (idx + 1) * blockLen))
				.Select(segment => RunTests(segment, client)).ToArray();
			await Task.WhenAll(tasks).ConfigureAwait(false);

			var benchMark = tasks.SelectMany(t => t.Result).GroupBy(r => r.TestCaseSourceItem.TestCase.Name)
					.Select(gr => new
					{
						Name = gr.Key,
						Types = gr.GroupBy(t => t.TestCaseSourceItem.TestCaseSource.TestCaseType)
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

		private static async Task<int> InitTests(HttpClient client)
		{
			await Task.Delay(2500).ConfigureAwait(false);

			HttpRequestMessage startRequest = new(HttpMethod.Get, $"{_host}api/test/StartTest");
			startRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));
			var resp = await client.SendAsync(startRequest).ConfigureAwait(false);
			resp.EnsureSuccessStatusCode();
			var lenValue = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

			return int.Parse(lenValue);
		}

		private static async Task<ITestCaseSourceItemResult[]> RunTests(ArraySegment<ITestCaseSourceItem> items, HttpClient client)
		{
			ITestCaseSourceItemResult[] results = new ITestCaseSourceItemResult[items.Count];
			for (int i = 0; i < items.Count; i++)
			{
				var item = items[i];
				results[i] = await item.RunTest(client).ConfigureAwait(false);
			}

			return results;
		}
	}
}
