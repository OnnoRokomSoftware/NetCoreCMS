/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccSettings : BaseModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
     
    }
}
