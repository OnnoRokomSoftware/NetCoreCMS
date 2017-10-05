using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.i18n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace NetCoreCMS.Framework.Utility
{
    public static class NccUrlHelper
    {
        public static string AddLanguageToUrl(this NccController controller, string url)
        {
            return CreateLanguageEnabledUrl(controller.CurrentLanguage, url);
        }

        public static string AddLanguageToUrl(string currentLanguage, string url)
        {
            return CreateLanguageEnabledUrl(currentLanguage, url);
        }

        private static string CreateLanguageEnabledUrl(string currentLanguage, string url)
        {
            if (GlobalConfig.WebSite.IsMultiLangual == true)
            {  
                var urlPrefix = "/";
                var urlSuffix = url;

                if (url.Contains("http://") || url.Contains("https://"))
                {
                    var uri = new Uri(url);
                    urlPrefix = uri.Scheme;
                    urlPrefix += "://";
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

        public static string EncodeUrl(string startupUrl)
        {
            var finalUrl = "";
            var urlParts = startupUrl.Split("/".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in urlParts)
            {
                finalUrl += "/" + WebUtility.UrlEncode(item);
            }
            return finalUrl;
        }
    }
}
