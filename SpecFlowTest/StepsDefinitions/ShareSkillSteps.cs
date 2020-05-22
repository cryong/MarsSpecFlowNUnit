using MarsFramework.Config;
using MarsFramework.WebDriver;
using MarsFramework.Factory;
using MarsFramework.Utilities;
using NUnit.Framework;
using MarsWebService.Model;
using MarsWebService.Pages.SkillShare;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Bindings;
using MarsCommonFramework.DataSetup;
using SpecFlowTest.Utilities;

namespace SpecFlowTest.StepsDefinitions
{
    [Binding]
    public sealed class ShareSkillSteps
    {
        private readonly Driver _driver;
        private readonly ScenarioContext _context;
        private ListingManagementPage ManageListingPage => new ListingManagementPage(_driver);
        private SkillSharePage SkillSharePage => new SkillSharePage(_driver);
        private readonly DataSetUpHelper _helper;

        public ShareSkillSteps(ScenarioContext context, Driver driver, DataSetUpHelper helper)
        {
            _context = context;
            _driver = driver;
            _helper = helper;
            ExcelDataReaderUtil.LoadWorsheet(PathUtil.GetCurrentPath($"{TestConfig.TestDataPath}Mars.xlsx"), "ShareSkill");
        }

        [Given(@"I open Share Skill page")]
        public void GivenIOpenShareSkillPage()
        {
            SkillSharePage.Open();
        }

        [Given(@"I already offer ""(.*)"" for trade")]
        [When(@"I list my ""(.*)"" skill for trade")]
        public void WhenIListMySkillForTrade(string skillTitle)
        {
            StepDefinitionType stepDefinitionType = _context.StepContext.StepInfo.StepDefinitionType;
            ExcelData data = ExcelDataReaderUtil.FetchRowUsingKey(skillTitle);
            var shareSkill = ObjectFactory.CreateInstance<ShareSkill>(data);
            _context.Set(shareSkill, skillTitle);
            // assumption that title is considered unique (even though it seems to have an internal ID that is not visible on page)
            if (stepDefinitionType == StepDefinitionType.When)
            {
                SkillSharePage.EnterShareSkill(shareSkill);
            }
            // retrieve ID
            shareSkill.Id = _helper.GetOrAdd(shareSkill);
            TestHelper.GetListOfObjectsToBeRemoved(_context).Add(shareSkill);
        }

        [Then(@"""(.*)"" is found in my managed listings")]
        [Then(@"""(.*)"" is updated successfully")]
        public void ThenIsFoundInMyManagedListings(string skillTitle)
        {
            // verify the url
            Assert.AreEqual(ManageListingPage.Url, _driver.GetCurrentUrl());
            Assert.That(ManageListingPage.SearchShareSkill(_context.Get<ShareSkill>(skillTitle)), Is.Not.Null);
        }

        [Given(@"I am on Listing Management page")]
        public void GivenIAmOnListingManagementPage()
        {
            ManageListingPage.Open();
        }

        [When(@"I update my ""(.*)"" listing as follows:")]
        public void WhenIUpdateMyListingAsFollows(string skillTitle, Table table)
        {
            // find an existing skill
            var skillToUpdate = _context.Get<ShareSkill>(skillTitle);
            SkillSharePage shareSkillPage = ManageListingPage.UpdateShareSkill(skillToUpdate);
            // update object created during set up with table values
            table.FillInstance(skillToUpdate);
            shareSkillPage.EnterShareSkill(skillToUpdate);

            // dirty hack
            var originalId = skillToUpdate.Id;

            skillToUpdate.Id = null;
            // It turns out that performing update actually adds another row
            // this approach is really dirty and needs a better way to handle this issue (can't think of any currently and I don't wnat to perform deep copy)
            // since results of WS call results in a listings from the latest -> oldest (LIFO)
            // can assume that the first match returned is the newly updated one (also need to make sure that other tests use different 'titles' to guarantee uniqueness)
            string id = _helper.GetOrAdd(skillToUpdate);
            TestHelper.GetListOfObjectsToBeRemoved(_context).Add(new ShareSkill() { Id = id }); // shouldn't need validation here
            skillToUpdate.Id = originalId;
        }

        [When(@"I delete my ""(.*)"" listing")]
        public void WhenIDeleteMyListing(string skillTitle)
        {
            var shareSkill = _context.Get<ShareSkill>(skillTitle);
            ManageListingPage.DeleteShareSkill(shareSkill);
            TestHelper.GetListOfObjectsToBeRemoved(_context).Remove(shareSkill);// should be null-safe at this point...
        }

        [Then(@"""(.*)"" is no longer found in my managed listings")]
        public void ThenIsNoLongerFoundInMyManagedListings(string skillTitle)
        {
            Assert.AreEqual(ManageListingPage.Url, _driver.GetCurrentUrl());
            Assert.That($"{skillTitle} has been deleted", Is.EqualTo(ManageListingPage.GetSuccessPopUpMessage()));
            // unfortunately there is no real easy way to make this record unique/distinct because since skill title is not a primary key
            // just applying assertions with popup message insteads
        }

    }
}
