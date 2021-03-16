using System.Collections.Generic;

namespace TestRunner.Model
{
	public interface ITestCaseSource
	{
		ITestCase TestCase { get; }
		TestCaseType TestCaseType { get; }
		IEnumerable<ITestCaseSourceItem> GetTestCaseItems(int size, int seed);
	}
}
