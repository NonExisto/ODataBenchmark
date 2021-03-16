using System.Threading.Tasks;

namespace TestRunner.Model
{
	public interface ITestCaseSourceItem
	{
		int Order { get; }
		Task<(long duration, long payloadSize)> RunTest(IHostingConfiguration hostingConfiguration);
	}
}
