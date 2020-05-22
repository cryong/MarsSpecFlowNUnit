using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MarsFramework.Utilities;
using Newtonsoft.Json;

namespace MarsFramework.Config
{
    public class ConfigurationLoader
    {
        public ConfigurationLoader()
        {
            Initialise();
        }

        private void Initialise()
        {
            var configurationLocation = Directory.GetFiles(PathUtil.GetCurrentPath("Config\\"));
            var configurationFile = configurationLocation.FirstOrDefault(x => x.Contains("config") && x.EndsWith(".json"));
            using (StreamReader reader = new StreamReader(configurationFile))
            {
                TestConfig items = JsonConvert.DeserializeObject<TestConfig>(reader.ReadToEnd());
            }
        }
    }
}
