using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Admin.Models.Dto
{
    public class StoreSettings
    {
        public StoreSettings()
        {
            StoreBaseUrl = "http://dotnetcorecms.com/";
            SecretKey = "DemoUserKey";
        }

        public string StoreBaseUrl { get; set; }
        public string SecretKey { get; set; }
    }
}
