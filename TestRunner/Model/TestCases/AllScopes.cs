using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestRunner.Model.TestCases
{
	public class AllScopes : ITestCase
	{
		string ITestCase.Name => "All Scopes, no expands, average size collection";

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
					yield return new TestCaseSourceItem(rnd.Next());
				}
			}

			private class TestCaseSourceItem : ITestCaseSourceItem
			{
				public TestCaseSourceItem(int order)
				{
					Order = order;
				}

				public int Order { get; }

				Task<(long duration, long payloadSize)> ITestCaseSourceItem.RunTest(IHostingConfiguration hostingConfiguration)
				{
					throw new NotImplementedException();
				}
			}
		}

		private class ODataTestCaseSource : ITestCaseSource
		{
			TestCaseType ITestCaseSource.TestCaseType => TestCaseType.Odata;

			IEnumerable<ITestCaseSourceItem> ITestCaseSource.GetTestCaseItems(int size, int seed)
			{
				throw new NotImplementedException();
			}
		}
	}
}
