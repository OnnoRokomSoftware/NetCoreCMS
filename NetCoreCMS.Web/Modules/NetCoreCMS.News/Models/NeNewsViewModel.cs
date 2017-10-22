/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using System.Collections.Generic;

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