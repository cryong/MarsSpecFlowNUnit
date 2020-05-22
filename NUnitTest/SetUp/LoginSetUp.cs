using MarsFramework.WebDriver;
using MarsWebService.Pages.Home;
using NUnit.Framework;

namespace NUnitTest.SetUp
{
    public class LoginSetUp : BaseSetUp
    {
        [SetUp]
        public override void StartDriver()
        {
            base.StartDriver();
            SignInPage signInPage = new HomePage(Driver).OpenLoginForm();
            signInPage.EnterCredentials(ValidCredentials.Username, ValidCredentials.Password);
            signInPage.ClickLogInButton();
            Driver.WaitForAjax();
        }
    }
}
