/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

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