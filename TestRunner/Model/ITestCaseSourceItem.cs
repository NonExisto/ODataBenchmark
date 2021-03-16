using System.Threading.Tasks;

namespace TestRunner.Model
{
	public interface ITestCaseSourceItem
	{
		int Order { get; }
		Task<long> RunTest(IHostingConfiguration hostingConfiguration);
	}
}
