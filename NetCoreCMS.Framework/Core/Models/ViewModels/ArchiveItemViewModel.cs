using NetCoreCMS.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Models.ViewModels
{
    public class ArchiveItemViewModel
    {
        public string Year { get; set; }
        public string Month { get; set; }
        public int MonthValue { get; set; }
        public int TotalPost { get; set; }
    }
}
