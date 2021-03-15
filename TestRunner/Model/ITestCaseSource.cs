using System.Collections.Generic;

namespace TestRunner.Model
{
	public interface ITestCaseSource
	{
		TestCaseType TestCaseType { get; }
		IEnumerable<ITestCaseSourceItem> GetTestCaseItems(int size);
	}
}
