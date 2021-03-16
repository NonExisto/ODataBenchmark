using System;
using System.Collections.Generic;

namespace TestRunner.Model.TestCases
{
	public class SingleProject : ITestCase
	{
		string ITestCase.Name => "Single Project by Id, level 2 expands";

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
				var maxValue = (seed >> 1) + 5;
				for (int i = 0; i < size; i++)
				{
					yield return new DefaultTestCaseSourceItem(rnd.Next(), $"api/test/Project/{rnd.Next(maxValue) + 5}");
				}
			}
		}

		private class ODataTestCaseSource : ITestCaseSource
		{
			TestCaseType ITestCaseSource.TestCaseType => TestCaseType.Odata;

			IEnumerable<ITestCaseSourceItem> ITestCaseSource.GetTestCaseItems(int size, int seed)
			{
				Random rnd = new();
				var maxValue = (seed >> 1) + 5;
				for (int i = 0; i < size; i++)
				{
					yield return new DefaultTestCaseSourceItem(rnd.Next(), $"odata/projects?$expand=Members,Owners,Scopes,Superviser($expand=JobTitles)&$filter=Id eq {rnd.Next(maxValue) + 5}");
				}
			}
		}
	}
}
