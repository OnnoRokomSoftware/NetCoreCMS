using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.LinkShare.Models
{
    public class LsLinkCategory
    {
        public long LsCategoryId { get; set; }
        public LsCategory LsCategory { get; set; }
        public long LsLinkId { get; set; }
        public LsLink LsLink { get; set; }
    }
}