using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using MarsFramework.Utilities;

namespace MarsFramework.Contexts
{
    public class ExtentReportContext
    {
        static ExtentReportContext() {
            var reporter = new ExtentHtmlReporter(PathUtil.GetCurrentPath("Reports\\")); //TestContext.CurrentContext.TestDirectory // some configuration
            reporter.Config.Theme = Theme.Dark;
            //_instance = new ExtentReports();
            Instance.AttachReporter(reporter);
        }

        private ExtentReportContext() { } // prevent instantiation elsewhere

        public static ExtentReports Instance { get; } = new ExtentReports();
    }
}
