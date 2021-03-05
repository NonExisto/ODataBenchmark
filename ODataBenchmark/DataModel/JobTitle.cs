using System.Collections.Generic;

namespace ODataBenchmark.DataModel
{
	public class JobTitle
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public virtual JobClassification JobClassification { get; set; }

		public virtual IList<Scope> Scopes { get; set; }

		public virtual IList<Emploee> Emploees { get; set; }
	}
}
