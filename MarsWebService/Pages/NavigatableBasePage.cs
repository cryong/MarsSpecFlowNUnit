using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsFramework.WebDriver;

namespace MarsWebService.Pages
{
    public abstract class NavigatableBasePage : BasePage
    {
        public NavigatableBasePage(Driver driver) : base(driver)
        {
        }
        public abstract string Url { get; }

        public void Open()
        {
            Driver.GoToUrl(Url);
            WaitForPageLoad();
        }
    }
}
