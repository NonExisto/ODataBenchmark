using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ODataBenchmark.DataModel
{
	internal class FakeData
	{
		public IList<Scope> Scopes { get; private set; }
		public IList<JobClassification> JobClassifications { get; private set; }
		public IList<JobTitle> JobTitles { get; private set; }
		public IList<Employee> Employees { get; private set; }
		public IList<Manager> Managers { get; private set; }
		public IList<Customer> Customers { get; private set; }
		public IList<Project> Projects { get; private set; }
		public IList<Address> Addresses { get; private set; }
		public IList<EmployeeJobTitle> EmployeeJobTitles { get; private set; }
		public IList<JobScope> JobScopes { get; private set; }
		public IList<EmployeeManager> EmployeeManagers { get; private set; }
		public IList<ProjectMember> ProjectMembers { get; private set; }
		public IList<ProjectScope> ProjectScopes { get; private set; }
		public IList<ProjectOwner> ProjectOwners { get; private set; }
		public IList<WorkItem> WorkItems { get; private set; }

		private long _personId = 50;
		public FakeData(int count)
		{
			FillScopes(count * 5);
			FillJobClassifications(16);
			FillJobTitles(count >> 2);
			FillEmployees(count);
			FilCustomers(count >> 1);
			FillProjects(count >> 1);
			FillWorkItems(count << 8);
		}

		private void FillWorkItems(int count)
		{
			var witLen = Enum.GetNames(typeof(WorkItemType)).Length - 1;
			var wisLen = Enum.GetNames(typeof(WorkItemState)).Length - 1;
			var id = 200_000;
			WorkItems = new Faker<WorkItem>()
				.RuleFor(p => p.Id, _ => id++)
				.RuleFor(p => p.Title, f => f.Vehicle.Model())
				.RuleFor(p => p.Description, f => f.Vehicle.Vin())
				.RuleFor(p => p.ProjectId, f => f.PickRandom(Projects).Id)
				.RuleFor(p => p.WorkItemType, f => (WorkItemType)f.Random.Number(witLen))
				.RuleFor(p => p.WorkItemState, f => (WorkItemState)f.Random.Number(wisLen))
				.Generate(count);
		}

		private void FillProjects(int count)
		{
			var id = 5L;
			var faker = new Faker<Project>()
			   .RuleFor(p => p.Id, _ => id++)
			   .RuleFor(p => p.Name, f => f.Commerce.Product())
			   .RuleFor(p => p.Scopes, f => f.PickRandom(Scopes, f.Random.Number(1, 8)).ToArray())
			   .RuleFor(p => p.Members, f => f.PickRandom(Employees, f.Random.Number(1, 12)).ToArray())
			   .RuleFor(p => p.Superviser, f => f.PickRandom(Managers))
			   .RuleFor(p => p.SuperviserId, (_, e) => e.Superviser.Id)
			   .RuleFor(p => p.Owners, f => f.PickRandom(Customers, f.Random.Number(1, 3)).ToArray());

			List<Project> projects = faker.Generate(count);
			ProjectMembers = projects.SelectMany(p => p.Members.Select(m => new ProjectMember { MembersId = m.Id, WorksOnId = p.Id})).ToArray();
			ProjectScopes = projects.SelectMany(p => p.Scopes.Select(s => new ProjectScope { ScopesId = s.Id, ProjectScopesId = p.Id })).ToArray();
			ProjectOwners = projects.SelectMany(p => p.Owners.Select(c => new ProjectOwner { OwnersId = c.Id, OwnsId = p.Id })).ToArray();
			projects.ForEach(p => { p.Superviser = null; p.Members = null; p.Scopes = null; p.Owners = null; });
			Projects = projects;
		}

		private void FilCustomers(int count)
		{
			var faker = new Faker<Customer>()
			   .RuleFor(p => p.Id, _ => _personId++)
			   .RuleFor(p => p.FirstName, f => f.Person.FirstName)
			   .RuleFor(p => p.LastName, f => f.Person.LastName)
			   .RuleFor(p => p.Login, f => f.Person.Email)
			   .RuleFor(p => p.PasswordHash, "N/A");

			Customers = faker.Generate(count);
		}

		private void FillEmployees(int count)
		{
			var faker = new Faker<Employee>()
			   .RuleFor(p => p.Id, _ => _personId++)
			   .RuleFor(p => p.FirstName, f => f.Person.FirstName)
			   .RuleFor(p => p.LastName, f => f.Person.LastName)
			   .RuleFor(p => p.JobTitles, f => f.PickRandom(JobTitles, f.Random.Number(1, 3)).ToArray())
			   .RuleFor(p => p.PhoneNumber, f => f.Person.Phone);

			var employees = faker.Generate(count);

			var managerFaker = new Faker<Manager>()
			   .RuleFor(p => p.Id, _ => _personId++)
			   .RuleFor(p => p.FirstName, f => f.Person.FirstName)
			   .RuleFor(p => p.LastName, f => f.Person.LastName)
			   .RuleFor(p => p.JobTitles, f => f.PickRandom(JobTitles, f.Random.Number(1, 3)).ToArray())
			   .RuleFor(p => p.PhoneNumber, f => f.Person.Phone)
			   .RuleFor(p => p.Subordinates, f => f.PickRandom(employees, f.Random.Number(3, 12)).ToArray());

			var managers = managerFaker.Generate(count >> 3);
			var topManager = managerFaker.Generate(1)[0];
			topManager.Subordinates = managers.Cast<Employee>().ToList();
			managers.Add(topManager);

			Addresses = new Faker<Address>()
			.RuleFor(c => c.City, f => f.Address.City())
			.RuleFor(c => c.Street, f => f.Address.StreetAddress())
			.RuleFor(c => c.EmployeeId, f => _personId - f.IndexFaker)
			.Generate(employees.Count + managers.Count);

			EmployeeJobTitles = employees.SelectMany(e => e.JobTitles.Select(j => new EmployeeJobTitle { EmployeesId = e.Id, JobTitlesId = j.Id }))
				.Concat(managers.SelectMany(e => e.JobTitles.Select(j => new EmployeeJobTitle { EmployeesId = e.Id, JobTitlesId = j.Id })))
				.ToArray();
			EmployeeManagers = managers.SelectMany(m => m.Subordinates.Select(e => new EmployeeManager { SubordinatesId = e.Id, ManagersId = m.Id })).ToArray();

			employees.ForEach(e => e.JobTitles = null);
			managers.ForEach(m => { m.JobTitles = null; m.Subordinates = null; });

			Employees = employees;
			Managers = managers;
		}

		private void FillJobTitles(int count)
		{
			var id = 5000L;
			var faker = new Faker<JobTitle>()
			   .RuleFor(p => p.Id, _ => id++)
			   .RuleFor(p => p.Name, f => f.Vehicle.Model())
			   .RuleFor(p => p.JobClassification, f => f.PickRandom(JobClassifications))
			   .RuleFor(p => p.JobClassificationId, (_, e) => e.JobClassification.Id)
			   .RuleFor(p => p.Scopes, f => f.PickRandom(Scopes, f.Random.Number(1, 5)).ToArray());

			var jobTitles = faker.Generate(count);
			JobTitles = jobTitles;
			JobScopes = JobTitles.SelectMany(jt => jt.Scopes.Select(s => new JobScope { JobScopesId = jt.Id, ScopesId = s.Id })).ToArray();
			jobTitles.ForEach(jt => { jt.Scopes = null; jt.JobClassification = null; });
		}

		private void FillJobClassifications(int count)
		{
			var id = 2000L;
			var faker = new Faker<JobClassification>()
			   .RuleFor(p => p.Id, _ => id += 20)
			   .RuleFor(p => p.Name, f => f.Hacker.Noun());

			JobClassifications = faker.Generate(count);
		}

		private void FillScopes(int count)
		{
			var id = 1000L;
			var faker = new Faker<Scope>()
			   .RuleFor(p => p.Id, _ => id++)
			   .RuleFor(p => p.Name, f => f.Hacker.Phrase())
			   .RuleFor(p => p.Description, f => f.Lorem.Sentence());

			Scopes = faker.Generate(count);
		}
	}
}
