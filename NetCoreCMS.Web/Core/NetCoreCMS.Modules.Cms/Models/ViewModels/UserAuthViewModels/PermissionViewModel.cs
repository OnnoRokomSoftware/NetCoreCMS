using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetCoreCMS.Framework.Core.Models.ViewModels;

namespace NetCoreCMS.Modules.Cms.Models.ViewModels.UserAuthViewModels
{
    public class PermissionViewModel
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }

        public string ModuleCount { get; set; }
        public string MenuCount { get; set; }
        public string UserCount { get; set; }

        public List<ModuleViewModel> Modules { get; set; }
        
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
                        MenuItems = GetMenuItems(adminMenu.Items),
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
                        MenuItems = GetMenuItems(webSiteMenu.Items),
                    };
                    module.SiteMenus.Add(menu);
                }
                Modules.Add(module);
            }
        }

        private List<MenuItemViewModel> GetMenuItems(List<MenuItem> menuItems)
        {
            var list = new List<MenuItemViewModel>();
            foreach (var item in menuItems)
            {
                string controller = "", action = "";
                if (string.IsNullOrEmpty(item.Url) == false)
                {
                    var parts = item.Url.Split("/".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 1)
                    {
                        controller = parts[0];
                        action = parts[1];
                    }
                }
                var mi = new MenuItemViewModel()
                {
                    Action = action,
                    Controller = controller,
                    Name = item.Name,
                    Order = item.Order
                };
                list.Add(mi);
            }
            return list;
        }
    }

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
