using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetCoreCMS.Framework.Setup
{
    public class SetupConfig
    {
        public bool IsDbCreateComplete { get; set; }
        public bool IsAdminCreateComplete { get; set; }
        public string SelectedDatabase { get; set; }
        public string ConnectionString { get; set; }
        public string StartupUrl { get; set; } = "/CmsHome";
        public string StartupType { get; set; } = StartupTypes.Default;
        public int LoggingLevel { get; set; } = (int) LogLevel.Debug;
        public bool IsMaintenanceMode { get; set; }
        public int MaintenanceDownTime { get; set; } = 30;
        public string MaintenanceMessage { get; set; } = "Doing Maintenance. Comming back soon...";
    }

    public class StartupTypes {
        public static string Default { get { return "Default"; } }
        public static string Page { get { return "Page"; } }
        public static string Post { get { return "Post"; } }
        public static string Category { get { return "Category"; } }
        public static string Module { get { return "Module"; } }
    }
}
