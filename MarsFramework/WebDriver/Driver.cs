using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace MarsFramework.WebDriver
{
    public class Driver // wrapper for IWebDriver
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public Driver() { }

        public Driver(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)) // could potentially make this configurable
            {
                Message = $"Something has gone wrong while trying to interact with element(s)"
            };
            _wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
        }

        public void Click(By locator)
        {
            _wait.Until((d) =>
            {
                var element = d.FindElement(locator);
                if (element != null && element.Enabled)
                {
                    // not really checking for visibility
                    // for detailed explanations read comments for FindElements method below
                    return element;
                }
                return null;
            }).Click();
        }

        public IWebElement FindElement(By locator)
        {
            return _wait.Until((d) =>
            {
                var element = d.FindElement(locator);
                if (element != null && element.Displayed)
                {
                    return element;
                }
                return null;
            });
        }

        public List<IWebElement> FindElements(By locator)
        {
            try
            {
                return _wait.Until(
                    (d) =>
                    {
                        try
                        {
                            var elements = d.FindElements(locator);
                            // I wasn't able to find any nice solution for dealing with checkboxes
                            // It seems to be 'enabled' and yet 'not visible'
                            // so finding checkbox elements ends up returning null when you simply check for visibility
                            // quick and dirty hack to make it work by checking for for either element is visible or enabled
                            if (elements.Any(element => !element.Displayed && !element.Enabled))
                            {
                                return null;
                            }
                            return elements.Any() ? elements.ToList() : null;
                        }
                        catch (StaleElementReferenceException)
                        {
                            return null;
                        }
                    });

            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }

        public void GoToUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public void Quit()
        {
            _driver.Quit();
        }

        public void WaitForAjax()
        {
            var js = (IJavaScriptExecutor)_driver;
            _wait.Until(wd => js.ExecuteScript("return jQuery.active").ToString() == "0");
        }

        public string ExecuteJavaScript(string javascript)
        {
            var js = (IJavaScriptExecutor)_driver;
            return _wait.Until(wd => js.ExecuteScript(javascript).ToString());
        }

        public void WaitForPageLoad()
        {
            var js = (IJavaScriptExecutor)_driver;
            _wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        }
        public void Enter(By locator, string value)
        {
            var element = FindElement(locator);
            element.Clear();
            element.SendKeys(value);
        }

        public string GetCurrentUrl()
        {
            WaitForAjax();
            WaitForPageLoad();
            return _driver.Url;
        }

        public Screenshot TakeScreenShot()
        {
            return _driver.TakeScreenshot();
        }

        public void SelectOption(SelectDropDownOption option, object value, By locator)
        {
            var select = new SelectElement(FindElement(locator));
            switch (option)
            {
                case SelectDropDownOption.INDEX:
                    select.SelectByIndex((int)value);
                    break;
                case SelectDropDownOption.TEXT:
                    select.SelectByText((string)value);
                    break;
                case SelectDropDownOption.VALUE:
                    select.SelectByValue((string)value);
                    break;
                default:
                    throw new ArgumentException($"Unknown {nameof(option)} was provided '{option:G}' with value {value}");
            }
        }
    }

    public enum SelectDropDownOption
    {
        VALUE,
        TEXT,
        INDEX
    }
}
