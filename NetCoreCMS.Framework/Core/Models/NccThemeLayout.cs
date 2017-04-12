/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccThemeLayout : BaseModel
    {   
        public NccTheme Theme { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string NetCoreCMSVersion { get; set; }        
    }
}
