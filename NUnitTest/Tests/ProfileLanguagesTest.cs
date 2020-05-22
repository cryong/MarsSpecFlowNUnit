using System;
using MarsCommonFramework.DataSetup;
using MarsFramework.Factory;
using MarsFramework.Utilities;
using MarsWebService.Model;
using MarsWebService.Pages.Profile;
using NUnit.Framework;
using NUnitTest.SetUp;

namespace NUnitTest.Tests
{
    [TestFixture]
    public class ProfileLanguagesTest : LoginSetUp
    {

        [Test]
        public void When_FourLanguagesAdded_Expect_AddDisabled()
        {
            try
            {
                // arrange
                DataSetUpHelper helper = new DataSetUpHelper(ValidCredentials.Username, ValidCredentials.Password);
                foreach (var i in new string[] { "1", "2", "3" })
                {
                    var language = ReadFromExcel(i);
                    language.Id = helper.GetOrAdd(language);
                    _setUpContext.Add(language);
                }

                // act
                ProfilePage profilePage = new ProfilePage(Driver); // reload
                profilePage.Open();
                var newLanguage = ReadFromExcel("4");
                profilePage.MainSection.EnterLanguageDetails(newLanguage);
                Driver.WaitForAjax();
                newLanguage.Id = helper.GetOrAdd(newLanguage);
                _setUpContext.Add(newLanguage);

                // assert
                Driver.WaitForAjax();
                Assert.That(profilePage.MainSection.IsAddRowButtonEnbaled(), Is.False);
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

        private Language ReadFromExcel(string key)
        {
            ExcelDataReaderUtil.LoadWorsheet(PathUtil.GetCurrentPath($"{TestContext.Parameters["TestDataPath"]}Mars.xlsx"), "ProfileLanguage");
            ExcelData data = ExcelDataReaderUtil.FetchRowUsingKey(key);
            return ObjectFactory.CreateInstance<Language>(data);
        }
    }
}
