/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using System.Reflection;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Messages;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Auth;

namespace NetCoreCMS.Framework.Utility
{
    /// <summary>
    /// A helper class for getting admin and website menus
    /// </summary>
    public class NccMenuHelper
    {
        /// <summary>
        /// Provide admin menu and it's items
        /// </summary>
        /// <returns>A dictionary of menu and menu items. Key is the menu and valus is a list of menu items.</returns>
        public static Dictionary<AdminMenu, List<AdminMenuItem>> GetModulesAdminMenus()
        { 
            Dictionary<AdminMenu, List<AdminMenuItem>> adminMenuDic = new Dictionary<AdminMenu, List<AdminMenuItem>>();

            foreach (var module in GlobalContext.Modules)
            {
                if (module.ModuleStatus == (int) NccModule.NccModuleStatus.Active)
                {
                    var controllers = module.Assembly.DefinedTypes.Select(t => t.AsType()).Where(x => typeof(NccController).IsAssignableFrom(x));
                    foreach (var controller in controllers)
                    {
                        try
                        {
                            var atrib = controller.GetTypeInfo().GetCustomAttribute<AdminMenu>();
                            if (atrib != null && atrib.IsVisible)
                            {
                                var key = adminMenuDic.Keys.Where(x => x.Name == atrib.Name).FirstOrDefault();

                                if (key == null)
                                {
                                    adminMenuDic.Add(atrib, new List<AdminMenuItem>());
                                    key = atrib;
                                }
                                var actions = controller.GetMethods();
                                foreach (var item in actions)
                                {
                                    var menuItem = item.GetCustomAttribute<AdminMenuItem>();
                                    if (menuItem != null && menuItem.IsVisible)
                                    {
                                        menuItem.Controller = controller.Name.Substring(0, controller.Name.Length-10);
                                        menuItem.Action = item.Name;
                                        adminMenuDic[key].Add(menuItem);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobalMessageRegistry.RegisterMessage(new GlobalMessage() {For = GlobalMessage.MessageFor.Admin, Registrater="MenuHelper", Text = ex.Message, Type = GlobalMessage.MessageType.Error }, new TimeSpan(0, 1, 0));
                        }
                    }
                }
            }
            return adminMenuDic;
        }

        /// <summary>
        /// Public Website menu and menu items exposed by modules.
        /// </summary>
        /// <returns>Site menu and it's menu items list as dictionaly. Where key is the site menu and value is site menu items.</returns>
        public static Dictionary<SiteMenu, List<SiteMenuItem>> GetModulesSiteMenus()
        {
            Dictionary<SiteMenu, List<SiteMenuItem>> siteMenuDic = new Dictionary<SiteMenu, List<SiteMenuItem>>();

            foreach (var module in GlobalContext.Modules)
            {
                if (module.ModuleStatus == (int)NccModule.NccModuleStatus.Active)
                {
                    var controllers = module.Assembly.DefinedTypes.Select(t => t.AsType()).Where(x => typeof(NccController).IsAssignableFrom(x));
                    foreach (var controller in controllers)
                    {
                        try
                        {
                            var atrib = controller.GetTypeInfo().GetCustomAttribute<SiteMenu>();
                            if (atrib != null && atrib.IsVisible)
                            {
                                var key = siteMenuDic.Keys.Where(x => x.Name == atrib.Name).FirstOrDefault();

                                if (key == null)
                                {
                                    siteMenuDic.Add(atrib, new List<SiteMenuItem>());
                                    key = atrib;
                                }
                                var actions = controller.GetMethods();
                                foreach (var item in actions)
                                {
                                    var menuItem = item.GetCustomAttribute<SiteMenuItem>();
                                    if (menuItem != null && menuItem.IsVisible)
                                    {
                                        if (string.IsNullOrEmpty(menuItem.Url))
                                        {
                                            menuItem.Controller = controller.Name.Substring(0, controller.Name.Length - 10);
                                            menuItem.Action = item.Name;
                                            menuItem.Url = "/" + menuItem.Controller + "/" + menuItem.Action;
                                        }
                                        siteMenuDic[key].Add(menuItem);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobalMessageRegistry.RegisterMessage(new GlobalMessage() { For = GlobalMessage.MessageFor.Admin, Registrater = "MenuHelper", Text = ex.Message, Type = GlobalMessage.MessageType.Error }, new TimeSpan(0, 1, 0));
                        }
                    }
                }
            }

            return siteMenuDic;
        }

        /// <summary>
        /// Provide generated menu HTML code using ul and li
        /// </summary>
        /// <param name="adminMenuDic">Module admin menu list</param>
        /// <param name="userId">Logged user Id</param>
        /// <returns>Menu HTML text</returns>
        public static string GetAdminMenuHtml(Dictionary<AdminMenu, List<AdminMenuItem>> adminMenuDic, long userId)
        {
            var userService = (NccUserService)GlobalContext.ServiceProvider.GetService(typeof(NccUserService));
            var user = userService.Get(userId, true);

            var menuStr = "";
            var orderdMenu = adminMenuDic.OrderBy(x => x.Key.Order);

            foreach (var item in orderdMenu)
            {
                if (item.Key.IsVisible == false)
                    continue;

                string menuIcon = "fa-list";
                if (!string.IsNullOrEmpty(item.Key.IconCls))
                {
                    menuIcon = item.Key.IconCls;
                }

                var menuItemCount = 0;
                var menuItemHtml = "";

                foreach (var subItem in item.Value.OrderBy(x => x.Order))
                {                    
                    if (subItem.IsVisible == false)
                    {
                        continue;
                    }

                    if(string.IsNullOrEmpty(subItem.Url) == true)
                    {
                        subItem.Url = "/" + subItem.Controller + "/" + subItem.Action;
                    }
                    
                    var icon = "fa-arrow-right";
                    if (!string.IsNullOrEmpty(subItem.IconCls))
                    {
                        icon = subItem.IconCls;
                    }

                    var qStr = "";
                    if (!string.IsNullOrEmpty(subItem.QueryString))
                    {
                        qStr = "/?" + subItem.QueryString;
                    }

                    if(subItem.HasAllowAnonymous || HasUserPermission(user, subItem) )
                    {
                        menuItemHtml += "<li><a href=\"" + subItem.Url + qStr + "\" ><i class=\"fa " + icon + " fa-fw\"></i> " + subItem.Name + "</a></li>";
                        menuItemCount++;
                    }
                }
                               
                if (menuItemCount > 0)
                {
                    menuStr += "<li><a href=\"#\"><i class=\"fa " + menuIcon + " fa-fw\"></i> " + item.Key.Name + " <span class=\"fa arrow\"></span></a>"
                            + "<ul class=\"nav nav-second-level\">";
                    menuStr += menuItemHtml;
                    menuStr += "</ul></li>";
                }
            }
            
            return menuStr;
        }

        private static bool HasUserPermission(NccUser user, AdminMenuItem subItem)
        {
            if(user.Roles.Where(x=>x.Role.Name == NccCmsRoles.SuperAdmin).Count() > 0)
            {
                return true;
            }

            var controller = "";
            var action = "";

            if(string.IsNullOrEmpty(subItem.Url) == false)
            {
                (controller, action) = NccUrlHelper.GetControllerActionFromUrl(subItem.Url);
            }
            else
            {
                controller = subItem.Controller;
                action = subItem.Action;
            }

            if (user.ExtraDenies.Where(x => x.Action == action && x.Controller == controller).Count() > 0) {
                return false;
            }
            else if(user.Permissions.Where(x => x.Permission.PermissionDetails.Where(y=>y.Action == action && y.Controller == controller).Count() > 0).Count() > 0)
            {
                return true;
            }
            else if(user.ExtraPermissions.Where(x => x.Action == action && x.Controller == controller).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}