/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;
using System.Linq;

namespace NetCoreCMS.Framework.i18n
{
    public class NccLanguageDetector
    {  
        private readonly HttpContext _httpContext;
        
        public NccLanguageDetector(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext; 
        }

        public string GetCurrentLanguage()
        {
            if (GlobalConfig.WebSite != null && GlobalConfig.WebSite.IsMultiLangual)
            {
                var lang = (string) _httpContext.GetRouteValue("lang");
                if (string.IsNullOrEmpty(lang))
                {
                    lang = GetLanguageFromCookie();
                }

                if (string.IsNullOrEmpty(lang))
                {
                    lang = GlobalConfig.WebSite.Language;
                }

                if (string.IsNullOrEmpty(lang))
                {
                    lang = SetupHelper.Language;
                }

                return lang;
            }
            else
            {
                return SetupHelper.Language;
            }
        }

        private string GetLanguageFromCookie()
        {
            if (_httpContext.Request.Cookies.ContainsKey(CookieRequestCultureProvider.DefaultCookieName))
            {
                var cultures = (string) _httpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
                if (string.IsNullOrEmpty(cultures) == false)
                {
                    var parsedValue = CookieRequestCultureProvider.ParseCookieValue(cultures);
                    var culture = parsedValue.Cultures.FirstOrDefault();
                    if (culture != null)
                    {
                        return culture.Value;
                    }
                }
            }
            return string.Empty;
        }
    }
}
