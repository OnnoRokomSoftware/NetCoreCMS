using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.Core.Mvc.Controllers
{
    public class NccController : Controller
    {
        protected ILogger _logger;
        protected string _currentLanguage;
        protected NccTranslator<SharedResource> _T;
        
        public NccController()
        {
            var language = SetupHelper.Language;            
            _T = new NccTranslator<SharedResource>(language);
        }

        public string CurrentLanguage
        {
            get
            {
                if(GlobalConfig.WebSite != null && GlobalConfig.WebSite.IsMultiLangual)
                {
                    if (RouteData.Values.ContainsKey("lang"))
                    {
                        _currentLanguage = RouteData.Values["lang"].ToString().ToLower();
                    }

                    if (string.IsNullOrEmpty(_currentLanguage))
                    {
                        var feature = HttpContext.Features.Get<IRequestCultureFeature>();
                        _currentLanguage = feature.RequestCulture.Culture.TwoLetterISOLanguageName;
                    }

                    if (string.IsNullOrEmpty(_currentLanguage))
                    {
                        _currentLanguage = SetupHelper.Language;
                    }
                }
                else
                {
                    _currentLanguage = SetupHelper.Language;
                }
                
                return _currentLanguage;
            }
        }
    }
}
