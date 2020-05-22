using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;

namespace MarsFramework.Decorators
{
    public class Driver : IDriver
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        public IWebElement FindElement(By locator)
        {
            return _wait.Until((d) => d.FindElement(locator));
        }

        public List<IWebElement> FindElements(By locator)
        {
            return _wait.Until(
                (d) =>
                {
                    try
                    {
                        var elements = d.FindElements(locator);
                        return elements.Any() ? elements.ToList() : null;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return null;
                    }
                });
        }

        public void GoToUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public void Quit()
        {
            _driver.Quit();
        }

        // delaying driver initialisation until we are actually running our tests
        public void Start(Browser browser)
        {
            // leaving it the way it is for now, possibly revisit later
            // possibly creating abstract class and having this feature as a subclass
            _driver = new EventFiringWebDriver(DriverFactory.Create(browser));
            
            // need to consider the message if wait fails
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
            {
                Message = $"Something has gone wrong while trying to interact with element(s)"
            };
            _wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
        }

        public void WaitForAjax()
        {
            var js = (IJavaScriptExecutor)_driver;
            _wait.Until(wd => js.ExecuteScript("return jQuery.active").ToString() == "0");
        }

        public void WaitForPageLoad()
        {
            var js = (IJavaScriptExecutor)_driver;
            _wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        }
    }
}
