using MarsFramework.Config;
using MarsFramework.WebDriver;
using NUnit.Framework;
using MarsWebService.Model;
using MarsWebService.Pages.Profile;
using SpecFlowTest.Base;
using TechTalk.SpecFlow;
using MarsWebService.Pages.Home;
using MarsFramework.Service;

namespace SpecFlowTest.StepsDefinitions
{
    [Binding]
    public sealed class SignInSteps
    {
        private readonly Driver _driver;
        private readonly Credentials _credentials = new Credentials();

        public SignInSteps(Driver driver)
        {
            _driver = driver;
        }

        [Given(@"I am on the application homepage")]
        public void GivenIAmOnTheApplicationHomepage()
        {
            _driver.GoToUrl(TestConfig.BaseURL);
        }

        [Given(@"I am a registered user")]
        public void GivenIAmARegisteredUser()
        {
            Credentials credentials = TestBase.Credentials;
            _credentials.Username = credentials.Username;
            _credentials.Password = credentials.Password;
        }

        [When(@"I login")]
        public void WhenILogin()
        {
            SignInPage signInPage = new HomePage(_driver).OpenLoginForm();
            signInPage.EnterCredentials(_credentials.Username, _credentials.Password);
            signInPage.ClickLogInButton();
        }

        [Then(@"my profile page is displayed")]
        public void ThenMyProfilePageIsDisplayed()
        {
            Assert.That(_driver.GetCurrentUrl, Is.EqualTo(new ProfilePage(DriverManager.GetDriver()).Url));
        }
    }
}
