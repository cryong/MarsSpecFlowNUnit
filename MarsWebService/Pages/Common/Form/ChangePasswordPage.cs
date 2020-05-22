using MarsFramework.WebDriver;
using OpenQA.Selenium;

namespace MarsWebService.Pages.Common.Form
{
    public class ChangePasswordPage : CloseableForm
    {
        public ChangePasswordPage(Driver driver) : base (driver)
        {
        }

        private By CurrentPassword => By.Name("oldPassword");
        private By NewPassword => By.Name("newPassword");
        private By ConfirmPassword => By.Name("confirmPassword");
        private By SaveButton => By.XPath("//form[@autocomplete='new-password']/div/button[.='Save']");

        public void ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            Driver.Enter(CurrentPassword, currentPassword);
            Driver.Enter(NewPassword, newPassword);
            Driver.Enter(ConfirmPassword, confirmPassword);
            Driver.Click(SaveButton);   
        }

        public override bool IsOpen()
        {
            return Driver.FindElement(CurrentPassword).Displayed;
        }
    }
}
