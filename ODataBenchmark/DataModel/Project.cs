using System.Collections.Generic;

namespace ODataBenchmark.DataModel
{
	public class Project : Entity
	{
		public string Name { get; set; }

		public virtual IList<Customer> Owners { get; set; }

		public virtual Manager Superviser { get; set; }

		public long SuperviserId { get; set; }

		public virtual IList<Scope> Scopes { get; set; }

		public IList<Employee> Members { get; set; }
	}
}
