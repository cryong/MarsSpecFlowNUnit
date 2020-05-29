using MarsFramework.WebDriver;
using NUnit.Framework;
using MarsWebService.Pages.Profile;
using TechTalk.SpecFlow;

namespace SpecFlowTest.StepsDefinitions
{
    [Binding]
    public sealed class ProfileAvailabilityInformationSteps
    {
        private readonly Driver _driver;
        private ProfilePage ProfilePage => new ProfilePage(_driver);

        public ProfileAvailabilityInformationSteps(Driver driver)
        {
            _driver = driver;
        }

        [When(@"I update available hours to ""(.*)""")]
        public void WhenIUpdateAvailableHoursTo(string optionText)
        {
            ProfilePage.GeneralInformationSection.GetAvailabilityHours().SelectByValue(ConvertAvailableHoursValue(optionText));
        }

        private string ConvertAvailableHoursValue(string text)
        {
            // don't really want to use steps argument tranformation just to handle this case
            return text == "Part Time" ? "0" : "1";
        }

        [Then(@"available hours is updated with a message ""(.*)""")]
        public void ThenAvailableHoursIsUpdatedWithAMessage(string expectedMessage)
        {
            Assert.That(expectedMessage, Is.EqualTo(ProfilePage.GetSuccessPopUpMessage()));
        }
    }
}
