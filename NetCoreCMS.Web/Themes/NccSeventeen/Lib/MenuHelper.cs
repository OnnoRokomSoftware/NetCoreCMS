using NetCoreCMS.Framework.Core.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NetCoreCMS.Framework.Utility;
using System.Text.RegularExpressions;

namespace NetCoreCMS.Themes.NccSeventeen.Lib
{
    public class MenuHelper
    {
        public static string PrepareMenuHtml(string position)
        {
            var menus = GlobalConfig.Menus.Where(x => x.Position == position).OrderBy(x => x.MenuOrder).ToList();
            var menuTxt = "";

            foreach (var item in menus)
            {
                menuTxt += "<div class=\"ncc-main-menu\">";
                menuTxt += PrepareMenu(item.MenuItems, "");
                menuTxt += "</div>";
            }

            return menuTxt;
        }

        public static string PrepareMenuHtml(string position, string currentLanguage)
        {
            var menus = GlobalConfig.Menus.Where(x => x.Position == position && (string.IsNullOrEmpty(x.MenuLanguage) || x.MenuLanguage.ToLower() == currentLanguage.ToLower())).OrderBy(x => x.MenuOrder).ToList();
            var menuTxt = "";

            foreach (var item in menus)
            {
                menuTxt += "<div class=\"ncc-main-menu\">";
                menuTxt += PrepareMenu(item.MenuItems, currentLanguage);
                menuTxt += "</div>";
            }

            return menuTxt;
        }

        public static string PrepareMenu(List<NccMenuItem> menuItem, string currentLanguage, string upperSubMenuCls = "nav navbar-nav", string menuItemCls = "")
        {
            var menuTxt = "<ul class=\"" + upperSubMenuCls + "\">";

            menuItem = menuItem.OrderBy(m => m.MenuOrder).ToList();
            foreach (var item in menuItem)
            {
                var hasChildren = item.Childrens.Count > 0;
                if (hasChildren)
                {

                    var subMenuText = "<li class=\"" + menuItemCls + "\">";

                    if (!string.IsNullOrEmpty(currentLanguage) && GlobalConfig.WebSite.IsMultiLangual && !IsExternalUrl(item.Url))
                        subMenuText += "<a href=\"/" + currentLanguage + item.Url + "\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" > " + item.Name + "</a>";
                    else
                        subMenuText += "<a href=\"" + item.Url + "\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" > " + item.Name + "</a>";

                    subMenuText += PrepareMenu(item.Childrens, currentLanguage, "dropdown-menu multi-level", "dropdown-submenu");
                    menuTxt += subMenuText + "</li>";
                }
                else
                {
                    menuTxt += ListItemHtml(item, currentLanguage);
                }
            }

            menuTxt += "</ul>";
            return menuTxt;
        }

        private static string ListItemHtml(NccMenuItem item, string currentLanguage)
        {
            var url = "/";
            var urlPrefix = "";
            var data = "";

            if (item.MenuActionType == NccMenuItem.ActionType.BlogCategory)
            {
                url = item.Url;
                url = NccUrlHelper.AddLanguageToUrl(currentLanguage, url);
                return "<li><a href=\"" + url + "\" target=\"" + item.Target + "\">" + item.Name + "</a></li>";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.BlogPost)
            {
                url = item.Url;
                url = NccUrlHelper.AddLanguageToUrl(currentLanguage, url);
                return "<li><a href=\"" + url + "\" target=\"" + item.Target + "\">" + item.Name + "</a></li>";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.Module)
            {
                url = item.Url;
                url = NccUrlHelper.AddLanguageToUrl(currentLanguage, url);
                return "<li><a href=\"" + url + "\" target=\"" + item.Target + "\">" + item.Name + "</a></li>";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.Page)
            {
                url = item.Url;
                url = NccUrlHelper.AddLanguageToUrl(currentLanguage, url);
                return "<li><a href=\"" + url + "\" target=\"" + item.Target + "\">" + item.Name + "</a></li>";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.Tag)
            {
                url = item.Url;
                url = NccUrlHelper.AddLanguageToUrl(currentLanguage, url);
                return "<li><a href=\"" + url + "\" target=\"" + item.Target + "\">" + item.Name + "</a></li>";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.Url)
            {
                urlPrefix = "";
            }

            if (!string.IsNullOrEmpty(item.Data))
            {
                data = "?slug=" + item.Data;
            }

            url = urlPrefix + item.Url + data;
            if (!string.IsNullOrEmpty(currentLanguage) && GlobalConfig.WebSite.IsMultiLangual && !IsExternalUrl(url))
            {
                url = "/" + currentLanguage + url;
            }

            var li = "<li><a href=\"" + url + "\" target=\"" + item.Target + "\">" + item.Name + "  </a></li>";
            return li;
        }

        private static bool IsExternalUrl(string url)
        {
            string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(url);
        }
    }
}
