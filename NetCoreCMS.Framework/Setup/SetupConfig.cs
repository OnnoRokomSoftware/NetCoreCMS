using Microsoft.AspNetCore.Hosting;
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
    }

    public class StartupTypes {
        public static string Default { get { return "Default"; } }
        public static string Page { get { return "Page"; } }
        public static string Post { get { return "Post"; } }
        public static string Category { get { return "Category"; } }
        public static string Module { get { return "Module"; } }
    }
}
