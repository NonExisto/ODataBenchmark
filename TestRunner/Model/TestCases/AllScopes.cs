using System;
using System.Collections.Generic;

namespace TestRunner.Model.TestCases
{
	public class AllScopes : ITestCase
	{
		string ITestCase.Name => "All Scopes, no expands, average size collection";

		IEnumerable<ITestCaseSource> ITestCase.GetTestSources()
		{
			throw new NotImplementedException();
		}
	}
}
