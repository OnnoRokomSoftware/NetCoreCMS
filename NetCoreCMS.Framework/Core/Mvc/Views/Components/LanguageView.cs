using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using NetCoreCMS.Framework.i18n;

namespace NetCoreCMS.Framework.Core.Mvc.Views.Components
{
    public abstract class LanguageView : RazorPage<object>
    {
        private NccLanguageDetector _nccLanguageDetector;
        public LanguageView(NccLanguageDetector nccLanguageDetector)
        {
            _nccLanguageDetector = nccLanguageDetector;
        }
        public string GetLanguage()
        {
            //var _currentLanguage = "";

            //if(GlobalConfig.WebSite != null && GlobalConfig.WebSite.IsMultiLangual)
            //{
            //    if (Context.Request.Query.ContainsKey("lang"))
            //    {
            //        _currentLanguage = Context.Request.Query["lang"];
            //    }

            //    if (string.IsNullOrEmpty(_currentLanguage))
            //    {
            //        var feature = Context.Features.Get<IRequestCultureFeature>();
            //        _currentLanguage = feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
            //    }

            //    if (string.IsNullOrEmpty(_currentLanguage))
            //    {
            //        _currentLanguage = SetupHelper.Language;
            //    }

            //    if (string.IsNullOrEmpty(_currentLanguage))
            //    {
            //        _currentLanguage = "en";
            //    }
            //}
            //else
            //{
            //    _currentLanguage = SetupHelper.Language;
            //}
            
            return _nccLanguageDetector.GetCurrentLanguage();
        }
    }
}
