using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ODataBenchmark.DataModel
{
	public static class EdmModelBuilder
	{
		public static IEdmModel GetEdmModel()
		{
			var builder = new ODataConventionModelBuilder();
			builder.EntitySet<Employee>("Employees");
			builder.EntitySet<Person>("People");
			builder.EntitySet<Customer>("Customers");
			builder.EntitySet<Project>("Projects");
			builder.EntitySet<Scope>("Scopes");
			builder.EntitySet<Manager>("Managers");
			builder.EntitySet<JobTitle>("JobTitles");
			builder.EntitySet<JobClassification>("JobClassifications");

			return builder.GetEdmModel();
		}
	}
}
