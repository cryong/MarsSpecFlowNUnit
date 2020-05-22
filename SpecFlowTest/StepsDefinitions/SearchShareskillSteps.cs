using System.Linq;
using MarsFramework.Config;
using MarsFramework.WebDriver;
using MarsFramework.Factory;
using MarsFramework.Utilities;
using NUnit.Framework;
using MarsWebService.Model;
using MarsWebService.Pages.Search;
using MarsWebService.Pages.SkillShare;
using TechTalk.SpecFlow;
using MarsCommonFramework.DataSetup;
using MarsFramework.Service;
using AventStack.ExtentReports.Utils;
using SpecFlowTest.Utilities;

namespace SpecFlowTest.StepsDefinitions
{
    [Binding]
    public sealed class SearchShareskillSteps
    {
        private readonly ScenarioContext _context;
        private readonly Driver _driver;
        private SearchResultPage searchResultPage;
        private readonly DataSetUpHelper _helper;

        public SearchShareskillSteps(ScenarioContext context, Driver driver, DataSetUpHelper helper)
        {
            _context = context;
            _driver = driver;
            _helper = helper;
            ExcelDataReaderUtil.LoadWorsheet(PathUtil.GetCurrentPath($"{TestConfig.TestDataPath}Mars.xlsx"), "ShareSkill");
        }

        [Given(@"the skill search results for ""(.*)"" are shown")]
        public void GivenTheSkillSearchResultsForAreShown(string searchKey)
        {
            ExcelData data = ExcelDataReaderUtil.FetchRowUsingKey(searchKey);
            var shareSkill = ObjectFactory.CreateInstance<ShareSkill>(data);
            _context.Set(shareSkill);
            // assumption that title is considered unique (even though it seems to have an internal ID that is not visible on page)
            // retrieve ID
            shareSkill.Id = _helper.GetOrAdd(shareSkill);

            var objectsToBeDeleted = TestHelper.GetListOfObjectsToBeRemoved(_context);
            objectsToBeDeleted.Add(shareSkill);

            ListingManagementPage listPage = new ListingManagementPage(_driver);
            listPage.Open();
            searchResultPage = listPage.SearchBar.SearchSkill(searchKey);
            _driver.WaitForAjax();
        }

        [When(@"I filter the results with ""(.*)"" location filter")]
        public void WhenIFilterTheResultsWithLocationFilter(string locationType)
        {
            searchResultPage.RefineResultsPane.FilterResultsByLocationType(locationType);
            _driver.WaitForAjax();
        }

        [When(@"I filter the results by category ""(.*)""")]
        public void WhenIFilterTheResultsByCategory(string category)
        {
            _driver.WaitForAjax();
            searchResultPage.RefineResultsPane.FilterByCategory(category);
        }

        [When(@"I filter results using a subcategory")]
        public void WhenIFilterResultsUsingASubcategory()
        {
            searchResultPage.RefineResultsPane.FilterBySubCategory("Other");
            _driver.WaitForAjax();
        }

        [Then(@"only the matching skills are displayed")]
        public void ThenOnlyTheMatchingSkillsAreDisplayed()
        {
            // check that there is at least one record
            // not sure how to handle this, information not visible on page
            // no way to verify at this point, just check that some items are displayed
            Assert.That(searchResultPage.ResultSection.SearchResults.SearchResultList.IsNullOrEmpty(), Is.False);
            Assert.True(searchResultPage.ResultSection.SearchResults.SearchResultList.First().GetSkillTitle() == _context.Get<ShareSkill>().Title);
        }
    }
}
