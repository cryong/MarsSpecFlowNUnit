using System;
using System.Collections.Generic;
using MarsFramework.Factory;
using MarsFramework.Utilities;
using MarsWebService.Model;
using MarsWebService.Pages.Home;
using NUnit.Framework;
using NUnitTest.SetUp;

namespace NUnitTest.Tests
{
    [TestFixture]
    public class RegistrationTest : BaseSetUp
    {

        [Test]
        [TestCaseSource(typeof(RegistrationTest), nameof(MyData))]
        public void When_InvalidInput_Expect_RegistrationFailure(Registration registrationData)
        {
            try
            {
                RegistrationPage RegistrationPage = new HomePage(Driver).OpenRegistrationForm();
                RegistrationPage.Register(registrationData.FirstName,
                                          registrationData.LastName,
                                          registrationData.Email,
                                          registrationData.Password,
                                          registrationData.ConfirmPassword);

                Assert.That(RegistrationPage.IsOpen(), Is.True);
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

        private static Registration ReadFromExcel(string key)
        {
            ExcelDataReaderUtil.LoadWorsheet(PathUtil.GetCurrentPath($"{TestContext.Parameters["TestDataPath"]}Mars.xlsx"), "Registration");
            ExcelData data = ExcelDataReaderUtil.FetchRowUsingKey(key);
            return ObjectFactory.CreateInstance<Registration>(data);
        }
        public static IEnumerable<TestCaseData> MyData()
        {
            yield return new TestCaseData(ReadFromExcel("MissingFirstName"));
            yield return new TestCaseData(ReadFromExcel("MissingLastName"));
        }
    }
}
