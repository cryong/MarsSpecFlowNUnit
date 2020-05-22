using System;
using MarsFramework.WebDriver;
using MarsWebService.Pages.Sections.Search.Sections;

namespace MarsWebService.Pages.Search
{
    public class SearchResultPage : BasePage
    {
        public RefineResultsPane RefineResultsPane { get; }
        public SearchResultsSection ResultSection { get; }

        public SearchResultPage(Driver driver) : base(driver)
        {
            RefineResultsPane = new RefineResultsPane(driver);
            ResultSection = new SearchResultsSection(driver);
        }
    }
}
