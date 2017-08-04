using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.LinkShare.Models
{
    public class NccCategoryLinkShare
    {
        [Key]
        public long Id { get; set; }
        public long NccCategoryId { get; set; }
        public NccCategory Category { get; set; }
        public long NccLinkShareId { get; set; }
        public NccLinkShare NccLinkShare { get; set; }
    }
}