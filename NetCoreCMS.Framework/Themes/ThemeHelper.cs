/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using Microsoft.AspNetCore.Http;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Framework.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace NetCoreCMS.Framework.Themes
{
    public static class ThemeHelper
    {
        private static List<NccResource> _nccResources = new List<NccResource>();
        public static Theme ActiveTheme { get; set; }
        public static NccWebSite WebSite { get; set; }
        public static string GetCurrentLanguage()
        {
            var languageDetector = new NccLanguageDetector(new HttpContextAccessor());
            var currentLanguage = languageDetector.GetCurrentLanguage();
            return currentLanguage;
        }

        public static void RegisterCss(string resourcePath, NccResource.IncludePosition position = NccResource.IncludePosition.Footer, string version = "", int order = 1000, bool minify = true)
        {
            RegisterNccResource(NccResource.ResourceType.CssFile, resourcePath, position, version, order, minify);            
        }

        public static void RegisterResource(string resourceLibName)
        {
            if(resourceLibName == NccResource.JQuery)
            {
                RegisterNccResource(NccResource.ResourceType.JsFile, "/lib/jquery/jquery.min.js", NccResource.IncludePosition.Header, "2.3.3", 1, false);
            }
            else if (resourceLibName == NccResource.Bootstrap)
            {
                RegisterNccResource(NccResource.ResourceType.CssFile, "/lib/bootstrap/css/bootstrap.min.css", NccResource.IncludePosition.Header, "2.3.3", 2, false);
                RegisterNccResource(NccResource.ResourceType.JsFile, "/lib/bootstrap/js/bootstrap.min.js", NccResource.IncludePosition.Header, "2.3.3", 4, false);
            }
            else if (resourceLibName == NccResource.BootstrapDateTimePicker)
            {
                RegisterNccResource(NccResource.ResourceType.CssFile, "/lib/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css", NccResource.IncludePosition.Header, "2.3.3", 2, false);
                RegisterNccResource(NccResource.ResourceType.JsFile, "/lib/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js", NccResource.IncludePosition.Header, "2.3.3", 3, false);
            }
            else if (resourceLibName == NccResource.DataTable)
            {
                
            }
        }
        

        public static void RegisterResource(NccResource resource)
        {
            _nccResources.Add(resource);
        }

        public static void RegisterResource(List<NccResource> resources)
        {
            _nccResources.AddRange(resources);
        }

        public static void RegisterJs(string resourcePath, NccResource.IncludePosition position = NccResource.IncludePosition.Footer, string version = "", int order = 1000, bool minify = true)
        {
            RegisterNccResource(NccResource.ResourceType.JsFile, resourcePath, position, version, order, minify);            
        }

        private static void RegisterNccResource(NccResource.ResourceType type, string resourcePath, NccResource.IncludePosition position = NccResource.IncludePosition.Footer, string version = "", int order = 1000, bool minify = true)
        {
            var nccResource = new NccResource()
            {
                FilePath = resourcePath,
                Order = order,
                Position = position,
                Type = type,
                UseMinify = minify,
                Version = version
            };

            if(_nccResources.Where(x=>x.FilePath.ToLower() == resourcePath.ToLower()).Count() == 0)
            {
                _nccResources.Add(nccResource);
            }
        }

        public static List<NccResource> GetAllResources(NccResource.ResourceType type, NccResource.IncludePosition position)
        {
            return _nccResources.Where(x=>x.Type == type && x.Position == position).OrderBy(x=>x.Order).ToList();
        }

        #region Website Informations
         
        public static string GetWebSiteName()
        {
            var ret = "";
            if (WebSite != null)
            {
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.Name;
                }
            }
            return ret;
        }

        public static string GetWebSiteTitle()
        {
            var ret = "";
            if (WebSite != null)
            { 
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.SiteTitle;
                } 
            }
            return ret;
        }

        public static string GetWebSiteTagline()
        {
            var ret = "";
            if (WebSite != null)
            { 
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.Tagline;
                } 
            }
            return ret;
        }

        public static string GetWebSiteFaviconUrl()
        {
            var ret = "";
            if (WebSite != null)
            { 
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.FaviconUrl;
                } 
            }
            return ret;
        }

        public static string GetWebSiteLogoUrl()
        {
            var ret = "";
            if (WebSite != null)
            {
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.SiteLogoUrl;
                } 
            }
            return ret;
        }

        public static string GetWebSiteCopyright()
        {
            var ret = "";
            if (WebSite != null)
            { 
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.Copyrights;
                } 
            }
            return ret;
        }

        public static string GetWebSitePrivacyPolicyUrl()
        {
            var ret = "";
            if (WebSite != null)
            { 
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.PrivacyPolicyUrl;
                } 
            }
            return ret;
        }

        public static string GetWebSiteTermsAndConditionsUrl()
        {
            var ret = "";
            if (WebSite != null)
            { 
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.TermsAndConditionsUrl;
                } 
            }
            return ret;
        }
        #endregion

        #region Menu
        public static List<NccMenu> GetMenus(string menuLocation, string language)
        {
            return GlobalConfig.Menus.Where(x => x.Position == menuLocation && (x.MenuLanguage == language || string.IsNullOrEmpty(x.MenuLanguage))).ToList();
        }

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
                //urlPrefix = "/Category/";
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
                //urlPrefix = "/" + item.Controller + "/" + item.Action + "/";
                url = item.Url;
                url = NccUrlHelper.AddLanguageToUrl(currentLanguage, url);
                return "<li><a href=\"" + url + "\" target=\"" + item.Target + "\">" + item.Name + "</a></li>";
            }
            else if (item.MenuActionType == NccMenuItem.ActionType.Page)
            {
                //urlPrefix = "";/*/CmsHome/CmsPage/View/*/
                //item.Url = item.Url.StartsWith("/") == true ? item.Url : "/" + item.Url;
                //item.Url = NccUrlHelper.AddLanguageToUrl(currentLanguage, item.Url);
                //return "<li><a href=\"" + item.Url + "\" target=\"" + item.Target + "\">" + item.Name + "  </a></li>";
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

        #endregion

        #region Widgets
        public static List<NccWebSiteWidget> GetWebsiteWidgets(string layout, string zone)
        {
            return GlobalConfig.WebSiteWidgets.Where(x => x.LayoutName == layout && x.Zone == zone).ToList();
        }

        public static List<Widget> GetWidgets(string widgetId)
        {
            return GlobalConfig.Widgets.Where(x => x.WidgetId == widgetId).ToList();
        }

        #endregion

        #region Common Helper
        public static string GetJquery()
        {
            return "<script src=\"/lib/jquery/dist/jquery.min.js\"></script>";
        }
        public static string GetBootstrap()
        {
            return "<link rel=\"stylesheet\" href=\"/lib/bootstrap/css/bootstrap.css\" />" +
                "<script src=\"/lib/bootstrap/js/bootstrap.min.js\"></script>";
        }
        #endregion

        public static class Sections
        {
            public static string StyleHeader { get { return "StyleHeader"; } }
            public static string StyleFooter { get { return "StyleFooter"; } }
            public static string ScriptHeader { get { return "ScriptHeader"; } }
            public static string ScriptFooter { get { return "ScriptFooter"; } }            
        }
    }
}
