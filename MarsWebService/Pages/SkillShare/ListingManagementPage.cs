using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsFramework.WebDriver;
using OpenQA.Selenium;
using MarsWebService.Model;

namespace MarsWebService.Pages.SkillShare
{
    public class ListingManagementPage : NavigatableBasePage
    {
        public ListingManagementPage(Driver driver) : base(driver)
        {
        }

        private By ManageListingsLink => By.LinkText("Manage Listings");

        public override string Url => "http://localhost:5000/Home/ListingManagement";

        public ListingManagementPage GoToManageListingsPage()
        {
            Driver.Click(ManageListingsLink);
            return new ListingManagementPage(Driver);
        }

        public bool PageNext()
        {
            // this is unavoidable
            // have to fetch the element again 
            // to update/refresh the state of the element
            IWebElement nextButton = Driver.FindElement(By.XPath("//div[contains(@class,'pagination')]/button[.='>']"));
            if (nextButton.Enabled)
            {
                nextButton.Click();
                return true;
            }
            return false;
        }

        public IWebElement SearchShareSkill(ShareSkill shareSkill)
        {
            IWebElement listingTable = Driver.FindElement(By.XPath("//div/h2[.='Manage Listings']/following-sibling::node()/div/table"));
            foreach (var rowCandidate in listingTable.FindElements(By.XPath("tbody/tr")))
            {
                // Column headings are as follows: 
                // Category,Title,Description,Service Type,Skill,Trade,Active
                // get all cells except first and last because we are not verifying image and buttons at the moment
                var cells = rowCandidate.FindElements(By.XPath("td[position() > 1 and position() < last()]"));
                if (cells == null || cells.Count == 0 || cells.Count < 6)
                {
                    // error out, throw some exception later
                    // something's wrong with the ui
                    return null;
                }

                var category = cells[0].Text;
                var title = cells[1].Text;
                var description = cells[2].Text;
                // assuming that title and description are sufficient for now
                //var serviceType = cells[3].Text;
                //var skillTrade = cells[4].Text;
                //var active = cells[5];

                if (category == shareSkill.Category &&
                    title == shareSkill.Title &&
                    description == shareSkill.Description)
                {
                    return rowCandidate;
                }
            }
            // click next and loop through again
            if (PageNext())
            {
                return SearchShareSkill(shareSkill);
            }
            return null;
        }

        public void DeleteShareSkill(ShareSkill shareSkill)
        {
            IWebElement matchingshareSkill = SearchShareSkill(shareSkill); // this should be the row
            matchingshareSkill.FindElement(By.XPath("td[last()]/div/button/i[contains(@class,'remove')]")).Click(); // click delete button
            Driver.Click(By.XPath("//div[@class='actions']/button[.='Yes']")); // click OK
        }

        public SkillSharePage UpdateShareSkill(ShareSkill shareSkill)
        {
            IWebElement matchingshareSkill = SearchShareSkill(shareSkill);
            matchingshareSkill.FindElement(By.XPath("td[last()]/div/button/i[contains(@class,'write')]")).Click();
            return new SkillSharePage(Driver);
        }
    }
}
