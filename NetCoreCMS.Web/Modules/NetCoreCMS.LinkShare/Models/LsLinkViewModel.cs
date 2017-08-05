using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.LinkShare.Models
{
    public class LsLinkViewModel
    {
        public string ColumnClass { get; set; }
        public string ColumnColor { get; set; }
        public List<LsLink> LsLinkList { get; set; }
    }
}