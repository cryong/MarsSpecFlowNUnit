using System;
using System.Collections.Generic;
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
    public class ProfileEducationTest : LoginSetUp
    {
        [Test]
        [TestCaseSource(typeof(ProfileEducationTest), nameof(MyData))]
        public void When_ValidEducationDetails_Expect_AdddSuccessful(Education education)
        {
            try
            {
                DataSetUpHelper helper = new DataSetUpHelper(ValidCredentials.Username, ValidCredentials.Password);

                // act
                ProfilePage profilePage = new ProfilePage(Driver); // reload
                profilePage.Open();
                profilePage.MainSection.EnterEducationDetails(education);
                Driver.WaitForAjax();
                education.Id = helper.GetOrAdd(education);
                _setUpContext.Add(education);

                // assert
                Driver.WaitForAjax();
                Assert.IsTrue(profilePage.MainSection.SearchForRow(education));
                Assert.That(
                    "Education has been added",
                    Is.EqualTo(profilePage.GetSuccessPopUpMessage()));
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

        private static Education ReadFromExcel(string key)
        {
            ExcelDataReaderUtil.LoadWorsheet(PathUtil.GetCurrentPath($"{TestContext.Parameters["TestDataPath"]}Mars.xlsx"), "ProfileEducation");
            ExcelData data = ExcelDataReaderUtil.FetchRowUsingKey(key);
            return ObjectFactory.CreateInstance<Education>(data);
        }
        public static IEnumerable<TestCaseData> MyData()
        {
            yield return new TestCaseData(ReadFromExcel("1"));
        }
    }
}
