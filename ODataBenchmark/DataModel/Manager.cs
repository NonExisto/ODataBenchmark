using System.Collections.Generic;

namespace ODataBenchmark.DataModel
{
	public class Manager : Emploee
	{
		public virtual IList<Emploee> Subordinates { get; set; }
		public virtual IList<Project> Supervise { get; set; }
	}
}
