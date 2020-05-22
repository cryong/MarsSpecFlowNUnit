using MarsFramework.WebDriver;
using OpenQA.Selenium;
using MarsWebService.Pages.Search;

namespace MarsWebService.Pages.Common
{
    public class SearchBar
    {
        private const string searchBoxXPath = "//div[contains(@class, 'menu')]/div[contains(@class, 'search-box')]/input[@placeholder='Search skills']";
        private By SearchBox => By.XPath(searchBoxXPath);
        private By SearchButton => By.XPath($"{searchBoxXPath}/following-sibling::i");

        private readonly Driver _driver;
        public SearchBar(Driver driver)
        {
            _driver = driver;
        }

        public SearchResultPage SearchSkill(string skillTitle)
        {
            _driver.Enter(SearchBox, skillTitle);
            _driver.Click(SearchButton);
            return new SearchResultPage(_driver);
        }
    }
}
