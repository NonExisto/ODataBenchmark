using Microsoft.AspNetCore.Mvc;
using ODataBenchmark.DataModel;
using System.Linq;

namespace ODataBenchmark.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		private readonly BenchmarkContext _benchmarkContext;

		public TestController(BenchmarkContext benchmarkContext)
		{
			_benchmarkContext = benchmarkContext;
		}

		[HttpGet]
		public IActionResult SampleData()
		{
			_benchmarkContext.Database.EnsureCreated();
			return Ok(_benchmarkContext.Employees.Count());
		}
	}
}
