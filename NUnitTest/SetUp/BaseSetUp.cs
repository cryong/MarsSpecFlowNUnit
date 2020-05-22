using System;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using MarsCommonFramework.DataSetup;
using MarsFramework.Config;
using MarsFramework.Model;
using MarsFramework.Service;
using MarsFramework.Utilities;
using MarsFramework.WebDriver;
using MarsWebService.Pages.Home;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnitTest.Context;
using OpenQA.Selenium;
using static NUnit.Framework.TestContext;

namespace NUnitTest.SetUp
{
    public abstract class BaseSetUp : TestBase
    {
        protected Driver Driver;
        protected DataSetUpContext _setUpContext; // fixture level parallelization
        public BaseSetUp()
        {
            // force children/subclasses to have a constructor for creating extent report test instance
            // currenly ClassName prints namespace + test clsasname
            parentTest = ExtentTestManager.CreateTest(CurrentContext.Test.ClassName);
            _setUpContext = new DataSetUpContext();
        }

        [SetUp]
        public virtual void StartDriver()
        {
            TestAdapter currentTest = CurrentContext.Test;
            childTest = ExtentTestManager.CreateMethod(parentTest.Model.Name, $"{currentTest.Name}");
            DriverManager.StartDriver(TestConfig.Browser);
            Driver = DriverManager.GetDriver();
            HomePage homePage = new HomePage(Driver);
            homePage.Open();
        }

        private MediaEntityModelProvider Capture(string screenshotName)
        {
            try
            {
                var screenshot = SaveScreenShot.SaveScreenshot(Driver, screenshotName);
                if (screenshot != null)
                {
                    return MediaEntityBuilder.CreateScreenCaptureFromPath(screenshot).Build();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        [TearDown]
        public void CloseDriver()
        {
            try
            {
                var status = CurrentContext.Result.Outcome.Status;
                var stacktrace = CurrentContext.Result.StackTrace;
                var errorMessage = CurrentContext.Result.Message;
                var screenShot = Capture($"{CurrentContext.Test.ClassName}_{CurrentContext.Test.ID}");
                Status logStatus;
                string message = $"Test ended with ";
                switch (status)
                {
                    case TestStatus.Failed:
                        logStatus = Status.Fail;
                        message += $"{logStatus} - {errorMessage}";
                        childTest.Log(logStatus, $"Test ended with {logStatus} - {errorMessage}");
                        break;
                    case TestStatus.Skipped:
                        logStatus = Status.Skip;
                        message += $"{logStatus}";
                        break;
                    default:
                        logStatus = Status.Pass;
                        message += $"{logStatus} ";
                        break;
                }
                childTest.Log(logStatus, message, screenShot);
                // remove data
                DataSetUpHelper helper = new DataSetUpHelper(ValidCredentials.Username, ValidCredentials.Password);
                foreach (var obj in _setUpContext.GetObejcts())
                {
                    helper.Delete(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DriverManager.CloseDriver();
            }
        }
    }

    public class SaveScreenShot
    {
        public static string SaveScreenshot(Driver driver, string ScreenShotFileName)
        {
            var folderLocation = PathUtil.GetCurrentPath(TestConfig.ScreenShotPath);
            if (!System.IO.Directory.Exists(folderLocation))
            {
                System.IO.Directory.CreateDirectory(folderLocation);
            }
            var screenShot = driver.TakeScreenShot();
            var fileName = $"{folderLocation}\\{ScreenShotFileName}{DateTime.Now:_dd-mm-yyyy_mss}.jpeg";
            screenShot.SaveAsFile(fileName.ToString(), ScreenshotImageFormat.Jpeg);
            return fileName.ToString();
        }
    }
}
