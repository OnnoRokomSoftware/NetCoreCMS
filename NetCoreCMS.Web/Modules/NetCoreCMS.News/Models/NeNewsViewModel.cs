using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.News.Models
{
    public class NeNewsViewModel
    {
        public string HeaderTitle { get; set; }
        public string ColumnClass { get; set; }
        public string ColumnColor { get; set; }
        public string ColumnBgColor { get; set; }
        public string FooterTitle { get; set; }
        public List<NeNews> NeNewsList { get; set; }
    }
}