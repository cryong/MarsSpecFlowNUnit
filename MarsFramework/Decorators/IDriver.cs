using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace MarsFramework.Decorators
{
    public interface IDriver
    {
        void Start(Browser browser);
        void Quit();
        void GoToUrl(string url);
        IWebElement FindElement(By locator);
        List<IWebElement> FindElements(By locator);
        void WaitForAjax();
        void WaitForPageLoad();
    }
}
