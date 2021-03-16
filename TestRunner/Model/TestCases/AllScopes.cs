using System;
using System.Collections.Generic;

namespace TestRunner.Model.TestCases
{
	public class AllScopes : ITestCase
	{
		string ITestCase.Name => "All Scopes, no expands, average size collection";

		IEnumerable<ITestCaseSource> ITestCase.GetTestSources()
		{
			yield return new ApiTestCaseSource(this);
			yield return new ODataTestCaseSource(this);
		}

		private class ApiTestCaseSource : ITestCaseSource
		{
			public ApiTestCaseSource(ITestCase testCase)
			{
				TestCase = testCase;
			}

			public ITestCase TestCase { get; }
			TestCaseType ITestCaseSource.TestCaseType => TestCaseType.Api;
			IEnumerable<ITestCaseSourceItem> ITestCaseSource.GetTestCaseItems(int size, int seed)
			{
				throw new NotImplementedException();
			}
		}

		private class ODataTestCaseSource : ITestCaseSource
		{
			public ODataTestCaseSource(ITestCase testCase)
			{
				TestCase = testCase;
			}

			TestCaseType ITestCaseSource.TestCaseType => TestCaseType.Odata;

			public ITestCase TestCase { get; }

			IEnumerable<ITestCaseSourceItem> ITestCaseSource.GetTestCaseItems(int size, int seed)
			{
				throw new NotImplementedException();
			}
		}
	}
}
