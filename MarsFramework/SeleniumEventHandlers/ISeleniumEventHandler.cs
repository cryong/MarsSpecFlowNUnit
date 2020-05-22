using OpenQA.Selenium.Support.Events;

namespace MarsFramework.SeleniumEventHandlers
{
    public interface ISeleniumEventHandler
    {
        void OnExceptionThrown(object sender, WebDriverExceptionEventArgs e);
        void OnNavigated(object sender, WebDriverNavigationEventArgs e);
        void OnNavigating(object sender, WebDriverNavigationEventArgs e);
        void OnElementClicked(object sender, WebElementEventArgs e);
        void OnElementClicking(object sender, WebElementEventArgs e);
        void OnElementValueChanged(object sender, WebElementValueEventArgs e);
        void OnElementValueChanging(object sender, WebElementValueEventArgs e);
        void OnFindingElement(object sender, FindElementEventArgs e);
        void OnFindElementCompleted(object sender, FindElementEventArgs e);
    }
}
