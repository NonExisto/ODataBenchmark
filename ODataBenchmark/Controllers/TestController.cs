using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODataBenchmark.DataModel;
using System.Diagnostics;
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
		public IActionResult StartTest()
		{
			_benchmarkContext.Database.EnsureCreated();
			Debug.Assert(_benchmarkContext.Employees.Count() > _benchmarkContext.SeedSize);
			return Ok(_benchmarkContext.SeedSize);
		}

		[HttpGet]
		public IActionResult AllCustomers()
		{
			return Ok(_benchmarkContext.Customers.Include(c => c.Owns).Select(c => new
			{
				c.BirthDate,
				c.FirstName,
				c.LastName,
				c.Id,
				c.Login,
				c.PasswordHash,
				Owns = c.Owns.Select(o => new { o.Id, o.Name, o.SuperviserId })
			}));
		}

		[HttpGet]
		public IActionResult AllProjects()
		{
			return Ok(_benchmarkContext.Projects.Include(c => c.Members).Include(c => c.Owners).Include(c => c.Scopes).Include(c => c.Superviser).ThenInclude(s => s.JobTitles).
				Select(p => new
				{
					p.Id,
					p.Name,
					p.SuperviserId,
					Members = p.Members.Select(m => new { m.Id, m.FirstName, m.LastName, m.PhoneNumber }),
					Owners = p.Owners.Select(o => new { o.Id, o.FirstName, o.LastName, o.Login, o.PasswordHash }),
					Scopes = p.Scopes.Select(s => new { s.Id, s.Name, s.Description }),
					Superviser = new { p.Superviser.Id, p.Superviser.FirstName, p.Superviser.LastName, p.Superviser.PhoneNumber, JobTitles = p.Superviser.JobTitles.Select(j => new { j.Id, j.Name }) }
				}));
		}
	}
}
