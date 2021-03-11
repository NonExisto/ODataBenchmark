using System.Collections.Generic;

namespace ODataBenchmark.DataModel
{
	public class JobTitle : Entity
	{
		public string Name { get; set; }

		public virtual JobClassification JobClassification { get; set; }

		public long JobClassificationId { get; set; }

		public virtual IList<Scope> Scopes { get; set; }

		public virtual IList<Employee> Employees { get; set; }
	}
}
