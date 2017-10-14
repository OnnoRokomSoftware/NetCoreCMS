using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Setup;

namespace NetCoreCMS.Framework.Core.Mvc.Controllers
{
    public class NccController : Controller
    {
        protected ILogger _logger;        
        protected NccLanguageDetector _nccLanguageDetector;

        public NccController()
        {
            var language = SetupHelper.Language;            
        }

        public string CurrentLanguage
        {
            get
            {
                if(_nccLanguageDetector == null)
                {
                    _nccLanguageDetector = (NccLanguageDetector)HttpContext.RequestServices.GetService(typeof(NccLanguageDetector));
                }
                
                return _nccLanguageDetector.GetCurrentLanguage();
            }
        }
    }
}
