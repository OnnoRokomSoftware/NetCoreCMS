using NetCoreCMS.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Blog.Models
{
    public class BlogPostSliderViewModel
    {
        public BlogPostSliderViewModel()
        {
            IsAutoScroll = true;
            Interval = 3;
            TotalPost = 5;
            DisplayPost = 1;
            ShowSideNav = true;
            ShowBottomNav = true;

            IsFeatured = false;
            IsSticky = false;

            CategoryId = 0;
            CategoryList = new List<NccCategory>();
            PostList = new List<NccPost>();
        }

        public string DesignTemplate { get; set; }
        public string HeaderText { get; set; }

        public bool IsAutoScroll { get; set; }
        public int Interval { get; set; } //second
        public int TotalPost { get; set; }
        public int DisplayPost { get; set; }
        public bool ShowSideNav { get; set; }
        public bool ShowBottomNav { get; set; }

        public bool IsFeatured { get; set; }
        public bool IsSticky { get; set; }

        public long CategoryId { get; set; }

        public List<NccCategory> CategoryList { get; set; }
        public List<NccPost> PostList { get; set; }
    }
}
