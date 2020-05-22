using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsCommonFramework.DataSetup;
using MarsFramework.Factory;
using MarsFramework.Utilities;
using MarsWebService.Model;
using MarsWebService.Pages.SkillShare;
using NUnit.Framework;
using NUnitTest.SetUp;
using OpenQA.Selenium;

namespace NUnitTest.Tests
{
    public class ShareSkillDeleteTest : LoginSetUp
    {
        [TestCaseSource(typeof(ShareSkillDeleteTest), nameof(MyData))]
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
                    //Assert.Throws<WebDriverTimeoutException>(() => listingManagementPage.SearchShareSkill(shareSkillToDelete));
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
            yield return new TestCaseData(ReadFromExcel("JMeter"));
        }

    }
}
