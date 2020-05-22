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
    public sealed class ShareSkillUpdateSteps
    {
        private readonly ScenarioContext context;
        private readonly IDriver _driver;
        private ManageListingPage manageListingPage;
        private readonly ShareSkill _shareSkill;
        public ShareSkillUpdateSteps(ScenarioContext injectedContext, IDriver driver, ShareSkill shareSkill)
        {
            context = injectedContext;
            _driver = driver;
            _shareSkill = shareSkill;
        }

        [Given(@"I open Manage Listing page")]
        public void GivenIOpenManageListingPage()
        {
            ExcelData data = ExcelDataReaderUtil.FetchRowUsingKey("Add");
            var shareSkill = ObjectFactory.CreateInstance<ShareSkill>(data);
            new SkillSharePage(_driver).Open();
            manageListingPage = new SkillSharePage(_driver).EnterShareSkill(shareSkill);
            context.Set(shareSkill);

        }

        [When(@"I update a skill to share")]
        public void WhenIUpdateASkillToShare()
        {
            SkillSharePage shareSkillPage = manageListingPage.UpdateShareSkill(context.Get<ShareSkill>());
            ExcelData data = ExcelDataReaderUtil.FetchRowUsingKey("Update");
            var shareSkill = ObjectFactory.CreateInstance<ShareSkill>(data);
            shareSkillPage.EnterShareSkill(shareSkill);
            context.Set(shareSkill);

        }

    }
}
