using MarsFramework.WebDriver;
using OpenQA.Selenium;

namespace MarsWebService.Pages
{
    public abstract class CloseableForm : IPage
    {
        protected readonly Driver Driver;
        public CloseableForm(Driver driver)
        {
            Driver = driver;
        }

        public void CloseForm()
        {
            Driver.FindElement(By.XPath("//body")).SendKeys(Keys.Escape);
        }
        public abstract bool IsOpen();
    }
}
