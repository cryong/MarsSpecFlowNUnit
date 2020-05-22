using MarsFramework.WebDriver;
using OpenQA.Selenium;

namespace MarsWebService.Pages.Home
{
    public class RegistrationPage : CloseableForm
    {
        private By FirstName => By.Name("firstName");
        private By LastName => By.Name("lastName");
        private By EmailAddress => By.Name("email");
        private By Password => By.Name("password");
        private By ConfirmPassword => By.Name("confirmPassword");
        private By TermsAndConditions => By.Name("terms");
        private By JoinButton => By.Id("submit-btn");

        public RegistrationPage(Driver driver) : base(driver)
        {
        }

        public void Register(string firstName, string lastName, string email, string password, string confirmPassword)
        {
            Driver.Enter(FirstName, firstName);
            Driver.Enter(LastName, lastName);
            Driver.Enter(EmailAddress, email);
            Driver.Enter(Password, password);
            Driver.Enter(ConfirmPassword, confirmPassword);
            Driver.Click(TermsAndConditions);
            Driver.Click(JoinButton);
        }

        public string GetFieldErrorMessage(string fieldName)
        {
            // maybe use reflection here and look for all private variables that match this name
            // this checks for the message for a given field which has 'error' class (highlights in red)
            return Driver.FindElement(By.XPath($"//input[@name='{fieldName}'][parent::div[contains(@class,'error')]]/following-sibling::div"))?.Text ?? null;
        }

        public override bool IsOpen()
        {
            return Driver.FindElement(FirstName).Displayed;
        }
    }
}
