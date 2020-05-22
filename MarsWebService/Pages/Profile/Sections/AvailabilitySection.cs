using System.Collections.Generic;
using System.Linq;
using MarsFramework.WebDriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MarsWebService.Pages.Sections.Profile.Sections
{
    public class AvailabilitySection
    {
        private readonly Driver _driver;

        public EditableDropDownFieldGroup DropDownFieldGroup => new EditableDropDownFieldGroup(_driver);
        public AvailabilitySection(Driver driver)
        {
            _driver = driver;
        }

        public EditableDropDownFieldGroup.EditableDropDownField GetAvailabilityHours()
        {
            return DropDownFieldGroup.DropDownList.ElementAt(1); //2nd element
        }
    }

    public class EditableDropDownFieldGroup
    {
        private readonly Driver _driver;
        public EditableDropDownFieldGroup(Driver driver)
        {
            _driver = driver;
        }

        public IEnumerable<EditableDropDownField> DropDownList
        {
            get
            {
                return _driver.FindElements(By.CssSelector("div.extra.content > div.ui.list > div")).Select(r => new EditableDropDownField(r)).ToList();
            }
        }

        public class EditableDropDownField //fragment
        {
            private readonly IWebElement _rootElement;
            public EditableDropDownField(IWebElement root)
            {
                _rootElement = root;
            }
            private By UpdateOrCanceLink => By.CssSelector("div > span > i.icon.small");
            private By DropDownField => By.CssSelector("div > span > select");
            public void SelectByText(string optionText)
            {
                // there seems to be an issue where if you are trying to select an ALREADY selected
                // option then selecting by text WILL NOT work due to "hidden" option
                // only way to get around it is by using JS or using other select methods
                _rootElement.FindElement(UpdateOrCanceLink).Click();
                var select = new SelectElement(_rootElement.FindElement(DropDownField));
                select.SelectByText(optionText);
            }

            public void SelectByValue(string value)
            {
                _rootElement.FindElement(UpdateOrCanceLink).Click();
                var select = new SelectElement(_rootElement.FindElement(DropDownField));
                select.SelectByValue(value);
            }
        }
    }


}
