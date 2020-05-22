using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoDi;
using MarsFramework.Config;
using MarsFramework.Driver;
using MarsFramework.Factory;
using MarsFramework.SeleniumEventHandlers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MarsFramework.Contexts
{
    public class WebDriverContext
    {
        public WebDriverContext(IObjectContainer container)
        {
            // maybe create a factory...
            //https://stackoverflow.com/questions/4822568/how-do-i-test-multiple-browsers-with-selenium-and-a-single-nunit-suite-and-keep
            //IWebDriver driver = DriverFactory.Create(TestConfig.Browser);

            // delay initialising

            Driver.Driver driver = new Driver.Driver(DriverFactory.Create(TestConfig.Browser, new ReportingSeleniumEventHandler()));
            //Driver driver = new ExtentReportDriver(new Driver());
            container.RegisterInstanceAs(driver);
        }
    }
}
