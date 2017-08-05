using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.LinkShare.Models
{
    public class LinkShareSettings
    {
        public LinkShareSettings()
        {
            ModuleVersion = "1.0";
        }

        public string ModuleVersion { get; set; }
    }
}