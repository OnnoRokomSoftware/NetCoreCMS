/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Models.ViewModels;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Admin.Models.ViewModels.UserAuthViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.Admin.Models.ViewModels
{
    public class UserViewModel
    {
        public long Id { get; set; }

        public long PermissionId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required(ErrorMessage = "The Email field is required")]
        [EmailAddress]
        public string Email { get; set; }

        public string Mobile { get; set; }

        public bool SendEmailNotification { get; set; }

        [Required]
        public long[] Roles { get; set; }
        
        public List<ModuleViewModel> DenyModules { get; set; }
        public List<ModuleViewModel> AllowModules { get; set; }
        public string RoleNames { get; set; }

        private readonly NccPermission _permission;
        private readonly NccUser _nccUser;

        public UserViewModel()
        {
            DenyModules = new List<ModuleViewModel>();
            AllowModules = new List<ModuleViewModel>();

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
                DenyModules.Add(module);
                AllowModules.Add(module);
            }
        }

        public UserViewModel(NccPermission permission)
        {
            _permission = permission;
            PermissionId = permission.Id;
            
            DenyModules = new List<ModuleViewModel>();
            AllowModules = new List<ModuleViewModel>();

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
                DenyModules.Add(module);
                AllowModules.Add(module);
            }
        }

        public UserViewModel(NccUser user)
        {
            _nccUser = user;

            Id = user.Id;
            Email = user.Email;
            FullName = user.FullName;
            Mobile = user.Mobile;
            UserName = user.UserName;

            DenyModules = new List<ModuleViewModel>();
            AllowModules = new List<ModuleViewModel>();
            
            foreach (var item in GlobalContext.GetActiveModules())
            {
                var module = new ModuleViewModel();
                module.ModuleId = item.ModuleId;
                module.Name = item.ModuleTitle;
                
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

                AllowModules.Add(module);
                DenyModules.Add(NccHelper.DeepClone(module));                
            }
            
            foreach (var module in AllowModules)
            {
                foreach (var adminMenu in module.AdminMenus)
                {
                    foreach (var adminMenuItem in adminMenu.MenuItems)
                    {
                        var pd = user.ExtraPermissions.Where(
                            x => x.ModuleId == module.ModuleId
                            && x.AllowUser != null
                            && x.ExtraAllowUserId == user.Id
                            && x.Action == adminMenuItem.Action
                            && x.Controller == adminMenuItem.Controller
                            ).FirstOrDefault();
                        if (pd != null)
                        {
                            module.IsChecked = true;
                            adminMenu.IsChecked = true;
                            adminMenuItem.Id = pd.Id;
                            adminMenuItem.Name = pd.Name;                            
                            adminMenuItem.IsChecked = true;
                        }
                        else
                        {
                            adminMenuItem.IsChecked = false;
                        }
                    }
                }

                foreach (var siteMenu in module.SiteMenus)
                {
                    foreach (var siteMenuItem in siteMenu.MenuItems)
                    {
                        var pd = user.ExtraPermissions.Where(
                            x => x.ModuleId == module.ModuleId
                            && x.AllowUser != null
                            && x.ExtraAllowUserId == user.Id
                            && x.Action == siteMenuItem.Action
                            && x.Controller == siteMenuItem.Controller
                            ).FirstOrDefault();
                        if (pd != null)
                        {
                            module.IsChecked = true;
                            siteMenu.IsChecked = true;
                            siteMenuItem.Id = pd.Id;
                            siteMenuItem.Name = pd.Name;                            
                            siteMenuItem.IsChecked = true;
                        }
                        else
                        {
                            siteMenuItem.IsChecked = false;
                        }
                    } 
                }
            }

            foreach (var module in DenyModules)
            {
                foreach (var adminMenu in module.AdminMenus)
                {
                    foreach (var adminMenuItem in adminMenu.MenuItems)
                    {
                        var pd = user.ExtraDenies.Where(
                            x => x.ModuleId == module.ModuleId
                            && x.DenyUser != null
                            && x.ExtraDenyUserId == user.Id
                            && x.Action == adminMenuItem.Action
                            && x.Controller == adminMenuItem.Controller
                            ).FirstOrDefault();

                        if (pd != null)
                        {
                            module.IsChecked = true;
                            adminMenu.IsChecked = true;
                            adminMenuItem.Id = pd.Id;
                            adminMenuItem.Name = pd.Name;                            
                            adminMenuItem.IsChecked = true;
                        }
                        else
                        {
                            adminMenuItem.IsChecked = false;
                        }
                    }

                }

                foreach (var siteMenu in module.SiteMenus)
                {
                    foreach (var siteMenuItem in siteMenu.MenuItems)
                    {
                        var pd = user.ExtraDenies.Where(
                            x => x.ModuleId == module.ModuleId
                            && x.DenyUser != null
                            && x.ExtraDenyUserId == user.Id
                            && x.Action == siteMenuItem.Action
                            && x.Controller == siteMenuItem.Controller
                            ).FirstOrDefault();

                        if (pd != null)
                        {
                            module.IsChecked = true;
                            siteMenu.IsChecked = true;
                            siteMenuItem.Id = pd.Id;
                            siteMenuItem.Name = pd.Name;                            
                            siteMenuItem.IsChecked = true;
                        }
                        else
                        {
                            siteMenuItem.IsChecked = false;
                        }
                    }
                }
            } 
        }

        private List<MenuItemViewModel> GetMenuItems(List<MenuItem> menuItems, string moduleId)
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
                    else if (parts.Length == 1)
                    {
                        controller = parts[0];
                        action = "Index";
                    }
                }

                if (_permission != null)
                {
                    var mip = _permission.PermissionDetails?.Where(x => x.ModuleId == moduleId && x.Action == action && x.Controller == controller).FirstOrDefault();
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
                            Order = item.Order,
                            IsChecked = false
                        };
                        list.Add(mi);
                    }
                }
                else
                {
                    var mi = new MenuItemViewModel()
                    {
                        Action = action,
                        Controller = controller,
                        Name = item.Name,
                        Order = item.Order,
                        IsChecked = false,
                    };

                    list.Add(mi);
                }
            }
            return list;
        }
    }
}
