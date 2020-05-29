using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports.Utils;
using MarsCommonFramework.DataSetup;
using MarsFramework.Factory;
using MarsFramework.Utilities;
using MarsWebService.Model;
using MarsWebService.Pages.Profile;
using MarsWebService.Pages.Search;
using NUnit.Framework;
using NUnitTest.SetUp;

namespace NUnitTest.Tests
{
    public class ShareSkillSearchCategoryTest : LoginSetUp
    {
        [TestCaseSource(nameof(TestData))]
        public void When_FilterCategory_Expect_FilterSuccessful(ShareSkill expectedSkilShare, string category, string subcategory)
        {
            try
            {
                // arrange
                DataSetUpHelper helper = new DataSetUpHelper(ValidCredentials.Username, ValidCredentials.Password);
                // act
                // find an existing skill
                expectedSkilShare.Id = helper.GetOrAdd(expectedSkilShare);
                _setUpContext.Add(expectedSkilShare);
                Driver.WaitForAjax();

                SearchResultPage searchResultPage = new ProfilePage(Driver).SearchBar.SearchSkill("Cucumber");
                searchResultPage.RefineResultsPane.FilterByCategory(category);
                Driver.WaitForAjax();
                searchResultPage.RefineResultsPane.FilterBySubCategory(subcategory);
                Driver.WaitForAjax();
                // assert
                Assert.Multiple(() =>
                {
                    Assert.That(searchResultPage.ResultSection.SearchResults.SearchResultList.IsNullOrEmpty(), Is.False);
                    Assert.True(searchResultPage.ResultSection.SearchResults.SearchResultList.First().GetSkillTitle() == expectedSkilShare.Title);
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
            yield return new TestCaseData(ReadFromExcel("Cucumber"), "Programming & Tech", "QA");
        }
    }
}
