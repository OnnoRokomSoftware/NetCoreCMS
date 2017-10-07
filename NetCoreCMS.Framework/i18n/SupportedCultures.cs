using Microsoft.AspNetCore.Localization;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NetCoreCMS.Framework.i18n
{
    public class SupportedCultures
    {
        public static List<CultureInfo> Cultures { get; set; } = new List<CultureInfo>
        {
            new CultureInfo("en-us"),
            new CultureInfo("bn-bd"),
        };

        public static RequestCulture DefaultCulture { get { return new RequestCulture(new CultureInfo(SetupHelper.Language)); }}
        public static string LanguageSessionKey { get { return "ncc_lang_session"; } }
        public static string LanguageCookieKey { get { return "ncc_lang_cookie"; } }
        public static object SharedResourceTranslatorObjectKey { get { return "ncc_shared_resource_translator_object"; } }
        
    }
}
