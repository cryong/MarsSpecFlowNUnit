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
    public class ShareSkillAddTest : LoginSetUp
    {
        [TestCaseSource(nameof(TestData))]
        public void When_ValidShareSkillData_Expect_AddSuccessful(ShareSkill addedShareSkill)
        {
            try
            {
                // arrange
                DataSetUpHelper helper = new DataSetUpHelper(ValidCredentials.Username, ValidCredentials.Password);
                // act
                // find an existing skill
                SkillSharePage shareSkillPage = new SkillSharePage(Driver);
                shareSkillPage.Open();

                ListingManagementPage listingManagementPage = shareSkillPage.EnterShareSkill(addedShareSkill);
                Driver.WaitForAjax();
                addedShareSkill.Id = helper.GetOrAdd(addedShareSkill);
                _setUpContext.Add(addedShareSkill);
                Driver.WaitForAjax();

                // assert
                Assert.Multiple(() =>
                {
                    Assert.AreEqual(listingManagementPage.Url, Driver.GetCurrentUrl());
                    Assert.That(listingManagementPage.SearchShareSkill(addedShareSkill), Is.Not.Null);
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
            yield return new TestCaseData(ReadFromExcel("Cypress"));
        }
    }
}
