/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace NetCoreCMS.Framework.i18n
{
    public class NccStringLocalizer<ResourceT> : StringLocalizer<ResourceT>
    {
        private NccTranslator _nccTranslator;        
        private NccLanguageDetector _nccLanguageDetector;

        public NccStringLocalizer(IStringLocalizerFactory factory, IHttpContextAccessor httpContextAccessor) : base(factory)
        {
            _nccLanguageDetector = new NccLanguageDetector(httpContextAccessor);
            CreateTranslator();
        }

        private void CreateTranslator()
        {
            var changedLanguage = _nccLanguageDetector.GetCurrentLanguage();
            
            if (string.IsNullOrEmpty(changedLanguage)) {
                changedLanguage = "en";
            }

            if (string.IsNullOrEmpty(CurrentLanguage))
            {
                CurrentLanguage = changedLanguage;
            }
            
            if (!CurrentLanguage.Equals(changedLanguage) || _nccTranslator == null)
            {
                _nccTranslator = new NccTranslator(changedLanguage);
                CurrentLanguage = changedLanguage;
            }
        }

        public override LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                CreateTranslator();
                var translatedText = string.Format(_nccTranslator[name], arguments);
                var tt = new LocalizedString(name, translatedText);
                return tt;
            }
        }

        public override LocalizedString this[string name]
        {
            get
            {
                CreateTranslator();
                var tt = _nccTranslator[name];
                if (string.IsNullOrEmpty(tt))
                {
                    tt = name;
                }
                var ls = new LocalizedString(name, tt);
                return ls;
            }
        }

        public string CurrentLanguage { get; set; }
        
    }
}
