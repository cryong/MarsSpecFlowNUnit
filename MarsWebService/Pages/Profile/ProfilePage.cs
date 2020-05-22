using MarsFramework.WebDriver;
using MarsWebService.Pages.Sections.Profile.Sections;

namespace MarsWebService.Pages.Profile
{
    public class ProfilePage : NavigatableBasePage
    {
        public override string Url => "http://localhost:5000/Account/Profile";
        public DescriptionSection DescriptionSection { get; private set; }
        public AvailabilitySection GeneralInformationSection { get; private set; }
        public MainSection MainSection { get; private set; }

        public ProfilePage(Driver driver) : base(driver)
        {
            DescriptionSection = new DescriptionSection(driver);
            GeneralInformationSection = new AvailabilitySection(driver);
            MainSection = new MainSection(driver);
        }
    }
}
