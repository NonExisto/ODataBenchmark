using Bogus;
using System.Collections.Generic;

namespace ODataBenchmark.DataModel
{
	public class FakeData
	{
		public IList<Scope> Scopes { get; private set; }
		public IList<JobClassification> JobClassifications { get; private set; }
		public IList<JobTitle> JobTitles { get; set; }
		public FakeData(int count)
		{
			FillScopes(count * 5);
			FillJobClassifications(16);
			FillJobTitles(count<<2);
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
