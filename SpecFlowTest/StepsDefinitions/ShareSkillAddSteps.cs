using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarsFramework.DriverAdapters;
using MarsFramework.Factory;
using MarsFramework.Utilities;
using NUnit.Framework;
using SpecFlowTest.Model;
using SpecFlowTest.Pages.SkillShare;
using TechTalk.SpecFlow;

namespace SpecFlowTest.StepsDefinitions
{
    [Binding]
    public sealed class ShareSkillAddSteps
    {
        private readonly ScenarioContext context;
        private readonly IDriver _driver;
        private ManageListingPage manageListingPage;
        public ShareSkillAddSteps(ScenarioContext injectedContext, IDriver driver)
        {
            context = injectedContext;
            _driver = driver;
        }

        [Given(@"I open Share Skill page")]
        public void GivenIOpenShareSkillPage()
        {
            new SkillSharePage(_driver).Open();
        }

        [When(@"I add a skill to share")]
        public void WhenIAddASkillToShare()
        {
            ExcelData data = ExcelDataReaderUtil.FetchRowUsingKey("Add");
            var shareSkill = ObjectFactory.CreateInstance<ShareSkill>(data);
            manageListingPage = new SkillSharePage(_driver).EnterShareSkill(shareSkill);
            context.Set(shareSkill);
        }

        [Then(@"I should be navigated to Manage Listings page")]
        public void ThenIShouldBeNavigatedToManageListingsPage()
        {
            Assert.AreEqual(ManageListingPage.Url, _driver.GetCurrentUrl());
        }

        [Then(@"the record should be saved")]
        public void ThenTheRecordShouldBeSaved()
        {
            // need new instance of manage listing page because it also gets called from share skill update steps
            Assert.That(new ManageListingPage(_driver).SearchShareSkill(context.Get<ShareSkill>()), Is.Not.Null);
        }

    }
}
