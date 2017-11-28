/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Models.ViewModels
{
    public class HomePageViewModel
    {
        public string CurrentLanguage { get; set; }
        public long TotalPost { get; set; }
        public int PostPerPage { get; set; }
        public int PageNumber { get; set; }
        public int PreviousPage { get; set; }
        public int NextPage { get; set; }
        public int TotalPage { get; set; }
        public List<NccPost>  StickyPosts { get; set; }
        public List<NccPost> FeaturedPosts { get; set; }
        public List<NccPost> AllPosts { get; set; }
    }
}
