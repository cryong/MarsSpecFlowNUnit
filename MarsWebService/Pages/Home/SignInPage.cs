using MarsFramework.WebDriver;
using OpenQA.Selenium;
using MarsWebService.Pages.Profile;

namespace MarsWebService.Pages.Home
{
    public class SignInPage : CloseableForm
    {
        private By Email => By.Name("email");
        private By Password => By.Name("password");
        private By LoginButton => By.XPath("//button[.='Login']");

        public SignInPage(Driver driver) : base(driver)
        {
        }

        public void EnterCredentials(string username, string password)
        {
            Driver.Enter(Email, username);
            Driver.Enter(Password, password);
        }

        public ProfilePage ClickLogInButton()
        {
            Driver.Click(LoginButton);
            return new ProfilePage(Driver);
        }

        public ProfilePage LogIn(string username, string password)
        {
            EnterCredentials(username, password);
            return ClickLogInButton();
        }

        public override bool IsOpen()
        {
            return Driver.FindElement(Email).Displayed;
        }
    }
}
