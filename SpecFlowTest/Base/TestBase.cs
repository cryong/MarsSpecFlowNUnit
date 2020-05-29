using AventStack.ExtentReports;
using BoDi;
using MarsFramework.Contexts;
using TechTalk.SpecFlow;
using MarsFramework.WebDriver;
using TechTalk.SpecFlow.Bindings;
using System;
using MarsFramework.Utilities;
using MarsFramework.Config;
using OpenQA.Selenium;
using MarsWebService.Model;
using MarsFramework.Factory;
using System.Linq;
using MarsWebService.Pages.Home;
using MarsCommonFramework.DataSetup;
using MarsFramework.Service;
using SpecFlowTest.Utilities;
using System.Collections.Generic;

namespace SpecFlowTest.Base
{
    [Binding]
    public class TestBase
    {
        [ThreadStatic]
        private static ExtentTest _feature;
        [ThreadStatic]
        private static ExtentTest _scenario;
        public static Credentials Credentials { get; private set; } // same credentials for all the tests
        //static TestBase() { }

        private readonly IObjectContainer _ioc;
        public TestBase(IObjectContainer ioc)
        {
            _ioc = ioc;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            new FrameworkContext().Initialise();
            ExcelDataReaderUtil.LoadWorsheet(PathUtil.GetCurrentPath($"{TestConfig.TestDataPath}Mars.xlsx"), "Login");
            ExcelData loginData = ExcelDataReaderUtil.FetchRowUsingKey("TestUser");
            Credentials = ObjectFactory.CreateInstance<Credentials>(loginData);
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            var featureName = featureContext.FeatureInfo.Title;
            _feature = ExtentTestManager.CreateTest(new GherkinKeyword("Feature"), featureName); //name of feature file
            TryLoadingWorkSheetWithFeatureName(featureName);
            // initialise driver here instead of beforescenario hook?
        }

        private static void TryLoadingWorkSheetWithFeatureName(string featureName)
        {
            // Pre-emptively trying to load data for a particular feature
            // As long as there is worksheet that matches the name of the feature,
            // this will try and load it, if it doesn't exist then move on
            try
            {
                ExcelDataReaderUtil.LoadWorsheet(PathUtil.GetCurrentPath($"{TestConfig.TestDataPath}Mars.xlsx"), featureName);
            }
            catch (NullReferenceException)
            {
                // this means any data loading needs to occur in appropriate step definitions (or just elsewhere)
            }
        }

        [BeforeScenario(Order = 1)]
        public void StartDriver()
        {
            // this means that the browser type is not configuarable at scenario level but rather at test suite level
            DriverManager.StartDriver(TestConfig.Browser);
            _ioc.RegisterInstanceAs(DriverManager.GetDriver()); // inject driver instance
        }

        [BeforeScenario(Order = 2)]
        public void CreateTestMethod(ScenarioContext scenarioContext)
        {
            _scenario = ExtentTestManager.CreateMethod(_feature.Model.Name, new GherkinKeyword("Scenario"), scenarioContext.ScenarioInfo.Title);
        }

        [BeforeScenario(Order = 3)]
        public void DoSignIn(ScenarioContext _context, Driver driver)
        {
            if (!_context.ScenarioInfo.Tags.Contains("NoLogin"))
            {
                HomePage homePage = new HomePage(driver);
                homePage.Open();
                homePage.OpenLoginForm().LogIn(Credentials.Username, Credentials.Password);
                driver.WaitForAjax();
            }
        }

        [BeforeScenario(Order = 4)]
        public void PrepareForDataSetup(ScenarioContext scenarioContext)
        {
            var helper = new DataSetUpHelper(Credentials.Username, Credentials.Password); // try registering at feature level
            _ioc.RegisterInstanceAs(helper);
            scenarioContext.Set(new List<object>(), TestHelper.DeleteKey);
        }

        [BeforeStep]
        public void InsertReportingStepsBefore(ScenarioContext scenarioContext)
        {
            var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType;
            var scenarioTitle = _scenario.Model.Name;
            var step = scenarioContext.StepContext.StepInfo.Text;
            switch (stepType)
            {
                case StepDefinitionType.Given:
                    ExtentTestManager.CreateMethod(scenarioTitle, new GherkinKeyword("Given"), step);
                    break;
                case StepDefinitionType.When:
                    ExtentTestManager.CreateMethod(scenarioTitle, new GherkinKeyword("When"), step);
                    break;
                case StepDefinitionType.Then:
                    ExtentTestManager.CreateMethod(scenarioTitle, new GherkinKeyword("Then"), step);
                    break;
                default:
                    break;
            }
        }

        [AfterStep]
        public void InsertReportingSteps(ScenarioContext scenarioContext)
        {
            //OK = 0,
            //StepDefinitionPending = 1,
            //UndefinedStep = 2,
            //BindingError = 3,
            //TestError = 4,
            //Skipped = 5
            //ScenarioExecutionStatus stepStatus = scenarioContext.ScenarioExecutionStatus;

            //public enum Status
            //{
            //    Pass = 0,
            //    Fail = 1,
            //    Fatal = 2,
            //    Error = 3,
            //    Warning = 4,
            //    Info = 5,
            //    Skip = 6,
            //    Debug = 7
            //}
            if (scenarioContext.TestError != null)
            {
                ExtentTestManager.GetMethod()
                    .Fail(scenarioContext.TestError.Message)
                    .Log(Status.Error, scenarioContext.TestError.Message);
            }
        }

        [AfterScenario(Order = 1)]
        public void TakeScreenShot(Driver driver)
        {
            try
            {
                var screenshot = SaveScreenShot.SaveScreenshot(
                    driver,
                    $"{_feature.Model.Name}_{_scenario.Model.Name.Replace(" ", "_")}");
                if (screenshot != null)
                {
                    var m = MediaEntityBuilder.CreateScreenCaptureFromPath(screenshot).Build();
                    ExtentTestManager.GetMethod().Log(Status.Info, m);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AfterScenario(Order = 2)]
        public void SignOut(Driver driver)
        {
            driver.ExecuteJavaScript("return $('body').trigger({ type: 'keyup', which: 27 });");
            driver.ExecuteJavaScript("return $('button:contains(\"Sign Out\")').click();");

        }

        [AfterScenario(Order = 3)]
        public void DataCleanUp(ScenarioContext scenarioContext, DataSetUpHelper helper)
        {
            var objectsToBeDeleted = TestHelper.GetListOfObjectsToBeRemoved(scenarioContext);
            if (objectsToBeDeleted.Count > 0)
            {
                foreach (var objectToRemove in objectsToBeDeleted)
                {
                    helper.Delete(objectToRemove);
                }
            }
        }

        [AfterScenario(Order = 4)]
        public void QuitDriver()
        {
            DriverManager.CloseDriver();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            ExtentReportContext.Instance.Flush();
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
