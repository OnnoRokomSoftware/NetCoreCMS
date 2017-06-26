using NetCoreCMS.Framework.Core.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using System.Reflection;

namespace NetCoreCMS.Framework.Utility
{
    public class AdminMenuHelper
    {
        public static string ModulesAdminMenuHtml()
        {
            string menuStr = "";
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

                            if ( key == null )
                            {
                                adminMenuDic.Add(atrib, new List<AdminMenuItem>());
                                key = atrib;
                            }
                            var actions = controller.GetMethods();
                            foreach (var item in actions)
                            {
                                var menuItem = item.GetCustomAttribute<AdminMenuItem>();
                                if(menuItem != null)
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


            foreach (var item in adminMenuDic)
            {
                menuStr += "<li><a href=\"#\"><i class=\"fa fa-gears fa-fw\"></i> "+ item.Key.Name +" <span class=\"fa arrow\"></span></a>"
                            + "<ul class=\"nav nav-second-level\">";
                foreach (var subItem in item.Value.OrderBy(x=>x.Order))
                {
                    var qStr = "";
                    if (!string.IsNullOrEmpty(subItem.QueryString))
                    {
                        qStr = "/?" + subItem.QueryString;
                    }
                    menuStr += "<li><a href=\""+subItem.Url + qStr + "\" ><i class=\"fa fa-gear fa-fw\"></i>" + subItem.Name + "</a></li>";
                }
                menuStr += "</ul></li>";
            }

            return menuStr;
        }
    }
}
