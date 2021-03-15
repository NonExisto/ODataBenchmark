using System.Threading.Tasks;

namespace TestRunner.Model
{
	public interface ITestCaseSourceItem
	{
		float Order { get; }

		ITestCaseSource TestCaseSource { get; }

		TestCase TestCase { get; }

		Task<ITestCaseSourceItemResult> RunTest();
	}
}
