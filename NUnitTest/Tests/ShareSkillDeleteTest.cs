using System;
using System.Collections.Generic;
using MarsCommonFramework.DataSetup;
using MarsFramework.Factory;
using MarsFramework.Utilities;
using MarsWebService.Model;
using MarsWebService.Pages.SkillShare;
using NUnit.Framework;
using NUnitTest.SetUp;

namespace NUnitTest.Tests
{
    public class ShareSkillDeleteTest : LoginSetUp
    {
        [TestCaseSource(nameof(TestData))]
        public void When_ExistingSkillData_Expect_DeleteSuccessful(ShareSkill shareSkillToDelete)
        {
            try
            {
                // arrange
                DataSetUpHelper helper = new DataSetUpHelper(ValidCredentials.Username, ValidCredentials.Password);
                shareSkillToDelete.Id = helper.GetOrAdd(shareSkillToDelete);
                _setUpContext.Add(shareSkillToDelete);
                // act
                // find an existing skill
                ListingManagementPage listingManagementPage = new ListingManagementPage(Driver);
                listingManagementPage.Open();
                Driver.WaitForAjax();

                listingManagementPage.DeleteShareSkill(shareSkillToDelete);
                // assert
                Assert.Multiple(() =>
                {
                    Assert.AreEqual(listingManagementPage.Url, Driver.GetCurrentUrl());
                    Assert.That($"{shareSkillToDelete.Title} has been deleted", Is.EqualTo(listingManagementPage.GetSuccessPopUpMessage()));
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
        public static IEnumerable<TestCaseData> TestData()
        {
            yield return new TestCaseData(ReadFromExcel("JMeter"));
        }

    }
}
