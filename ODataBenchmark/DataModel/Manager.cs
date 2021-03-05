using System.Collections.Generic;

namespace ODataBenchmark.DataModel
{
	public class Manager : Emploee
	{
		public virtual IList<Emploee> Subordinates { get; set; }
		public IList<Project> Supervise { get; set; }
	}
}
