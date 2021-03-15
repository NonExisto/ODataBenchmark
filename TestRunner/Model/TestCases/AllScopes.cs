using System;
using System.Collections.Generic;

namespace TestRunner.Model.TestCases
{
	public class AllScopes : TestCase
	{
		public AllScopes(int seedSize) : base("All Scopes, average length, no expands ", seedSize)
		{
		}

		public override IEnumerable<ITestCaseSource> GetTestSources()
		{
			throw new NotImplementedException();
		}
	}
}
