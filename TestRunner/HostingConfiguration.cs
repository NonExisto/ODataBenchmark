using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TestRunner
{
	public interface IHostingConfiguration
	{
		Task<HttpResponseMessage> SendAsync(string path, string accepts, Action<HttpRequestMessage> configure = null);
	}

	public class HostingConfiguration : IHostingConfiguration
	{
		private const string _host = "http://localhost:26649/";

		private readonly HttpClient _httpClient = new();

		Task<HttpResponseMessage> IHostingConfiguration.SendAsync(string path, string accepts, Action<HttpRequestMessage> configure)
		{
			HttpRequestMessage message = new(HttpMethod.Get, $"{_host}{path}");
			if (!string.IsNullOrEmpty(accepts))
			{
				message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accepts));
			}
			configure?.Invoke(message);
			return _httpClient.SendAsync(message);
		}
	}
}
