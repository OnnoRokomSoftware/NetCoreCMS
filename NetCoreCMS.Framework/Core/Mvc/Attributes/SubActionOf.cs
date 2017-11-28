using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Mvc.Attributes
{
    public class SubActionOf : Attribute
    {
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
