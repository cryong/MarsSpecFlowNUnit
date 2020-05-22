using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsFramework.Config;
using MarsFramework.WebDriver;
using OpenQA.Selenium;

namespace MarsWebService.Pages.Home
{
    public class HomePage : IPage
    {
        private readonly Driver _driver;
        private By SignInButton => By.XPath("//a[@class='item'][.='Sign In']");
        private By SignUpButton => By.XPath("//button[.='Join']");
        public HomePage(Driver driver)
        {
            _driver = driver;
        }

        public void Open()
        {
            _driver.GoToUrl(TestConfig.BaseURL);
        }

        public SignInPage OpenLoginForm()
        {
            _driver.Click(SignInButton);
            return new SignInPage(_driver);
        }

        public RegistrationPage OpenRegistrationForm()
        {
            _driver.Click(SignUpButton);
            return new RegistrationPage(_driver);
        }

    }
}
