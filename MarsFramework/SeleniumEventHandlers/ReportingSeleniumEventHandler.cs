using AventStack.ExtentReports;
using MarsFramework.Service;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;

namespace MarsFramework.SeleniumEventHandlers
{
    public class ReportingSeleniumEventHandler : ISeleniumEventHandler
    {
        public void OnElementClicked(object sender, WebElementEventArgs e)
        {
            // this actually becomes an issue if you are redirected to a different page afte clicking a button or a link
            // and that button or link is no longer present on the landing page
            try
            {
                string message = $"Clicked on element with tag name: [{e.Element?.TagName ?? ""}] and text : [{e.Element.Text}]";
                WriteMethod(Status.Info, message);
            }
            catch (StaleElementReferenceException)
            {
                WriteMethod(Status.Info, $"Clicked on element");
            }
        }

        public void OnElementClicking(object sender, WebElementEventArgs e)
        {
            string message = $"Clicking on element with tag name: [{e.Element.TagName}] and text : [{e.Element.Text}]";
            WriteMethod(Status.Info, message);
        }

        public void OnElementValueChanged(object sender, WebElementValueEventArgs e)
        {
            string message = $"Changed value to [{e.Value}] on element with tag name: [{e.Element.TagName}]";
            WriteMethod(Status.Info, message);
        }

        public void OnElementValueChanging(object sender, WebElementValueEventArgs e)
        {
            string message = $"Changing value to [{e.Value}] on element with tag name: [{e.Element.TagName}]";
            WriteMethod(Status.Info, message);
        }

        public void OnExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
            string message = $"Exception thrown with message : [{e.ThrownException.Message}]";
            WriteMethod(Status.Info, message);
            // due to slow loading of components, when we use explicit wait, we may see lots of exceptions being thrown
            // logging Info instead of failing
        }

        public void OnFindElementCompleted(object sender, FindElementEventArgs e)
        {
            string message = $"Found element with tag name: [{e.Element?.TagName ?? ""}] using locator : [{e.FindMethod}]";
            WriteMethod(Status.Info, message);
        }

        public void OnFindingElement(object sender, FindElementEventArgs e)
        {
            string message = $"Finding element with tag name: [{e.Element?.TagName ?? ""}] using locator : [{e.FindMethod}]";
            WriteMethod(Status.Info, message);
        }

        public void OnNavigated(object sender, WebDriverNavigationEventArgs e)
        {
            string message = $"Navigated to : [{e.Url}]";
            WriteMethod(Status.Info, message);
        }

        public void OnNavigating(object sender, WebDriverNavigationEventArgs e)
        {
            string message = $"Navigating to : [{e.Url}]";
            WriteMethod(Status.Info, message);
        }

        private void WriteMethod(Status status, string message)
        {
            ExtentTestManager.GetMethod()?.Log(status, message);
        }

    }
}
