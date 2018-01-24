/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.EasyNews.Models.Entities;
using System.Collections.Generic;

namespace NetCoreCMS.EasyNews.Models.ViewModels
{
    public class NewsViewModel
    {
        public string HeaderTitle { get; set; }
        public string HeaderColor { get; set; }
        public string HeaderBgColor { get; set; }

        public string ColumnClass { get; set; }
        public string ColumnColor { get; set; }
        public string ColumnBgColor { get; set; }
        public string Scrollamount { get; set; }
        public string Height { get; set; }

        public string FooterTitle { get; set; }
        public string FooterColor { get; set; }
        public string FooterBgColor { get; set; }
        public List<News> NewsList { get; set; }
    }
}