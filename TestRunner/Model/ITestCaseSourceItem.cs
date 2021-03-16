﻿using System.Net.Http;
using System.Threading.Tasks;

namespace TestRunner.Model
{
	public interface ITestCaseSourceItem
	{
		float Order { get; }

		ITestCaseSource TestCaseSource { get; }

		ITestCase TestCase { get; }

		Task<ITestCaseSourceItemResult> RunTest(HttpClient client);
	}
}