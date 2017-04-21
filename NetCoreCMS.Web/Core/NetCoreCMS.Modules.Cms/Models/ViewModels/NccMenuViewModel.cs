using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Models.ViewModels
{
    public class NccMenuViewModel
    {
        public string MenuName { get; set; }
        public string MenuLocation { get; set; }
        public List<NccMenuItemViewModel> MenuItems { get; set; }

    }

    public class NccMenuItemViewModel{
        
    }
}
