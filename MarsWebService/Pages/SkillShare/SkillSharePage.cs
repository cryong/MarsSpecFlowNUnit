using System.Collections.Generic;
using System.Linq;
using MarsFramework.WebDriver;
using OpenQA.Selenium;
using MarsWebService.Model;

namespace MarsWebService.Pages.SkillShare
{
    public class SkillSharePage : NavigatableBasePage
    {
        public SkillSharePage(Driver driver) : base(driver)
        {
        }

        public override string Url => "http://localhost:5000/Home/ServiceListing";

        private By Title => By.Name("title");
        private By Description => By.Name("description");
        private By CategoryDropDown => By.Name("categoryId");
        private By SubCategoryDropDown => By.Name("subcategoryId");
        private By Tags => By.XPath("//div[h3/.='Tags']/following-sibling::div//input[@placeholder='Add new tag']");
        private By ServiceTypeHourlyBasisOption => By.XPath("//input[@name='serviceType' and following-sibling::node()[.='Hourly basis service']]");
        private By ServiceTypeOneOffOption => By.XPath("//input[@name='serviceType' and following-sibling::node()[.='Hourly basis service']]");
        private By LocationTypeOnSiteOption => By.XPath("//input[@name='locationType'][following-sibling::node()[.='On-site']]");
        private By LocationTypeOnlineOption => By.XPath("//input[@name='locationType'][following-sibling::node()[.='Online']]");
        private By StartDateDropDown => By.Name("startDate");
        private By EndDateDropDown => By.Name("endDate");
        private By SkillTradeSkillExchangeOption => By.XPath("//input[@name='skillTrades'][following-sibling::node()[.='Skill-exchange']]");
        private By SkillTradeCreditOption => By.XPath("//input[@name='skillTrades'][following-sibling::node()[.='Credit']]");
        private By SkillExchange => By.XPath("//div[@class='form-wrapper']//input[@placeholder='Add new tag']");
        private By CreditAmount => By.XPath("//input[@placeholder='Amount']");
        private By ActiveActiveOption => By.XPath("//input[@name='isActive'][following-sibling::node()[.='Active']]");
        private By ActiveHiddenOption => By.XPath("//input[@name='isActive'][following-sibling::node()[.='Hidden']]");
        private By WorkSample => By.XPath("//i[contains(@class,'plus circle')]");
        private By Save => By.XPath("//input[@value='Save']");

        private By AvailableDayAndTimes => By.XPath("//div[h3[.='Available days']]/following-sibling::div/div/div[div/div/input[@name='Available']]/descendant::input");

        public ListingManagementPage EnterShareSkill(ShareSkill shareSkill) // same logic for both add and update
        {
            // enter valid title
            Driver.Enter(Title, shareSkill.Title);
            // enter description
            Driver.Enter(Description, shareSkill.Description);
            //choose category
            Driver.SelectOption(SelectDropDownOption.TEXT, shareSkill.Category, CategoryDropDown);
            // choose subcategory
            Driver.SelectOption(SelectDropDownOption.TEXT, shareSkill.SubCategory, SubCategoryDropDown);
            //enter tags
            foreach (var tag in shareSkill.TagsList)
            {
                Driver.Enter(Tags, tag.Text + Keys.Enter);
            }
            //select service type
            if (shareSkill.ServiceType == ShareSkill.ServiceTypeOption.OneOffService)
            {
                Driver.Click(ServiceTypeOneOffOption);
            }
            else
            {
                Driver.Click(ServiceTypeHourlyBasisOption);
            }

            //select location type
            if (shareSkill.LocationType == ShareSkill.LocationTypeOption.OnSite)
            {
                Driver.Click(LocationTypeOnSiteOption);
            }
            else
            {
                Driver.Click(LocationTypeOnlineOption);
            }

            // enter today's date

            // check available days .e.g monday checkbox
            // enter start time
            // enter end time
            if (shareSkill.Availability != null)
            {
                Driver.Enter(StartDateDropDown, shareSkill.Availability.StartDate);
                Driver.Enter(EndDateDropDown, shareSkill.Availability.EndDate); // should be ok when null?

                IList<DayEntry> dayEntries = shareSkill.Availability.DayEntries ?? new List<DayEntry>();
                var dayTimeRow = Driver.FindElements(AvailableDayAndTimes);
                for (int i =0; i < dayEntries.Count; i++)
                {
                    var dayEntry = dayEntries[i];
                    // day of week not necessary anymore?
                    var startTime = dayEntry.ReadableStartTime;
                    var endTime = dayEntry.ReadableEndTime;
                    var isAvailable = dayEntry.IsAvailable;

                    var inputs = dayTimeRow.Where(r =>
                    {
                        var index = r.GetAttribute("index") ?? "-1";
                        if (int.Parse(index) == i)
                        {
                            return true;
                        }
                        return false;
                    }).ToList();

                    // expecting 3 items
                    if (inputs != null && inputs.Count == 3)
                    {
                        if (isAvailable)
                        {
                            inputs[0].Click();
                        }
                        inputs[1].SendKeys(startTime);
                        inputs[2].SendKeys(endTime);
                    }
                }
            }

            //select skill trade credit
            if (shareSkill.SkillTrade == ShareSkill.SkillTradeOption.Credit)
            {
                Driver.Click(SkillTradeCreditOption);
                // enter credit
                Driver.Enter(CreditAmount, shareSkill.Credit.ToString());
            }
            else
            {
                Driver.Click(SkillTradeSkillExchangeOption);
                // enter tag
                foreach (var skillExchangeTag in shareSkill.SkillExchangesList)
                {
                    Driver.Enter(SkillExchange, skillExchangeTag.Text + Keys.Enter);
                }
            }

            //select option for active

            if (shareSkill.Active == ShareSkill.ActiveOption.Hidden)
            {
                Driver.Click(ActiveHiddenOption);
            }
            else
            {
                Driver.Click(ActiveActiveOption);
            }

            //click save
            Driver.Click(Save);
            return new ListingManagementPage(Driver);
        }
    }
}
