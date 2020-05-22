using System.Threading;
using AventStack.ExtentReports;
using MarsFramework.Contexts;
using MarsFramework.Model;

namespace MarsFramework.Service
{
    public class ExtentTestManager
	{
		private static ThreadLocal<TestHierarchyModel<ExtentTest>> _testHierarchy = new ThreadLocal<TestHierarchyModel<ExtentTest>>();
		private static ThreadLocal<ExtentTest> _parentTest = new ThreadLocal<ExtentTest>();
		private static ThreadLocal<ExtentTest> _childTest = new ThreadLocal<ExtentTest>();

		private static readonly object _synclock = new object();

		// creates a parent test
		public static ExtentTest CreateTest(string testName, string description = null)
		{
			lock (_synclock)
			{
				_parentTest.Value = ExtentReportContext.Instance.CreateTest(testName, description);
				_testHierarchy.Value = new TestHierarchyModel<ExtentTest>(_parentTest.Value);
				return _parentTest.Value;
			}
		}

		public static ExtentTest CreateTest(GherkinKeyword keyword, string testName, string description = null)
		{
			lock (_synclock)
			{
				_parentTest.Value = ExtentReportContext.Instance.CreateTest(keyword, testName, description);
				_testHierarchy.Value = new TestHierarchyModel<ExtentTest>(_parentTest.Value);
				return _parentTest.Value;
			}
		}

		public static ExtentTest CreateMethod(string parentName, string testName, string description = null)
		{
			lock (_synclock)
			{
				ExtentTest parentTest = _testHierarchy.Value.Search((t) => t.Model.Name, parentName);
				if (parentTest == null)
				{
					parentTest = CreateTest(testName);
				}
				_parentTest.Value = parentTest;
				_childTest.Value = parentTest.CreateNode(testName, description);
				_testHierarchy.Value.AddChild(_childTest.Value);
				return _childTest.Value;
			}
		}

		public static ExtentTest CreateMethod(string parentName, GherkinKeyword keyword, string testName, string description = null)
		{
			lock (_synclock)
			{
				//ExtentTest parentTest = _testHierarchy.Value.Search(parentName);
				ExtentTest parentTest = _testHierarchy.Value.Search((t) => t.Model.Name, parentName);
				if (parentTest == null)
				{
					parentTest = CreateTest(keyword, testName);
				}
				_parentTest.Value = parentTest;
				_childTest.Value = parentTest.CreateNode(keyword, testName, description);
				_testHierarchy.Value.AddChild(_childTest.Value);
				return _childTest.Value;
			}
		}

		public static ExtentTest GetMethod()
		{
			lock (_synclock)
			{
				return _childTest.Value;
			}
		}

		public static ExtentTest GetTest()
		{
			lock (_synclock)
			{
				return _parentTest.Value;
			}
		}
	}
}
