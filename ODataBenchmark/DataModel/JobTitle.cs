﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODataBenchmark.DataModel
{
	public class JobTitle
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public virtual JobClassification JobClassification { get; set; }

		public long JobClassificationId { get; set; }

		public virtual IList<Scope> Scopes { get; set; }

		public virtual IList<Emploee> Emploees { get; set; }
	}
}
