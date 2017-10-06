using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.Subscription.Models
{
    public class SubscriptionSettings
    {
        public SubscriptionSettings()
        {
            ModuleVersion = "1.0";
        }

        public string ModuleVersion { get; set; }
    }
}