using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ODataBenchmark.DataModel;
using System.Linq;

namespace ODataBenchmark.Controllers
{
	public class EmploeesController : ControllerBase
	{
		private readonly BenchmarkContext _benchmarkContext;

		public EmploeesController(BenchmarkContext benchmarkContext)
		{
			_benchmarkContext = benchmarkContext;
		}

		[HttpGet]
		[EnableQuery]
		public IActionResult Get()
		{
			return Ok(_benchmarkContext.Emploees);
		}

		[HttpGet]
		[EnableQuery]
		public IActionResult Get(long key)
		{
			var value = _benchmarkContext.Emploees.FirstOrDefault(p => p.Id == key);
			if (value == null)
			{
				return NotFound($"Not found entity with key {key}");
			}
			return base.Ok(value);
		}
	}
}
