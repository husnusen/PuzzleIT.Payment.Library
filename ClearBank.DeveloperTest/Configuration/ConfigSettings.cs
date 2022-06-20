using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ClearBank.DeveloperTest.Configuration
{
    public class ConfigSettings : IConfigSettings
    {
        public string DataStoreType => ConfigurationManager.AppSettings["DataStoreType"];
    }
}
