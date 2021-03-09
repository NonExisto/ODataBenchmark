using System.Collections.Generic;

namespace ODataBenchmark.DataModel
{
	public class Customer : Person
	{
		public string Login { get; set; }
		public string PasswordHash { get; set; }

		public virtual IList<Project> Owns { get; set; }
	}
}
