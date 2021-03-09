using Bogus;
using System.Collections.Generic;
using System.Linq;

namespace ODataBenchmark.DataModel
{
	public class FakeData
	{
		public IList<Scope> Scopes { get; private set; }
		public IList<JobClassification> JobClassifications { get; private set; }
		public IList<JobTitle> JobTitles { get; private set; }
		public IList<Emploee> Emploees { get; private set; }
		public IList<Manager> Managers { get; private set; }
		public IList<Customer> Customers { get; private set; }
		public IList<Project> Projects { get; private set; }
		public FakeData(int count)
		{
			FillScopes(count * 5);
			FillJobClassifications(16);
			FillJobTitles(count >> 2);
			FillEmploees(count);
			FilManagers(count >> 3);
			FilCustomers(count >> 1);
			FillProjects(count >> 1);
		}

		private void FillProjects(int count)
		{
			var id = 5L;
			var faker = new Faker<Project>()
			   .RuleFor(p => p.Id, _ => id++)
			   .RuleFor(p => p.Name, f => f.Commerce.Product())
			   .RuleFor(p => p.Scopes, f => f.PickRandom(Scopes, f.Random.Number(1, 8)))
			   .RuleFor(p => p.Members, f => f.PickRandom(Emploees, f.Random.Number(1, 12)))
			   .RuleFor(p => p.Superviser, f => f.PickRandom(Managers))
			   .RuleFor(p => p.Owners, f => f.PickRandom(Customers, f.Random.Number(1,3)));

			Projects = faker.Generate(count);
		}

		private void FilCustomers(int count)
		{
			var id = 35000L;
			var faker = new Faker<Customer>()
			   .RuleFor(p => p.Id, _ => id++)
			   .RuleFor(p => p.FirstName, f => f.Person.FirstName)
			   .RuleFor(p => p.LastName, f => f.Person.LastName)
			   .RuleFor(p => p.Login, f => f.Person.Email)
			   .RuleFor(p => p.PasswordHash, "N/A");

			Customers = faker.Generate(count);
		}

		private void FilManagers(int count)
		{
			var id = 25000L;
			var faker = new Faker<Manager>()
			   .RuleFor(p => p.Id, _ => id++)
			   .RuleFor(p => p.FirstName, f => f.Person.FirstName)
			   .RuleFor(p => p.LastName, f => f.Person.LastName)
			   .RuleFor(p => p.JobTitles, f => f.PickRandom(JobTitles, f.Random.Number(1, 3)))
			   .RuleFor(p => p.PhoneNumber, f => f.Person.Phone)
			   .RuleFor(p => p.Subordinates, f => f.PickRandom(Emploees, f.Random.Number(3, 12)))
			   .RuleFor(p => p.HomeAddress, f => new Address { City = f.Address.City(), Street = f.Address.StreetAddress() });

			Managers = faker.Generate(count);
			var topManager = faker.Generate(1)[0];
			topManager.Subordinates = Managers.Cast<Emploee>().ToList();
			Managers.Add(topManager);
		}

		private void FillEmploees(int count)
		{
			var id = 15000L;
			var faker = new Faker<Emploee>()
			   .RuleFor(p => p.Id, _ => id++)
			   .RuleFor(p => p.FirstName, f => f.Person.FirstName)
			   .RuleFor(p => p.LastName, f => f.Person.LastName)
			   .RuleFor(p => p.JobTitles, f => f.PickRandom(JobTitles, f.Random.Number(1, 3)))
			   .RuleFor(p => p.PhoneNumber, f => f.Person.Phone)
			   .RuleFor(p => p.HomeAddress, f => new Address { City = f.Address.City(), Street = f.Address.StreetAddress()});

			Emploees = faker.Generate(count);
		}

		private void FillJobTitles(int count)
		{
			var id = 5000L;
			var faker = new Faker<JobTitle>()
			   .RuleFor(p => p.Id, _ => id++)
			   .RuleFor(p => p.Name, f => f.Vehicle.Model())
			   .RuleFor(p => p.JobClassification, f => f.PickRandom(JobClassifications))
			   .RuleFor(p => p.JobClassificationId, (_, e) => e.JobClassification.Id)
			   .RuleFor(p => p.Scopes, f => f.PickRandom(Scopes, f.Random.Number(5)));

			JobTitles = faker.Generate(count);
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
