using System;
using MarsFramework.DriverAdapters;
using SpecFlowTest.Model;
using SpecFlowTest.Pages.SkillShare;
using TechTalk.SpecFlow;

namespace SpecFlowTest.StepsDefinitions
{
    [Binding]
    public class ShareSkillDeleteSteps
    {

        private readonly IDriver _driver;
        private readonly ScenarioContext _context;
        private readonly ShareSkill _shareSkill;

        public ShareSkillDeleteSteps(ScenarioContext context, IDriver driver, ShareSkill shareSkill)
        {
            _context = context;
            _driver = driver;
            _shareSkill = shareSkill; // this is not necessary
        }

        [When(@"I delete a skill to share")]
        public void WhenIDeleteASkillToShare()
        {
            var shareSkill = _context.Get<ShareSkill>();
            Console.WriteLine(_shareSkill.ToString() == shareSkill.ToString());
            new ManageListingPage(_driver).DeleteShareSkill(shareSkill);
        }
        
        [Then(@"the record should be deleted")]
        public void ThenTheRecordShouldBeDeleted()
        {
            Console.WriteLine("hohohoho");
        }
    }
}
