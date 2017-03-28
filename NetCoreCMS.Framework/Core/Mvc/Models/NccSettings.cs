using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Mvc.Models
{
    public class NccSettings
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
