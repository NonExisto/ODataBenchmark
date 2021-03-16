using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestRunner.Model
{
	public class DefaultTestCaseSourceItem : ITestCaseSourceItem
	{
		private readonly string _path;

		public DefaultTestCaseSourceItem(int order, string path)
		{
			Order = order;
			_path = path;
		}

		public int Order { get; }

		async Task<(long duration, long payloadSize)> ITestCaseSourceItem.RunTest(IHostingConfiguration hostingConfiguration)
		{
			HttpResponseMessage resp = null;
			Stopwatch stopwatch = new();
			stopwatch.Start();
			try
			{
				resp = await hostingConfiguration.SendAsync(_path, "application/json").ConfigureAwait(false);
			}
			catch
			{
			}
			stopwatch.Stop();
			return (stopwatch.ElapsedMilliseconds, resp?.Content.Headers.ContentLength.Value ?? 0);
		}
	}
}
