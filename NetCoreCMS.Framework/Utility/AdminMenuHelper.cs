using NetCoreCMS.Framework.Core.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using System.Reflection;
using NetCoreCMS.Framework.Themes;

namespace NetCoreCMS.Framework.Utility
{
    public class AdminMenuHelper
    {
        public static Dictionary<AdminMenu, List<AdminMenuItem>> ModulesAdminMenus()
        {
            Dictionary<AdminMenu, List<AdminMenuItem>> adminMenuDic = new Dictionary<AdminMenu, List<AdminMenuItem>>();

            foreach (var module in GlobalConfig.Modules)
            {
                var controllers = module.Assembly.DefinedTypes.Select(t => t.AsType()).Where(x => typeof(NccController).IsAssignableFrom(x));
                foreach (var controller in controllers)
                {
                    try
                    {
                        var atrib = controller.GetTypeInfo().GetCustomAttribute<AdminMenu>();
                        if (atrib != null)
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
                                if (menuItem != null)
                                {
                                    adminMenuDic[key].Add(menuItem);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO: Log error. Raise global error message.
                    }
                }
            }
            return adminMenuDic;
        }

        public static Dictionary<SiteMenu, List<SiteMenuItem>> ModulesSiteMenus()
        {
            Dictionary<SiteMenu, List<SiteMenuItem>> siteMenuDic = new Dictionary<SiteMenu, List<SiteMenuItem>>();

            foreach (var module in GlobalConfig.Modules)
            {
                var controllers = module.Assembly.DefinedTypes.Select(t => t.AsType()).Where(x => typeof(NccController).IsAssignableFrom(x));
                foreach (var controller in controllers)
                {
                    try
                    {
                        var atrib = controller.GetTypeInfo().GetCustomAttribute<SiteMenu>();
                        if (atrib != null)
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
                                if (menuItem != null)
                                {
                                    siteMenuDic[key].Add(menuItem);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO: Log error. Raise global error message.
                    }
                }
            }

            return siteMenuDic;
        }

        public static string GetAdminMenuHtml(Dictionary<AdminMenu, List<AdminMenuItem>> adminMenuDic)
        {
            var menuStr = "";
            foreach (var item in adminMenuDic)
            {
                string icon = "fa-gears";
                if (!string.IsNullOrEmpty(item.Key.IconCls))
                {
                    icon = item.Key.IconCls;
                }
                menuStr += "<li><a href=\"#\"><i class=\"fa " + icon + " fa-fw\"></i> " + item.Key.Name + " <span class=\"fa arrow\"></span></a>"
                            + "<ul class=\"nav nav-second-level\">";
                foreach (var subItem in item.Value.OrderBy(x => x.Order))
                {
                    icon = "fa-gear";
                    if (!string.IsNullOrEmpty(subItem.IconCls))
                    {
                        icon = subItem.IconCls;
                    }
                    var qStr = "";
                    if (!string.IsNullOrEmpty(subItem.QueryString))
                    {
                        qStr = "/?" + subItem.QueryString;
                    }
                    menuStr += "<li><a href=\"" + subItem.Url + qStr + "\" ><i class=\"fa " + icon + " fa-fw\"></i> " + subItem.Name + "</a></li>";
                }
                menuStr += "</ul></li>";
            }

            return menuStr;
        }

    }
}