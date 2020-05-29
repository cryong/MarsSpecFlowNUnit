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
    public class ProfileSkillTest : LoginSetUp
    {
        [TestCaseSource(nameof(TestData))]
        public void When_ValidCertficationDetails_Expect_AdddSuccessful(Skill skill)
        {
            try
            {
                DataSetUpHelper helper = new DataSetUpHelper(ValidCredentials.Username, ValidCredentials.Password);

                // act
                ProfilePage profilePage = new ProfilePage(Driver);
                profilePage.Open();
                profilePage.MainSection.EnterSkillDetails(skill);
                Driver.WaitForAjax();
                skill.Id = helper.GetOrAdd(skill);
                _setUpContext.Add(skill);

                // assert
                Driver.WaitForAjax();
                Assert.IsTrue(profilePage.MainSection.SearchForRow(skill));
                Assert.That(
                    $"{skill.Name} has been added to your skills",
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

        private static Skill ReadFromExcel(string key)
        {
            ExcelDataReaderUtil.LoadWorsheet(PathUtil.GetCurrentPath($"{TestContext.Parameters["TestDataPath"]}Mars.xlsx"), "ProfileSkill");
            ExcelData data = ExcelDataReaderUtil.FetchRowUsingKey(key);
            return ObjectFactory.CreateInstance<Skill>(data);
        }
        public static IEnumerable<TestCaseData> TestData()
        {
            yield return new TestCaseData(ReadFromExcel("1"));
            yield return new TestCaseData(ReadFromExcel("2"));
            yield return new TestCaseData(ReadFromExcel("3"));
            yield return new TestCaseData(ReadFromExcel("4"));
            yield return new TestCaseData(ReadFromExcel("5"));
        }
    }
}
