using System;
using System.Collections.Generic;
using System.Linq;
using MarsFramework.WebDriver;
using MarsFramework.Factory;
using OpenQA.Selenium;
using MarsWebService.Pages.Common.Form;
using MarsWebService.Pages.Profile;

namespace MarsWebService.Pages.Common
{
    public class HeaderBar
    {
        private By SignOut => By.XPath("//button[.='Sign Out']");
        private By AccountMenuHeader => By.XPath("//button[.='Sign Out']/parent::a/preceding-sibling::span");

        private readonly Driver _driver;

        private static readonly IDictionary<string, Type> NavigtatoinMap;

        static HeaderBar()
        {
            NavigtatoinMap = new Dictionary<string, Type>
            {
                ["Go to Profile"] = typeof(ProfilePage),
                ["Change Password"] = typeof(ChangePasswordPage),
                //["Credits"] = 3,
                //["Transaction"] = 3,
                //["Account Settings"] = 3,

            };
        }

        public HeaderBar(Driver driver)
        {
            _driver = driver;
        }

        public void DoSignOut()
        {
            _driver?.Click(SignOut);
        }

        public IPage GoToAccountMenuItem(string menuItemText)
        {
            // ideally we want to use action here
            // but would have to update my wrapper driver class
            _driver.Click(AccountMenuHeader);
            _driver.WaitForAjax();
            var menuItems = _driver.FindElements(By.CssSelector("div.menu.transition > a"));
            menuItems.Where(m => m.Text == menuItemText).FirstOrDefault()?.Click();

            if (NavigtatoinMap.TryGetValue(menuItemText, out Type pageType))
            {
                return PageFactory.CreatePage(pageType, _driver) as IPage;
            }
            // handle null in tests
            return null;
        }


    }
}
