using System;
using System.Collections.Generic;

namespace TestRunner.Model.TestCases
{
	public class AllProjects : ITestCase
	{
		string ITestCase.Name => "All Projects, level 2 expands, average size collection";

		IEnumerable<ITestCaseSource> ITestCase.GetTestSources()
		{
			yield return new ApiTestCaseSource();
			yield return new ODataTestCaseSource();
		}

		private class ApiTestCaseSource : ITestCaseSource
		{
			TestCaseType ITestCaseSource.TestCaseType => TestCaseType.Api;
			IEnumerable<ITestCaseSourceItem> ITestCaseSource.GetTestCaseItems(int size, int seed)
			{
				Random rnd = new();
				for (int i = 0; i < size; i++)
				{
					yield return new DefaultTestCaseSourceItem(rnd.Next(), "api/test/AllProjects");
				}
			}
		}

		private class ODataTestCaseSource : ITestCaseSource
		{
			TestCaseType ITestCaseSource.TestCaseType => TestCaseType.Odata;

			IEnumerable<ITestCaseSourceItem> ITestCaseSource.GetTestCaseItems(int size, int seed)
			{
				Random rnd = new();
				for (int i = 0; i < size; i++)
				{
					yield return new DefaultTestCaseSourceItem(rnd.Next(), "odata/projects?$expand=Members,Owners,Scopes,Superviser($expand=JobTitles)");
				}
			}
		}
	}
}
