using System;
using MarsWebService.Pages.Profile;
using NUnit.Framework;
using NUnitTest.SetUp;

namespace NUnitTest.Tests
{
    [TestFixture]
    public class AvailabilityInformationTest : LoginSetUp
    {
        [TestCase("0", "Availability updated")]
        [TestCase("1", "Availability updated")]
        public void When_InvalidInput_Expect_RegistrationFailure(string value, string expectedMessage)
        {
            try
            {
                ProfilePage profilePage = new ProfilePage(Driver);
                profilePage.GeneralInformationSection.GetAvailabilityHours().SelectByValue(value);
                Assert.That(expectedMessage, Is.EqualTo(profilePage.GetSuccessPopUpMessage()));
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
