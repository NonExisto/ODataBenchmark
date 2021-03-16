using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODataBenchmark.DataModel;
using System.Diagnostics;
using System.Linq;

namespace ODataBenchmark.Controllers
{
	[Route("api/{controller}/{action}/{id?}")]
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
		public IActionResult AllScopes()
		{
			return Ok(_benchmarkContext.Scopes.Select(c => new
			{
				c.Id,
				c.Name,
				c.Description
			}));
		}

		[HttpGet]
		public IActionResult AllWorkItems()
		{
			return Ok(_benchmarkContext.WorkItems.Select(c => new
			{
				c.Id,
				c.Title,
				c.Description,
				c.ProjectId,
				c.WorkItemState,
				c.WorkItemType
			}));
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
			return Ok(_benchmarkContext.Projects.Include(c => c.Members).Include(c => c.Owners).Include(c => c.Scopes)
				.Include(c => c.Superviser).ThenInclude(s => s.JobTitles)
				.Select(p => new
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

		[HttpGet]
		public IActionResult Project(long id)
		{
			return Ok(_benchmarkContext.Projects.Where(p => p.Id == id).Include(c => c.Members).Include(c => c.Owners).Include(c => c.Scopes)
				.Include(c => c.Superviser).ThenInclude(s => s.JobTitles)
				.Select(p => new
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



		[HttpGet]
		public IActionResult DeepProject(long id)
		{
			return Ok(_benchmarkContext.Projects.Where(p => p.Id == id).Include(c => c.Scopes)
				.Include(c => c.Superviser).ThenInclude(s => s.JobTitles).ThenInclude(j => j.JobClassification)
				.Include(p => p.Superviser).ThenInclude(s => s.Managers).ThenInclude(m => m.Subordinates)
				.Select(p => new
				{
					p.Id,
					p.Name,
					p.SuperviserId,
					Scopes = p.Scopes.Select(s => new { s.Id, s.Name, s.Description }),
					Superviser = new
					{
						p.Superviser.Id,
						p.Superviser.FirstName,
						p.Superviser.LastName,
						p.Superviser.PhoneNumber,
						JobTitles = p.Superviser.JobTitles.Select(j => new
						{
							j.Id,
							j.Name,
							JobClassification = new { j.JobClassification.Id, j.JobClassification.Name }
						}),
						Managers = p.Superviser.Managers.Select(m => new
						{
							m.Id,
							m.FirstName,
							m.LastName,
							m.BirthDate,
							m.PhoneNumber,
							Subordinates = m.Subordinates.Select(sb => new { sb.Id, sb.FirstName, sb.LastName, sb.PhoneNumber })
						})

					}
				}));
		}
	}
}
