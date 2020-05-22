using System;
using MarsFramework.WebDriver;
using MarsFramework.SeleniumEventHandlers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Events;

namespace MarsFramework.Factory
{
    // consider making it into a builder instead for more clarity
    public class DriverFactory
    {

        //private Driver driver = new Driver();

        //public DriverFactory 

        //public Driver Build()
        //{
        //    return driver;
        //}

        public static IWebDriver Create(Browser browser, ISeleniumEventHandler eventHandler)
        {
            IWebDriver driver;

            switch (browser)
            {
                case Browser.Chrome:
                    driver = new ChromeDriver();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{nameof(Browser)} is currently not supported");
            }

            if (eventHandler != null)
            {
                driver = new EventFiringWebDriver(driver);
                AttachDriverEventListener(driver, eventHandler);
            }

            driver.Manage().Window.Maximize();
            return driver;
        }

        private static void AttachDriverEventListener(IWebDriver driver, ISeleniumEventHandler eventHandler)
        {
            ((EventFiringWebDriver)driver).Navigating += eventHandler.OnNavigating;
            ((EventFiringWebDriver)driver).Navigated += eventHandler.OnNavigated;
            ((EventFiringWebDriver)driver).ElementClicking += eventHandler.OnElementClicking;
            ((EventFiringWebDriver)driver).ElementClicked += eventHandler.OnElementClicked;
            ((EventFiringWebDriver)driver).ElementValueChanging += eventHandler.OnElementValueChanging;
            ((EventFiringWebDriver)driver).ElementValueChanged += eventHandler.OnElementValueChanged;
            ((EventFiringWebDriver)driver).FindingElement += eventHandler.OnFindingElement;
            ((EventFiringWebDriver)driver).FindElementCompleted += eventHandler.OnFindElementCompleted;
            ((EventFiringWebDriver)driver).ExceptionThrown += eventHandler.OnExceptionThrown;
        }
    }
}
