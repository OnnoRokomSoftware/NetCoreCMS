using NetCoreCMS.Framework.Core.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Themes.DefaultDark.Lib
{
    public class MenuHelper
    {
        public static string PrepareMenuHtml(string position)
        {
            var menus = GlobalConfig.Menus.Where( x => x.Position == position);
            var menuTxt = "";
            
            foreach (var item in menus)
            {
                menuTxt += "<div class=\"ncc-main-menu\">";
                menuTxt += PrepareMenu(item.MenuItems);
                menuTxt += "</div>";
            }

            return menuTxt;
        }

        public static string PrepareMenu(List<NccMenuItem> menuItem, string upperSubMenuCls = "nav navbar-nav", string menuItemCls= "")
        {
            var menuTxt = "<ul class=\"" + upperSubMenuCls + "\">";

            foreach (var item in menuItem)
            {
                var hasChildren = item.Childrens.Count > 0;
                if (hasChildren)
                {
                    var subMenuText = "<li class=\""+menuItemCls+"\">";
                    subMenuText += "<a href=\""+item.Url+"\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" > "+item.Name+"</a>";
                    subMenuText += PrepareMenu(item.Childrens, "dropdown-menu multi-level", "dropdown-submenu");
                    menuTxt += subMenuText + "</li>";                    
                }
                else
                {
                    menuTxt += ListItemHtml(item);
                }
            }

            menuTxt += "</ul>";
            return menuTxt;
        }

        private static string ListItemHtml(NccMenuItem item)
        {
            var url = "/";
            var urlPrefix = "";
            var data = "";

            if(item.MenuActionType == NccMenuItem.ActionType.BlogCategory)
            {
                urlPrefix = "/CmsBlog/Blog/Category/";
            }
            else if(item.MenuActionType == NccMenuItem.ActionType.BlogPost)
            {
                urlPrefix = "/CmsBlog/Blog/Posts/";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.Module)
            {
                urlPrefix = "/" + item.Module + "/" + item.Controller + "/" + item.Action + "/";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.Page)
            {
                urlPrefix = "";/*/CmsHome/CmsPage/View/*/
                return "<li><a href=\"" + item.Url  + "\" target=\"" + item.Target + "\">" + item.Name + "  </a></li>";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.Tag)
            {
                urlPrefix = "/CmsBlog/Tag/Index/";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.Tag)
            {
                urlPrefix = "/CmsBlog/Tag/Index/";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.Url)
            {
                urlPrefix = "";
            }

            if (!string.IsNullOrEmpty(item.Data))
            {
                data = "?" + item.Data;
            }

            url = urlPrefix + item.Url + data;
            var li = "<li><a href=\""+ url + "\" target=\""+item.Target+"\">" + item.Name + "  </a></li>";
            return li;
        }
    }
}
