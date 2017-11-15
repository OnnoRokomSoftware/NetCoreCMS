using NetCoreCMS.Framework.Core.Models.ViewModels;
using System.Collections.Generic;

namespace NetCoreCMS.Modules.Blog.Models
{
    public class ArchiveViewModel
    {        
        public bool ShowPostCount { get; set; }
        public bool DisplayAsDropdown { get; set; }
        public List<ArchiveItemViewModel> ItemList { get; set; }
    }
}
