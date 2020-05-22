using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsFramework.Contexts;
using MarsFramework.ExtentReport;
using MarsFramework.SeleniumEventHandlers;
using OpenQA.Selenium;

namespace MarsFramework.Decorators
{
    // TODO probably change the name now...
    public class ExtentReportDriver : DriverDecorator
    {

        private readonly ISelenumEventHandler eventHandler;
        public ExtentReportDriver(IDriver driver, ISelenumEventHandler handler) : base(driver)
        {
            eventHandler = handler;
        }

        public override void Start(Browser browser)
        {
            
            base.Start(browser);
            //Driver?.Start(browser);
        }

        private void Write()
        {
            // HOW DO I GET THE TEST OBJECT HERE?
            //https://stackoverflow.com/questions/34635740/logging-as-a-decorator-vs-dependency-injection-what-if-i-need-to-log-inside-t
            //https://medium.com/@cummingsi1993/the-decorator-pattern-a-simple-guide-333f5e79a9fd
            //https://dzone.com/articles/is-inheritance-dead
            var x = ExtentReportContext.Instance;
            //x.StartedReporterList[0].
        }


        private void WirteToReport(string content)
        {
            ExtentTestManager.GetTest();
            //new LoggerDriver(new ExtentReportDriver(new Driver())).GoToUrl("xxx");
            // console.writeline("aaa");
            // report.writeline();
        }


        //private void AttachEventListner(ISelenumEventHandler eventHandler)
        //{
        //    Driver.
        //}
    }
}
