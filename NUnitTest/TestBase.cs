using AventStack.ExtentReports;
using BoDi;
using MarsFramework.Contexts;
using MarsFramework.Model;
using MarsFramework.WebDriver;
using MarsFramework.Config;
using NUnit.Framework;
using MarsWebService.Model;
using MarsFramework.Utilities;
using MarsFramework.Factory;
using System;

namespace NUnitTest
{
    //[SetUpFixture]
    public class TestBase
    {
        protected static Credentials ValidCredentials { get; private set; } // could potentially be public
        [ThreadStatic]
        protected static ExtentTest parentTest;
        [ThreadStatic]
        protected static ExtentTest childTest;


        [OneTimeSetUp]
        public void BeforeTestRun()
        {
            // init framework
            new FrameworkContext().Initialise();
            /*
             * OneTimeSetUp methods run in the context of the TestFixture or SetUpFixture, which is separate from the context of any individual test cases. I
             */
            ExcelDataReaderUtil.LoadWorsheet(PathUtil.GetCurrentPath($"{TestConfig.TestDataPath}Mars.xlsx"), "Login");
            ExcelData loginData = ExcelDataReaderUtil.FetchRowUsingKey("TestUser");
            ValidCredentials = ObjectFactory.CreateInstance<Credentials>(loginData);
        }

        [OneTimeTearDown]
        public void AfterTestRun()
        {
            //Driver.Quit();
            ExtentReportContext.Instance.Flush();
        }
    }
}
