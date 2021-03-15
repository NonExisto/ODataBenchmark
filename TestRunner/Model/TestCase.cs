using System.Collections.Generic;

namespace TestRunner.Model
{
	public abstract class TestCase
	{
		protected TestCase(string name, int seedSize)
		{
			Name = name;
			SeedSize = seedSize;
		}

		public string Name { get; }
		public int SeedSize { get; }

		public abstract IEnumerable<ITestCaseSource> GetTestSources();
	}
}
