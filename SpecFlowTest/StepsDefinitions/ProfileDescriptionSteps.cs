using MarsFramework.WebDriver;
using NUnit.Framework;
using MarsWebService.Pages.Profile;
using TechTalk.SpecFlow;

namespace SpecFlowTest.StepsDefinitions
{
    [Binding]
    public class ProfileDescriptionSteps
    {
        private readonly Driver _driver;
        private ProfilePage ProfilePage => new ProfilePage(_driver);

        public ProfileDescriptionSteps(Driver driver)
        {
            _driver = driver;
        }

        [When(@"I modify description to ""(.*)""")]
        public void WhenIModifyDescriptionTo(string description)
        {
            ProfilePage.DescriptionSection.UpdateDescription(description);
        }

        [Then(@"description is not saved")]
        public void ThenDescriptionIsNotSaved()
        {
            Assert.IsTrue(ProfilePage.DescriptionSection.IsOpen());
        }

        [Then(@"the error popup message ""(.*)"" is displayed")]
        public void ThenTheErrorMessageIsDisplayed(string expectedMessage)
        {
            Assert.That(expectedMessage, Is.EqualTo(ProfilePage.GetErrorPopUpMessage()));
        }
    }
}
