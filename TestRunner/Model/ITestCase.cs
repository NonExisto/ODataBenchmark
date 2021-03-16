using System.Collections.Generic;

namespace TestRunner.Model
{
	public interface ITestCase
	{
		string Name { get; }

		IEnumerable<ITestCaseSource> GetTestSources();
	}
}
