using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Models.ViewModels.UserAuthViewModels
{
    public class PermissionViewModel
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string ModuleCount { get; set; }
        public string MenuCount { get; set; }
        public string UserCount { get; set; }

        public List<ModuleViewModel> Modules { get; set; }

    }

    public class ModuleViewModel
    {
        public string Name { get; set; }
        public long Id { get; set; }
        public bool IsChecked { get; set; }
        public List<MenuViewModel> AdminMenus { get; set; }
        public List<MenuViewModel> SiteMenus { get; set; }
    }

    public class MenuViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public bool IsChecked { get; set; }

        public List<MenuItemViewModel> MenuItems { get; set; }
    }

    public class MenuItemViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsChecked { get; set; }
    }
}
