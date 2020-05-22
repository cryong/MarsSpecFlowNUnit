using NUnit.Framework;
using MarsWebService.Pages.Common.Form;
using MarsWebService.Pages.Profile;
using TechTalk.SpecFlow;
using MarsFramework.WebDriver;

namespace SpecFlowTest.StepsDefinitions
{
    [Binding]
    public sealed class ChangePasswordSteps
    {
        private ChangePasswordPage changePasswordPage;
        private readonly Driver _driver;

        public ChangePasswordSteps(Driver driver)
        {
            _driver = driver;
        }

        [Given(@"I navigate to ""(.*)"" page")]
        public void GivenINavigateToPage(string pageTextFromMenu)
        {
            changePasswordPage = new ProfilePage(_driver).HeaderBar.GoToAccountMenuItem(pageTextFromMenu)
                                 as ChangePasswordPage;
        }

        [When(@"I reuse my current password")]
        public void WhenIReuseMyCurrentPassword()
        {
            var currentPassword = Base.TestBase.Credentials.Password;
            changePasswordPage.ChangePassword(currentPassword, currentPassword, currentPassword);
        }

        [Then(@"password is not saved")]
        public void ThenPasswordIsNotSaved()
        {
            // Form still open
            Assert.That(changePasswordPage.IsOpen(), Is.True);
        }

    }
}
