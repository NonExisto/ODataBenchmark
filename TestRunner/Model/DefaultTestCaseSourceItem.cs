using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestRunner.Model
{
	public class DefaultTestCaseSourceItem : ITestCaseSourceItem
	{
		private readonly string _path;
		private readonly string _accepts;

		public DefaultTestCaseSourceItem(int order, string path, string accepts = "application/json")
		{
			Order = order;
			_path = path;
			_accepts = accepts;
		}

		public int Order { get; }

		async Task<long> ITestCaseSourceItem.RunTest(IHostingConfiguration hostingConfiguration)
		{
			HttpResponseMessage resp = null;
			try
			{
				resp = await hostingConfiguration.SendAsync(_path, _accepts).ConfigureAwait(false);
			}
			catch(Exception ex)
			{
				Console.Error.Write("Failed to send request {0} with error: {1}", _path, ex);
			}
			return resp?.IsSuccessStatusCode == true ? resp.Content.Headers.ContentLength.Value : 0;
		}
	}
}
