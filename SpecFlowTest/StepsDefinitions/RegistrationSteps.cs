using MarsFramework.WebDriver;
using MarsFramework.Utilities;
using NUnit.Framework;
using MarsWebService.Pages.Home;
using TechTalk.SpecFlow;

namespace SpecFlowTest.StepsDefinitions
{
    [Binding]
    public sealed class RegistrationSteps
    {
        private readonly Driver _driver;
        private RegistrationPage RegistrationPage { get; set; }
        public RegistrationSteps(Driver driver)
        {
            _driver = driver;
        }

        [When(@"I register with a registered email address")]
        public void WhenIRegisterWithARegisteredEmailAddress()
        {
            ExcelData data = ExcelDataReaderUtil.FetchRowUsingKey("DuplicateEmail"); // this is lot of code duplication
            RegistrationPage = new HomePage(_driver).OpenRegistrationForm();
            RegistrationPage.Register(data.FetchColumnValue("FirstName"),
                                      data.FetchColumnValue("LastName"),
                                      data.FetchColumnValue("Email"),
                                      data.FetchColumnValue("Password"),
                                      data.FetchColumnValue("ConfirmPassword"));
        }

        [Then(@"registration is not successful")]
        public void ThenRegistrationIsNotSuccessful()
        {
            Assert.That(RegistrationPage.IsOpen(), Is.True);
        }

        [Then(@"the failure message ""(.*)"" is displayed")]
        public void ThenTheFailureMessageIsDisplayed(string expectedMessage)
        {
            Assert.That(expectedMessage, Is.EqualTo(RegistrationPage.GetFieldErrorMessage("email")));
        }

    }
}
