using System.Collections.Generic;
using System.Linq;
using MarsFramework.WebDriver;
using OpenQA.Selenium;

namespace MarsWebService.Pages.Sections.Search.Sections
{
    public class SearchResultsSection
    {
        private readonly Driver _driver;
        public SearchResultsSection(Driver driver)
        {
            _driver = driver;
        }

        public SearchResults SearchResults => new SearchResults(_driver);
    }

    public class SearchResults // inner section
    {
        private readonly Driver _driver;
        public SearchResults(Driver driver)
        {
            _driver = driver;
        }

        public SearchResult this[int position]
        {
            get
            {
                return SearchResultList.ElementAt(position); ;
            }
        }

        public IEnumerable<SearchResult> SearchResultList
        {
            get
            {
                return _driver.FindElements(By.CssSelector("div.ui.card"))?.Select(r => new SearchResult(r)).ToList();
            }
        }

    }
    public class SearchResult // fragment
    {
        private readonly IWebElement _rootElement;
        public SearchResult(IWebElement root)
        {
            _rootElement = root;
        }

        private By ImageLink => By.CssSelector("a");
        private By SellerInfoLink => By.CssSelector("div > a.seller-info");
        private By SkillInfoLink => By.CssSelector("div > a.service-info");
        private By Rating => By.CssSelector("div > div.rating");
        private By SkillPayment => By.CssSelector("div.extra.content > label");
        public string GetSkillTitle() => _rootElement.FindElement(SkillInfoLink).Text;
    }

}
