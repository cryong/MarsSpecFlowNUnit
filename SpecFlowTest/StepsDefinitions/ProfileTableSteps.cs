using System;
using System.Collections.Generic;
using MarsFramework.WebDriver;
using NUnit.Framework;
using OpenQA.Selenium;
using MarsWebService.Model;
using MarsWebService.Pages.Profile;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using static MarsWebService.Pages.Sections.Profile.Sections.MainSection;
using MarsCommonFramework.DataSetup;
using SpecFlowTest.Utilities;

namespace SpecFlowTest.StepsDefinitions
{
    [Binding]
    public sealed class ProfileTableSteps // all the common steps related to data table in Profile page
    {
        private readonly ScenarioContext _context;
        private readonly Driver _driver;
        private ProfilePage ProfilePage => new ProfilePage(_driver);
        private readonly DataSetUpHelper _helper;

        public ProfileTableSteps(ScenarioContext injectedContext, Driver driver, DataSetUpHelper helper)
        {
            _context = injectedContext;
            _driver = driver;
            _helper = helper;
        }

        [Given(@"I am on ""(.*)"" tab")]
        public void GivenIAmOnTab(ProfileInfoType type)
        {
            ProfilePage.Open();
            ProfilePage.MainSection.ClickTab(type);
        }

        [Given(@"I already have (?:\d+) '(.*)(?:\(s\))' as follows:")]
        public void GivenIAlreadyHaveAsFollows(ProfileInfoType type, Table table)
        {
            var details = CreateProfileDetails(type, table);
            //Type elementType = details.FirstOrDefault()?.GetType();
            //var genericMethod = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(elementType);
            //System.Collections.IEnumerable converted = genericMethod.Invoke(null, new[] { details }) as System.Collections.IEnumerable;
            //var credentials = TestBase.Credentials;
            //var helper = new DataSetUpHelper(credentials.Username, credentials.Password); // share helper in the same thread? not sure
            //initialise

            var objectsToBeDeleted = TestHelper.GetListOfObjectsToBeRemoved(_context);
            foreach (var detail in details)
            {
                detail.Id = _helper.GetOrAdd((object)detail);
                objectsToBeDeleted.Add(detail);
            }
        }

        [When(@"I save (?:another )?'(.*)' as follows:")]
        public void WhenISaveAnotherAsFollows(ProfileInfoType type, Table table)
        {
            var detail = CreateProfileDetail(type, table);
            // retrieve ID
            ProfilePage.MainSection.SaveProfileDetail(detail);
            _context.Set(detail);
            TestHelper.GetListOfObjectsToBeRemoved(_context).Add(detail);
            _driver.WaitForAjax();
            detail.Id = _helper.GetOrAdd((object)detail);
        }

        [Then(@"my profile page displays the newly added (?:.+?)")]
        public void ThenMyProfilePageDisplaysTheNewlyAdded()
        {
            bool isFound;
            try
            {
                isFound = ProfilePage.MainSection.SearchForRow(_context.Get<SearchableItem>());
            }
            catch (Exception)
            {
                throw;
            }
            Assert.IsTrue(isFound);
        }

        [Then(@"the success popup message ""(.*)"" is displayed")]
        public void ThenTheSuccessMessageIsDisplayed(string expectedMessage)
        {
            CheckForSuccessPopUp(expectedMessage);
        }

        [Then(@"I can no longer add another '(?:.*)'")]
        public void ThenICanNoLongerAddAnother()
        {
            Assert.Throws<WebDriverTimeoutException>(() => ProfilePage.MainSection.SaveProfileDetail(_context.Get<SearchableItem>()));
        }

        private void CheckForSuccessPopUp(string expectedMessage)
        {
            string actualMessage;
            try
            {
                actualMessage = ProfilePage.GetSuccessPopUpMessage();
            }
            catch (Exception)
            {
                throw;
            }

            Assert.That(expectedMessage, Is.EqualTo(actualMessage));
        }

        private SearchableItem CreateProfileDetail(ProfileInfoType type, Table table)
        {
            switch (type)
            {
                case ProfileInfoType.Language:
                    return table.CreateInstance<Language>();
                case ProfileInfoType.Skill:
                    return table.CreateInstance<Skill>();
                case ProfileInfoType.Education:
                    return table.CreateInstance<Education>();
                case ProfileInfoType.Certification:
                    return table.CreateInstance<Certification>();
                default:
                    throw new ArgumentException($"Unknown {nameof(type)} specified : {type:G}");
            }
        }

        private IEnumerable<SearchableItem> CreateProfileDetails(ProfileInfoType type, Table table)
        {
            switch (type)
            {
                case ProfileInfoType.Language:
                    return table.CreateSet<Language>();
                case ProfileInfoType.Skill:
                    return table.CreateSet<Skill>();
                case ProfileInfoType.Education:
                    return table.CreateSet<Education>();
                case ProfileInfoType.Certification:
                    return table.CreateSet<Certification>();
                default:
                    throw new ArgumentException($"Unknown {nameof(type)} specified : {type:G}");
            }
        }

    }
}
