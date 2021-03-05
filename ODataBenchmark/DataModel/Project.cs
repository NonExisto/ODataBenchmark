using System.Collections.Generic;

namespace ODataBenchmark.DataModel
{
	public class Project
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public virtual IList<Customer> Owners { get; set; }

		public virtual Manager Superviser { get; set; }

		public virtual IList<Scope> Scopes { get; set; }

		public IList<Emploee> Members { get; set; }
	}
}
