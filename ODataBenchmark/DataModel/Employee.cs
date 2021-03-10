using System.Collections.Generic;

namespace ODataBenchmark.DataModel
{
	public class Employee : Person
	{
		public virtual IList<JobTitle> JobTitles { get; set; }
		public virtual IList<Project> WorksOn { get; set; }
		public virtual Address HomeAddress { get; set; }
		public string PhoneNumber { get; set; }
		public virtual IList<Manager> Managers { get; set; }
	}
}
