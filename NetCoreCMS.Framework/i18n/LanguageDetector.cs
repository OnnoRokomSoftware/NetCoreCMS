using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.i18n
{
    public class LanguageDetector
    {  
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizerFactory _factory;

        public LanguageDetector(IStringLocalizerFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            _factory = factory;
            _httpContextAccessor = httpContextAccessor;            
        }

        public string GetCurrentLanguage()
        {
            if (GlobalConfig.WebSite != null && GlobalConfig.WebSite.IsMultiLangual)
            {
                var lang = _httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
                if (string.IsNullOrEmpty(lang))
                {
                    var feature = _httpContextAccessor.HttpContext?.Features?.Get<IRequestCultureFeature>();
                    lang = feature?.RequestCulture.Culture.TwoLetterISOLanguageName;
                    if (string.IsNullOrEmpty(lang))
                    {
                        lang = SetupHelper.Language;
                    }
                }
                return lang;
            }
            else
            {
                return SetupHelper.Language;
            }
        }
    }
}
