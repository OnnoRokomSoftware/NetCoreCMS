using NetCoreCMS.Framework.Core.Models.ViewModels;
using System.Collections.Generic;

namespace NetCoreCMS.Modules.Blog.Models
{
    public class TagCloudViewModel
    {        
        public bool ShowTagHasPost { get; set; }
        public bool ShowPostCount { get; set; }
        public List<TagCloudItemViewModel> ItemList { get; set; }
    }
}
