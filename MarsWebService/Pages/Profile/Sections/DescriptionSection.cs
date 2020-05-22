using MarsFramework.WebDriver;
using OpenQA.Selenium;

namespace MarsWebService.Pages.Sections.Profile.Sections
{
    public class DescriptionSection
    {
        private By Description => By.Name("value");
        private By EditButton => By.XPath("//h3[.='Description']/span/i");
        private By SaveButton => By.XPath("//div[node()=textarea]/following-sibling::button[.='Save']");

        private readonly Driver _driver;

        public DescriptionSection(Driver driver)
        {
            _driver = driver;
        }

        public void UpdateDescription(string description)
        {
            _driver.Click(EditButton);
            _driver.Enter(Description, description);
            _driver.Click(SaveButton);
        }

        public bool IsOpen()
        {
            return _driver.FindElement(SaveButton).Displayed;
        }

    }
}
