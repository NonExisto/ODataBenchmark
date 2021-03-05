using System.Collections.Generic;

namespace ODataBenchmark.DataModel
{
	public class Emploee : Person
	{
		public IList<JobTitle> JobTitles { get; set; }
		public IList<Project> WorksOn { get; set; }
		public virtual Address HomeAddress { get; set; }
		public string PhoneNumber { get; set; }
		public virtual Manager Manager { get; set; }
	}
}
