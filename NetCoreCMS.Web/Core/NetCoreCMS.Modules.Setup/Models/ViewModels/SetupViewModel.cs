using NetCoreCMS.Framework.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Setup.Models.ViewModels
{
    public class SetupViewModel
    {
        public string SiteName { get; set; }
        public string DatabaseHost { get; set; }
        public string DatabasePort { get; set; }
        public string DatabaseUserName { get; set; }
        public string DatabasePassword { get; set; }
        public string DatabaseName { get; set; }
        public SupportedDatabase Database { get; set; }

    }
}
