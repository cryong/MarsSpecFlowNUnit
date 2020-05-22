using System;
using MarsFramework.WebDriver;
using NUnit.Framework;
using MarsWebService.Pages.Profile;
using TechTalk.SpecFlow;
using MarsFramework.Service;

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
            try
            {
                Assert.That(expectedMessage, Is.EqualTo(ProfilePage.GetSuccessPopUpMessage()));
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                {
                    throw;
                }
                Assert.Fail($"Error has occurred\nMessage : {e.Message}\nStackTrace : {e.StackTrace}");
            }
        }
    }
}
