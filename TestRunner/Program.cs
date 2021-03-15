using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestRunner
{
	internal static class Program
	{
		private const string _host = "http://localhost:26649/";
		public async static Task Main()
		{
			await Task.Delay(5500).ConfigureAwait(false);
			HttpClient client = new();
			HttpRequestMessage startRequest = new HttpRequestMessage(HttpMethod.Get, $"{_host}api/test/StartTest");
			startRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("plain/text"));
			var resp = await client.SendAsync(startRequest).ConfigureAwait(false);
			resp.EnsureSuccessStatusCode();
			var lenValue =  await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

			int seedSize = int.Parse(lenValue);
		}
	}
}
