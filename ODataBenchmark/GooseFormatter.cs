using Microsoft.AspNetCore.Mvc.Formatters;
using System.Threading.Tasks;

namespace ODataBenchmark
{
	public class GooseFormatter : IOutputFormatter
	{
		bool IOutputFormatter.CanWriteResult(OutputFormatterCanWriteContext context)
		{
			return context.HttpContext.Request.Method == "Get";
		}

		Task IOutputFormatter.WriteAsync(OutputFormatterWriteContext context)
		{
			context.HttpContext.Response.ContentType = "text/goose";
			context.HttpContext.Response.Body.Write(new byte[] {  71, 97, 71, 97 } );
			return Task.CompletedTask;
		}
	}
}
