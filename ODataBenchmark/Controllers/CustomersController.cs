using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ODataBenchmark.DataModel;
using System.Linq;

namespace ODataBenchmark.Controllers
{
	public class CustomersController : ControllerBase
	{
		private readonly BenchmarkContext _benchmarkContext;

		public CustomersController(BenchmarkContext benchmarkContext)
		{
			_benchmarkContext = benchmarkContext;
		}

		[HttpGet]
		[EnableQuery]
		public IActionResult Get()
		{
			return Ok(_benchmarkContext.Customers);
		}

		[HttpGet]
		[EnableQuery]
		public IActionResult Get(long id)
		{
			var value = _benchmarkContext.Customers.FirstOrDefault(p => p.Id == id);
			if (value == null)
			{
				return NotFound($"Not found entity with key {id}");
			}
			return base.Ok(value);
		}
	}
}
