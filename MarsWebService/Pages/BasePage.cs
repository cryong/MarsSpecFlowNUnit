using MarsFramework.WebDriver;
using OpenQA.Selenium;
using MarsWebService.Pages.Common;

namespace MarsWebService.Pages
{
    public abstract class BasePage : IPage
    {
        public HeaderBar Header { get; private set; }
        protected readonly Driver Driver;
        public HeaderBar HeaderBar { get; private set; }
        public SearchBar SearchBar { get; private set; }

        private By SuccessPopUp => By.XPath("//div[contains(@class, 'ns-type-success')]");
        private By ErrorPopUp => By.XPath("//div[contains(@class, 'ns-type-error')]");

        public BasePage(Driver driver)
        {
            Driver = driver;
            Header = new HeaderBar(driver);
            HeaderBar = new HeaderBar(driver);
            SearchBar = new SearchBar(driver);
        }

        public string GetSuccessPopUpMessage()
        {
            return Driver.FindElement(SuccessPopUp).Text;
        }

        public string GetErrorPopUpMessage()
        {
            return Driver.FindElement(ErrorPopUp).Text;
        }

        public virtual void WaitForPageLoad()
        {
            Driver.WaitForAjax();
            Driver.WaitForPageLoad();
        }
    }
}
