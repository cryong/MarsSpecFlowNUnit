using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsFramework.WebDriver;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MarsFramework.Config
{
    public class TestConfig
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName ="Browser")]
        public static Browser Browser { get; set; }

        [JsonProperty(PropertyName ="BaseURL")]
        public static string BaseURL { get; set; }

        [JsonProperty(PropertyName = "ReportPath")]
        public static string ReportPath { get; set; }

        [JsonProperty(PropertyName = "TestDataPath")]
        public static string TestDataPath { get; set; }
        [JsonProperty(PropertyName = "ScreenShotPath")]
        public static string ScreenShotPath { get; set; }
    }
}
