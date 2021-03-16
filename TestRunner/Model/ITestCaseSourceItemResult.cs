namespace TestRunner.Model
{
	public class TestCaseSourceItemResult
	{
		public long Duration { get; init; }
		public long PayloadSize { get; init; }

		public ITestCaseSourceItem TestCaseSourceItem { get; init; }
		public ITestCase TestCase { get; init; }
		public ITestCaseSource TestSource { get; init; }
	}
}
