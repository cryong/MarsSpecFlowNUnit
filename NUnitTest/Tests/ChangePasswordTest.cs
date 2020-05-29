using System;
using MarsFramework.Model;
using MarsFramework.Service;
using MarsWebService.Pages.Common.Form;
using MarsWebService.Pages.Profile;
using NUnit.Framework;
using NUnitTest.SetUp;

namespace NUnitTest.Tests
{
    [TestFixture]
    public class ChangePasswordTest : LoginSetUp
    {
        [Test]
        public void When_SamePassword_Expect_PasswordChangeFailure()
        {
            ChangePasswordPage changePasswordPage = new ProfilePage(Driver).HeaderBar.GoToAccountMenuItem("Change Password")
                                                    as ChangePasswordPage;
            changePasswordPage.ChangePassword(ValidCredentials.Password, ValidCredentials.Password, ValidCredentials.Password);
            try
            {
                Assert.Multiple(() =>
                {
                    Assert.That(changePasswordPage.IsOpen(), Is.True);
                    Assert.That("Current Password and New Password should not be same",
                                Is.EqualTo(new ProfilePage(Driver).GetErrorPopUpMessage()));
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
    }
}
