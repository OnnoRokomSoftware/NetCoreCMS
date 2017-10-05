using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.News.Models
{
    public class NeNewsCategory
    {
        public long NeCategoryId { get; set; }
        public NeCategory NeCategory { get; set; }
        public long NeNewsId { get; set; }
        public NeNews NeNews { get; set; }
    }
}