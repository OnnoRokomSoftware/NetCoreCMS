using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreCMS.Framework.i18n;

namespace NetCoreCMS.Framework.Core.Mvc.Views.Extensions
{
    public static class LanguageExtension
    {
        public static string CurrentLanguage(this IHtmlHelper helper)
        {
            var langDetector = new NccLanguageDetector(new HttpContextAccessor() { HttpContext = helper.ViewContext.HttpContext });
            return langDetector.GetCurrentLanguage();
        }
    }
}
