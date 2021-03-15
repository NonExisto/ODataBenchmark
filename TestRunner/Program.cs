using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TestRunner.Model;
using TestRunner.Model.TestCases;
using System.Net.Http.Headers;

namespace TestRunner
{
	internal static class Program
	{
		private const string _host = "http://localhost:26649/";
		public async static Task Main()
		{
			await Task.Delay(5500).ConfigureAwait(false);
			HttpClient client = new();
			HttpRequestMessage startRequest = new(HttpMethod.Get, $"{_host}api/test/StartTest");
			startRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("plain/text"));
			var resp = await client.SendAsync(startRequest).ConfigureAwait(false);
			resp.EnsureSuccessStatusCode();
			var lenValue = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

			int seedSize = int.Parse(lenValue);

			TestCase[] testCases = new[] { new AllScopes(seedSize) };

			var tests = testCases.SelectMany(tc => tc.GetTestSources().SelectMany(ts => ts.GetTestCaseItems(1000))).OrderBy(tsi => tsi.Order).ToArray();
			var split = Environment.ProcessorCount >> 2;

			var blockLen = tests.Length / split;
			var tasks = Enumerable.Range(0, split).Select(idx => new ArraySegment<ITestCaseSourceItem>(tests, idx * blockLen, (idx + 1) * blockLen))
				.Select(segment => RunTests(segment)).ToArray();
			await Task.WhenAll(tasks).ConfigureAwait(false);
		}

		private static async Task<ITestCaseSourceItemResult[]> RunTests(ArraySegment<ITestCaseSourceItem> items)
		{
			ITestCaseSourceItemResult[] results = new ITestCaseSourceItemResult[items.Count];
			for (int i = 0; i < items.Count; i++)
			{
				var item = items[i];
				results[i] = await item.RunTest().ConfigureAwait(false);
			}

			return results;
		}
	}
}
