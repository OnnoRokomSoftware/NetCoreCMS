using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetCoreCMS.Framework.Utility
{
    public class SetupConfig
    {
        public bool IsComplete { get; set; }
        public string SelectedDatabase { get; set; }
        public string ConnectionString { get; set; }
    }
    
    public class SetupHelper
    {
        private static string _configFileName = "setup.json";
        public static bool IsComplete { get; set; }
        public static string SelectedDatabase { get; set; }
        public static string ConnectionString { get; set; }
        public static SetupConfig LoadSetup(IHostingEnvironment env)
        {
            var config = new SetupConfig();
            var rootDir = env.ContentRootPath;
            var file = File.Open(Path.Combine(rootDir, _configFileName), FileMode.Open);
            using (StreamReader sr = new StreamReader(file))
            {
                var content = sr.ReadToEnd();
                if (!string.IsNullOrEmpty(content))
                {
                    config = JsonConvert.DeserializeObject<SetupConfig>(content);
                    IsComplete = config.IsComplete;
                    SelectedDatabase = config.SelectedDatabase;
                    ConnectionString = config.ConnectionString;
                }
            }
            return config;
        }

        public static SetupConfig SaveSetup(IHostingEnvironment env)
        {
            var config = new SetupConfig();
            var rootDir = env.ContentRootPath;
            var file = File.Open(Path.Combine(rootDir, _configFileName), FileMode.Create);
            using (StreamWriter sw = new StreamWriter(file))
            {
                var content = JsonConvert.SerializeObject(new SetupConfig()
                {
                    IsComplete = IsComplete,
                    SelectedDatabase = SelectedDatabase,
                    ConnectionString = ConnectionString
                },Formatting.Indented);

                if (!string.IsNullOrEmpty(content))
                {
                    sw.Write(content.ToCharArray());
                }
            }
            return config;
        }
    }
}
