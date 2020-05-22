using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace MarsFramework.Decorators
{
    public abstract class DriverDecorator : IDriver
    {
        protected readonly IDriver Driver;

        protected DriverDecorator(IDriver driver)
        {
            Driver = driver;
        }

        public virtual IWebElement FindElement(By locator)
        {
            return Driver?.FindElement(locator);
        }

        public virtual List<IWebElement> FindElements(By locator)
        {
            return Driver?.FindElements(locator);
        }

        public virtual void GoToUrl(string url)
        {
            Driver?.GoToUrl(url);
        }

        public virtual void Quit()
        {
            Driver?.Quit();
        }

        public virtual void Start(Browser browser)
        {
            Driver?.Start(browser);
        }

        public virtual void WaitForAjax()
        {
            Driver?.WaitForAjax();
        }

        public virtual void WaitForPageLoad()
        {
            Driver?.WaitForPageLoad();
        }
    }
}
