using System;

namespace TestRunner.Model
{
	public interface ITestCaseSourceItemResult
	{
		TimeSpan Duration { get; }
		int PayloadSize { get; }

		ITestCaseSourceItem TestCaseSourceItem { get; }
	}
}
