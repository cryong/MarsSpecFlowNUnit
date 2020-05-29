using System;
using MarsWebService.Pages.Profile;
using NUnit.Framework;
using NUnitTest.SetUp;

namespace NUnitTest.Tests
{
    [TestFixture]
    public class ProfileDescriptionTest : LoginSetUp
    {
        [TestCase(" This is invalid description", "First character can only be digit or letters")]
        public void When_LeadingWhiteSpace_Expect_UpdateFailure(string description, string expectedMessage)
        {
            try
            {
                ProfilePage profilePage = new ProfilePage(Driver);
                profilePage.DescriptionSection.UpdateDescription(description);

                try
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(profilePage.DescriptionSection.IsOpen(), Is.True);
                        Assert.That(expectedMessage, Is.EqualTo(new ProfilePage(Driver).GetErrorPopUpMessage()));
                    });
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
