using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ODataBenchmark.DataModel;
using System.Linq;

namespace ODataBenchmark.Controllers
{
	public class WorkItemsController : ControllerBase
	{
		private readonly BenchmarkContext _benchmarkContext;

		public WorkItemsController(BenchmarkContext benchmarkContext)
		{
			_benchmarkContext = benchmarkContext;
		}

		[HttpGet]
		[EnableQuery]
		public IActionResult Get()
		{
			return Ok(_benchmarkContext.WorkItems);
		}

		[HttpGet]
		[EnableQuery]
		public IActionResult Get(long key)
		{
			var value = _benchmarkContext.WorkItems.FirstOrDefault(p => p.Id == key);
			if (value == null)
			{
				return NotFound($"Not found entity with key {key}");
			}
			return base.Ok(value);
		}
	}
}