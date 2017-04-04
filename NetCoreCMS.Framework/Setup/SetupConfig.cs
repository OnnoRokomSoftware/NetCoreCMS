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
        public bool IsComplete { get; set; }
        public string SelectedDatabase { get; set; }
        public string ConnectionString { get; set; }
    }
}
