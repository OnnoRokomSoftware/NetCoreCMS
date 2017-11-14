using NetCoreCMS.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Blog.Models
{
    public class CategoryViewModel
    {
        public bool ShowCategoryHasPost { get; set; }
        public bool ShowPostCount { get; set; }
        public bool ShowHierarchy { get; set; }
        public bool DisplayAsDropdown { get; set; }
        public List<NccCategory> CategoryList { get; set; }
    }
}
