using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.VisualBasic;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NetCoreCMS.Framework.i18n
{
    public class NccStringLocalizer<ResourceT> : StringLocalizer<ResourceT>
    {
        private NccTranslator<ResourceT> _nccTranslator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizerFactory _factory;

        public NccStringLocalizer(IStringLocalizerFactory factory, IHttpContextAccessor httpContextAccessor) : base(factory)
        {
            _factory = factory;
            _httpContextAccessor = httpContextAccessor;
            CreateTranslator();
        }

        private void CreateTranslator()
        {
            var changedLanguage = GetCurrentLanguage();
             
            if (string.IsNullOrEmpty(changedLanguage)) {
                changedLanguage = "en";
            }

            if (string.IsNullOrEmpty(CurrentLanguage))
            {
                CurrentLanguage = changedLanguage;
            }
            
            if (!CurrentLanguage.Equals(changedLanguage) || _nccTranslator == null)
            {
                _nccTranslator = new NccTranslator<ResourceT>(changedLanguage);
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

        private string GetCurrentLanguage()
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
