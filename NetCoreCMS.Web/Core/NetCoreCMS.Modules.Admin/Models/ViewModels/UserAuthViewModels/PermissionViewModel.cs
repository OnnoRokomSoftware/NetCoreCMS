using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetCoreCMS.Framework.Core.Models.ViewModels;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Modules.Admin.Models.ViewModels.UserAuthViewModels
{
    [Serializable]
    public class PermissionViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public int Rank { get; set; }

        public int ModuleCount { get; set; }
        public int MenuCount { get; set; }
        public int UserCount { get; set; }

        public List<ModuleViewModel> Modules { get; set; }
        private NccPermission _permission;

        public PermissionViewModel()
        {
            Modules = new List<ModuleViewModel>();
            foreach (var item in GlobalContext.GetActiveModules())
            {
                var module = new ModuleViewModel();

                var menus = item.Menus;
                var adminMenus = menus
                    .Where(x => x.Type == Menu.MenuType.Admin)
                    .GroupBy(y => y.DisplayName,
                        (key, g) => new { MenuName = key, Menu = g.FirstOrDefault(), Items = g.SelectMany(x => x.MenuItems).ToList() }
                    ).ToList();

                var siteMenus = menus.Where(x => x.Type == Menu.MenuType.WebSite)
                    .GroupBy(y => y.DisplayName,
                        (key, g) => new { MenuName = key, Menu = g.FirstOrDefault(), Items = g.SelectMany(z => z.MenuItems).ToList() }
                    ).ToList();

                foreach (var adminMenu in adminMenus)
                {
                    var menu = new MenuViewModel() {
                        Type = "Admin",
                        Name = adminMenu.MenuName,
                        Order = adminMenu.Menu.Order,                        
                        MenuItems = GetMenuItems(adminMenu.Items, item.ModuleId),
                    };
                    module.AdminMenus.Add(menu);
                }

                foreach (var webSiteMenu in siteMenus)
                {
                    var menu = new MenuViewModel()
                    {
                        Type = "WebSite",
                        Name = webSiteMenu.MenuName,
                        Order = webSiteMenu.Menu.Order,                        
                        MenuItems = GetMenuItems(webSiteMenu.Items, item.ModuleId),
                    };
                    module.SiteMenus.Add(menu);
                }
                Modules.Add(module);
            }
        }

        public PermissionViewModel(NccPermission permission)
        {
            _permission = permission;

            Id = permission.Id;
            Group = permission.Group;
            Name = permission.Name;
            Description = permission.Description;
            Rank = permission.Rank;
            ModuleCount = permission.PermissionDetails.GroupBy(x => x.ModuleId).Count();
            MenuCount = permission.PermissionDetails.GroupBy(x => x.Action).Count();
            UserCount = permission.Users.Count;
           
            Modules = new List<ModuleViewModel>();
            foreach (var item in GlobalContext.GetActiveModules())
            {
                var module = new ModuleViewModel();

                var menus = item.Menus;
                var adminMenus = menus
                    .Where(x => x.Type == Menu.MenuType.Admin)
                    .GroupBy(y => y.DisplayName,
                        (key, g) => new { MenuName = key, Menu = g.FirstOrDefault(), Items = g.SelectMany(x => x.MenuItems).ToList() }
                    ).ToList();

                var siteMenus = menus.Where(x => x.Type == Menu.MenuType.WebSite)
                    .GroupBy(y => y.DisplayName,
                        (key, g) => new { MenuName = key, Menu = g.FirstOrDefault(), Items = g.SelectMany(z => z.MenuItems).ToList() }
                    ).ToList();

                foreach (var adminMenu in adminMenus)
                {
                    var menu = new MenuViewModel()
                    {
                        Type = "Admin",
                        Name = adminMenu.MenuName,
                        Order = adminMenu.Menu.Order,
                        MenuItems = GetMenuItems(adminMenu.Items, item.ModuleId),
                    };
                    module.AdminMenus.Add(menu);
                }

                foreach (var webSiteMenu in siteMenus)
                {
                    var menu = new MenuViewModel()
                    {
                        Type = "WebSite",
                        Name = webSiteMenu.MenuName,
                        Order = webSiteMenu.Menu.Order,
                        MenuItems = GetMenuItems(webSiteMenu.Items, item.ModuleId),
                    };
                    module.SiteMenus.Add(menu);
                }
                Modules.Add(module);
            }
        }

        private List<MenuItemViewModel> GetMenuItems(List<MenuItem> menuItems, string moduleId)
        {
            var list = new List<MenuItemViewModel>();
            foreach (var item in menuItems)
            {
                string controller = "", action = "";
                //if (string.IsNullOrEmpty(item.Url) == false)
                //{
                //    var parts = item.Url.Split("/".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                //    if (parts.Length > 1)
                //    {
                //        controller = parts[0];
                //        action = parts[1];
                //    }
                //    else if (parts.Length == 1) {
                //        controller = parts[0];
                //        action = "Index";
                //    }
                //}

                if (string.IsNullOrEmpty(item.Url) == false)
                {
                    (controller, action) = NccUrlHelper.GetControllerActionFromUrl(item.Url);
                }
                else
                {
                    controller = item.Controller;
                    action = item.Action;
                }

                var mip = _permission?.PermissionDetails?.Where(x => x.ModuleId == moduleId && x.Action == action && x.Controller == controller).FirstOrDefault();
                if (mip != null)
                {
                    var mi = new MenuItemViewModel()
                    {
                        Id = mip.Id,
                        Action = action,
                        Controller = controller,
                        Name = mip.Name,
                        Order = mip.Order,
                        IsChecked = true,
                    };
                    list.Add(mi);
                }
                else
                {
                    var mi = new MenuItemViewModel()
                    {
                        Id = 0,
                        Action = action,
                        Controller = controller,
                        Name = item.Name,
                        Order = item.Order
                    };
                    list.Add(mi);
                }                
            }
            return list;
        }
    }

    [Serializable]
    public class ModuleViewModel
    {
        public string Name { get; set; }
        public string ModuleId { get; set; }
        public bool IsChecked { get; set; }
        public List<MenuViewModel> AdminMenus { get; set; }
        public List<MenuViewModel> SiteMenus { get; set; }

        public ModuleViewModel()
        {
            Name = "";
            ModuleId = "";
            IsChecked = false;

            AdminMenus = new List<MenuViewModel>();
            SiteMenus = new List<MenuViewModel>();            
        }
    }

    [Serializable]
    public class MenuViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public bool IsChecked { get; set; }
        public int Order { get; set; }

        public List<MenuItemViewModel> MenuItems { get; set; }

        public MenuViewModel()
        {
            MenuItems = new List<MenuItemViewModel>();
            Name = "";
            Url = "";
            Type = "";
            IsChecked = false;
            Order = 0;
        }
    }

    [Serializable]
    public class MenuItemViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsChecked { get; set; }
        public int Order { get; set; }

        public MenuItemViewModel()
        {
            Name = "";
            Controller = "";
            Action = "";
            IsChecked = false;
            Order = 0;
        }
    }
}
