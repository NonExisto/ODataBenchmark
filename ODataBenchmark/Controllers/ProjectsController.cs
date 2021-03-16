using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ODataBenchmark.DataModel;
using System.Linq;

namespace ODataBenchmark.Controllers
{
	public class ProjectsController : ControllerBase
	{
		private readonly BenchmarkContext _benchmarkContext;

		public ProjectsController(BenchmarkContext benchmarkContext)
		{
			_benchmarkContext = benchmarkContext;
		}

		[HttpGet]
		[EnableQuery(MaxExpansionDepth = 4)]
		public IActionResult Get()
		{
			return Ok(_benchmarkContext.Projects);
		}

		[HttpGet]
		[EnableQuery]
		public IActionResult Get(long key)
		{
			var value = _benchmarkContext.Projects.FirstOrDefault(p => p.Id == key);
			if (value == null)
			{
				return NotFound($"Not found entity with key {key}");
			}
			return base.Ok(value);
		}
	}
}
