using System.Collections.Generic;

namespace ODataBenchmark.DataModel
{
	public class Manager : Employee
	{
		public virtual IList<Employee> Subordinates { get; set; }
		public virtual IList<Project> Supervise { get; set; }
	}
}
