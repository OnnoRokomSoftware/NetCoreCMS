using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Mvc.Views.Components
{
    public class Language : ViewComponent
    {
        private IConfiguration _config;
        public Language(IConfiguration config)
        {
            _config = config;
        }

        public IViewComponentResult Invoke(dynamic viewBag)
        {
            var _currentLanguage = "";

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
             
            ViewBag.Language = _currentLanguage;
            return View();
        }
    }
}
