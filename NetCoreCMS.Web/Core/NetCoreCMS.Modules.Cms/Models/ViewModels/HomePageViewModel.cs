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
        public NccPost  StickyPost { get; set; }
        public List<NccPost> FeaturedPosts { get; set; }
        public List<NccPost> AllPosts { get; set; }
    }
}
