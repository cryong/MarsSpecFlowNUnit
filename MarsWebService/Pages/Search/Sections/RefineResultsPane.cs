using MarsFramework.WebDriver;
using OpenQA.Selenium;

namespace MarsWebService.Pages.Sections.Search.Sections
{
    public class RefineResultsPane
    {
        private readonly Driver _driver;
        public RefineResultsPane(Driver driver)
        {
            _driver = driver;
        }

        public void FilterResultsByLocationType(string type)
        {
            _driver.Click(By.XPath($"//div[.='Filter']/following-sibling::div/button['{type}']"));
        }

        public void FilterByCategory(string category)
        {
            _driver.Click(By.XPath($"//div[@role='list']/a[descendant-or-self::text()='{category}' and contains(@class, 'category')]"));
        }

        public void FilterBySubCategory(string subcategory)
        {
            _driver.Click(By.XPath($"//a[contains(@class, 'subcategory')][text()='{subcategory}']"));
        }

    }
}
