using MarsFramework.Config;

namespace MarsFramework.Contexts
{
    public class FrameworkContext
    {
        public FrameworkContext() { }

        public void Initialise()
        {
            // load config file
            _ = new ConfigurationLoader();
            // initialise extent report
            _ = ExtentReportContext.Instance;
            // load any pre-requisite data (at test suite level) i.e. excel spreadsheet, csv etc
        }
    }
}
