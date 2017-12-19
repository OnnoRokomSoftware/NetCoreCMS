using NetCoreCMS.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Blog.Models
{
    public class RecentPostViewModel
    {
        public bool IsDateShow { get; set; }
        public List<NccPost> PostList { get; set; }
    }
}
