/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

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
        public string StartupType { get; set; } = StartupTypeText.Url;
        public string StartupData { get; set; } = "/CmsHome";
        public string StartupUrl { get; set; } = "/CmsHome";
        public string TablePrefix { get; set; } = "ncc_";
        public bool EnableCache { get; set; } = false;

        public int LoggingLevel { get; set; } = (int) LogLevel.Warning;
        public bool IsMaintenanceMode { get; set; }
        public int MaintenanceDownTime { get; set; } = 30;
        public string MaintenanceMessage { get; set; } = "Doing Maintenance. Comming back soon...";
        public string Language { get; set; }
    }

    public class StartupTypeText {
        public static string Url { get { return "Url"; } }
        public static string Page { get { return "Page"; } }
        public static string Post { get { return "Post"; } }
        public static string Category { get { return "Category"; } }
        public static string Module { get { return "Module"; } }
        public static string Tag { get { return "Tag"; } } 
    }
}
