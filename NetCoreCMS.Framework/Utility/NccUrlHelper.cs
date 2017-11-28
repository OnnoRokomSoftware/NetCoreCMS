/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.i18n;
using System;
using System.Web;
using System.Linq;

namespace NetCoreCMS.Framework.Utility
{
    /// <summary>
    /// Utility for generating language enabled URL
    /// </summary>
    public static class NccUrlHelper
    {
        /// <summary>
        /// Add language code into URL if that not present.
        /// </summary>
        /// <param name="controller">Takes controller instance for getting selected language</param>
        /// <param name="url">Regular URL</param>
        /// <returns>Language enabled URL</returns>
        public static string AddLanguageToUrl(this NccController controller, string url)
        {
            return CreateLanguageEnabledUrl(controller.CurrentLanguage, url);
        }

        /// <summary>
        /// Add given language into URL if language segment not present.
        /// </summary>
        /// <param name="currentLanguage">Current site language</param>
        /// <param name="URL">Regular URL</param>
        /// <returns>Language enabled URL</returns>
        public static string AddLanguageToUrl(string currentLanguage, string url)
        {
            return CreateLanguageEnabledUrl(currentLanguage, url);
        }

        private static string CreateLanguageEnabledUrl(string currentLanguage, string url)
        {
            if (GlobalContext.WebSite?.IsMultiLangual == true)
            {  
                var urlPrefix = "/";
                var urlSuffix = url;

                if (url.Contains("http://") || url.Contains("https://"))
                {
                    var uri = new Uri(url);
                    urlPrefix = uri.Scheme;
                    urlPrefix += "://";
                    urlPrefix += uri.Authority;                     
                    var fUrl = urlPrefix + "/" + currentLanguage  + uri.AbsolutePath;
                    if(string.IsNullOrEmpty(uri.Query) == false)
                    {
                        if (fUrl.EndsWith("/"))
                        {
                            fUrl += uri.Query;
                        }
                        else
                        {
                            fUrl += "/" + uri.Query;
                        }   
                    }
                    return fUrl;
                }

                if (!urlSuffix.StartsWith("/"))
                    urlSuffix = "/" + urlSuffix;

                if (IsStartedWithLang(urlSuffix))
                {
                    return urlSuffix;
                }
                else
                {
                    return urlPrefix + currentLanguage + urlSuffix;
                }
                
            }
            return url;
        }

        private static bool IsStartedWithLang(string urlSuffix)
        {
            foreach (var item in SupportedCultures.Cultures)
            {
                if(urlSuffix.StartsWith(item.TwoLetterISOLanguageName) || urlSuffix.StartsWith("/" + item.TwoLetterISOLanguageName))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// .Net encoder encode URL seperators also. This method replace seperator slash, equalto, colon and question mark again.
        /// </summary>
        /// <param name="url">Any URL</param>
        /// <returns>Simplified encoded URL</returns>
        public static string EncodeUrl(string url)
        {
            var finalUrl = HttpUtility.UrlEncode(url);
            
            finalUrl = finalUrl.Replace("%26", "&");
            finalUrl = finalUrl.Replace("%3f", "?");
            finalUrl = finalUrl.Replace("%3F", "?");
            finalUrl = finalUrl.Replace("%3d", "=");
            finalUrl = finalUrl.Replace("%3D", "=");
            finalUrl = finalUrl.Replace("%2f", "/");
            finalUrl = finalUrl.Replace("%2F", "/");
            finalUrl = finalUrl.Replace("%2d", "-");
            finalUrl = finalUrl.Replace("%2D", "-");            
            finalUrl = finalUrl.Replace("%23", "#");
            finalUrl = finalUrl.Replace("%3a", ":");            

            return finalUrl;
        }

        public static (string Controller, string Action) GetControllerActionFromUrl(string url)
        {
            string controller = "", action = "";
            if (string.IsNullOrEmpty(url) == false)
            {
                var parts = url.Split("/".ToArray(), StringSplitOptions.RemoveEmptyEntries);
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

            return (controller, action);
        }
    }
}
