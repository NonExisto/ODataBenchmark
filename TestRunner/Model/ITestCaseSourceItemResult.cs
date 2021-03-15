namespace TestRunner.Model
{
	public interface ITestCaseSourceItemResult
	{
		long Duration { get; }
		long PayloadSize { get; }

		ITestCaseSourceItem TestCaseSourceItem { get; }
	}
}
