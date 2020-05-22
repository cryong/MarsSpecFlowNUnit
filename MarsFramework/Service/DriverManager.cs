using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MarsFramework.WebDriver;
using MarsFramework.Factory;
using MarsFramework.SeleniumEventHandlers;

namespace MarsFramework.Service
{
    public static class DriverManager
    {
        private static ThreadLocal<Driver> _driverStored = new ThreadLocal<Driver>();
        /// <summary>
        /// This method return driver base on generic TypeofDriver
        /// Ex: GetDriver<IWebDriver> will return IWebDriver
        /// GetDriver<PhantomJSDriver> will return PhantomJSDriver
        /// </summary>        
        public static Driver GetDriver() => DriverStored;
        
        /// <summary>
        /// This is use for stored driver 
        /// when running in paralell in single machine
        /// </summary>
        private static Driver DriverStored
        {
            get
            {
                if (_driverStored == null || _driverStored.Value == null)
                {
                    throw new Exception("Please call method 'StartDriver' before can get Driver");
                }
                return _driverStored.Value;
            }
            set
            {
                _driverStored.Value = value;
            }
        }
        /// <summary>
        /// This method is use for instance driver
        /// </summary>
        /// <param name="factoryType"></param>
        /// <param name="type"></param>
        /// <param name="configuaration"></param>     
        public static void StartDriver(Browser type)
        {
            //DriverFactory.Create(type, new ReportingSeleniumEventHandler());
            DriverStored = new Driver(DriverFactory.Create(type, new ReportingSeleniumEventHandler()));

        }
        /// <summary>
        /// This method is use for close and destroy driver
        /// </summary>
        public static void CloseDriver()
        {
            DriverStored.Quit();
            if (DriverStored != null)
            {
                DriverStored = null;
            }
        }
    }
}
