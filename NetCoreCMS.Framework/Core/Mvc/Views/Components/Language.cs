/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreCMS.Framework.i18n;

namespace NetCoreCMS.Framework.Core.Mvc.Views.Components
{
    public class Language : ViewComponent
    {
        private IConfiguration _config;
        private NccLanguageDetector _languageDetector;
        public Language(IConfiguration config, NccLanguageDetector languageDetector)
        {
            _languageDetector = languageDetector;
            _config = config;
        }

        public IViewComponentResult Invoke(dynamic viewBag)
        {
            //var _currentLanguage = "";

            //if(GlobalConfig.WebSite != null && GlobalConfig.WebSite.IsMultiLangual)
            //{
            //    if (RouteData.Values.ContainsKey("lang"))
            //    {
            //        _currentLanguage = RouteData.Values["lang"].ToString().ToLower();
            //    }

            //    if (string.IsNullOrEmpty(_currentLanguage))
            //    {
            //        var feature = HttpContext.Features.Get<IRequestCultureFeature>();
            //        _currentLanguage = feature.RequestCulture.Culture.TwoLetterISOLanguageName;
            //    }

            //    if (string.IsNullOrEmpty(_currentLanguage))
            //    {
            //        _currentLanguage = SetupHelper.Language;
            //    }
            //}
            //else
            //{
            //    _currentLanguage = SetupHelper.Language;
            //}
            
            ViewBag.Language = _languageDetector.GetCurrentLanguage();
            return View();
        }
    }
}
