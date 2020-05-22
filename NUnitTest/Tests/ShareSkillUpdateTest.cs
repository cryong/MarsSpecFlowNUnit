using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsCommonFramework.DataSetup;
using MarsFramework.Factory;
using MarsFramework.Utilities;
using MarsWebService.Model;
using MarsWebService.Pages.Profile;
using MarsWebService.Pages.SkillShare;
using NUnit.Framework;
using NUnitTest.SetUp;

namespace NUnitTest.Tests
{
    [TestFixture]
    public class ShareSkillUpdateTest : LoginSetUp
    {

        [TestCaseSource(typeof(ShareSkillUpdateTest), nameof(MyData))]
        public void When_ValidShareSkillData_Expect_UpdateSuccessful(ShareSkill addedShareSkill, ShareSkill shareSkillToUpdate)
        {
            try
            {
                // arrange
                DataSetUpHelper helper = new DataSetUpHelper(ValidCredentials.Username, ValidCredentials.Password);
                addedShareSkill.Id = helper.GetOrAdd(addedShareSkill);
                _setUpContext.Add(addedShareSkill);

                // act
                // find an existing skill

                ListingManagementPage listingManagementPage = new ListingManagementPage(Driver);
                listingManagementPage.Open();
                SkillSharePage shareSkillPage = listingManagementPage.UpdateShareSkill(addedShareSkill);
                shareSkillPage.EnterShareSkill(shareSkillToUpdate);
                Driver.WaitForAjax();
                shareSkillToUpdate.Id = helper.GetOrAdd(shareSkillToUpdate);
                _setUpContext.Add(shareSkillToUpdate);
                Driver.WaitForAjax();

                // assert
                Assert.Multiple(() =>
                {
                    Assert.AreEqual(listingManagementPage.Url, Driver.GetCurrentUrl());
                    Assert.That(listingManagementPage.SearchShareSkill(shareSkillToUpdate), Is.Not.Null);
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

        private static ShareSkill ReadFromExcel(string key)
        {
            ExcelDataReaderUtil.LoadWorsheet(PathUtil.GetCurrentPath($"{TestContext.Parameters["TestDataPath"]}Mars.xlsx"), "ShareSkill");
            ExcelData data = ExcelDataReaderUtil.FetchRowUsingKey(key);
            return ObjectFactory.CreateInstance<ShareSkill>(data);
        }
        public static IEnumerable<TestCaseData> MyData()
        {
            yield return new TestCaseData(ReadFromExcel("Selenium"), ReadFromExcel("Cucumber"));
        }

    }
}
