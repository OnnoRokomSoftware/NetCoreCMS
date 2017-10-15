using Microsoft.AspNetCore.Http;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.i18n;
using System.Collections.Generic;
using System.Linq;


namespace NetCoreCMS.Framework.Themes
{
    public static class ThemeHelper
    {
        public static Theme ActiveTheme { get; set; }
        public static NccWebSite WebSite { get; set; }
        public static string GetCurrentLanguage()
        {
            var languageDetector = new NccLanguageDetector(new HttpContextAccessor());
            var currentLanguage = languageDetector.GetCurrentLanguage();
            return currentLanguage;
        }

        #region Website Informations
         
        public static string GetWebSiteName()
        {
            var ret = "";
            if (WebSite != null)
            {
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.Name;
                }
            }
            return ret;
        }

        public static string GetWebSiteTitle()
        {
            var ret = "";
            if (WebSite != null)
            { 
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.SiteTitle;
                } 
            }
            return ret;
        }

        public static string GetWebSiteTagline()
        {
            var ret = "";
            if (WebSite != null)
            { 
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.Tagline;
                } 
            }
            return ret;
        }

        public static string GetWebSiteFaviconUrl()
        {
            var ret = "";
            if (WebSite != null)
            { 
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.FaviconUrl;
                } 
            }
            return ret;
        }

        public static string GetWebSiteLogoUrl()
        {
            var ret = "";
            if (WebSite != null)
            {
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.SiteLogoUrl;
                } 
            }
            return ret;
        }

        public static string GetWebSiteCopyright()
        {
            var ret = "";
            if (WebSite != null)
            { 
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.Copyrights;
                } 
            }
            return ret;
        }

        public static string GetWebSitePrivacyPolicyUrl()
        {
            var ret = "";
            if (WebSite != null)
            { 
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.PrivacyPolicyUrl;
                } 
            }
            return ret;
        }

        public static string GetWebSiteTermsAndConditionsUrl()
        {
            var ret = "";
            if (WebSite != null)
            { 
                var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == GetCurrentLanguage()).FirstOrDefault();
                if (webInfo != null)
                {
                    ret = webInfo.TermsAndConditionsUrl;
                } 
            }
            return ret;
        }
        #endregion 

        #region Common Helper
        public static string GetJquery()
        {
            return "<script src=\"/lib/jquery/dist/jquery.js\"></script>";
        }
        public static string GetBootstrap()
        {
            return "<link rel=\"stylesheet\" href=\"/lib/bootstrap/dist/css/bootstrap.css\" />" +
                "<script src=\"/lib/bootstrap/dist/js/bootstrap.js\"></script>";
        }
        #endregion

        public static class Sections
        {
            public static string StyleHeader { get { return "StyleHeader"; } }
            public static string StyleFooter { get { return "StyleFooter"; } }
            public static string ScriptHeader { get { return "ScriptHeader"; } }
            public static string ScriptFooter { get { return "ScriptFooter"; } }            
        }
    }
}
