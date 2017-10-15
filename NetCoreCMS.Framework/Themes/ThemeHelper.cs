using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Setup;
using System.Collections.Generic;
using System.Linq;


namespace NetCoreCMS.Framework.Themes
{
    public static class ThemeHelper
    {
        public static NccWebSite WebSite { get; set; }

        #region Website Informations
        public static NccWebSiteInfo GetWebSiteInfoForLanguae(List<NccWebSiteInfo> webSiteInfos, string currentLanguage)
        {
            var webInfo = WebSite.WebSiteInfos.Where(x => x.Language.ToLower() == currentLanguage.ToLower()).FirstOrDefault();
            return webInfo;
        }

        public static string GetWebSiteName(string currentLanguage)
        {
            var ret = "";
            if (WebSite != null)
            {
                if (WebSite.IsMultiLangual)
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, currentLanguage);
                    if (webInfo == null)
                    {
                        webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, WebSite.Language);
                    }
                    if (webInfo != null)
                    {
                        ret = webInfo.Name;
                    }
                }
                else
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, SetupHelper.Language);
                    if (webInfo != null)
                    {
                        ret = webInfo.Name;
                    }
                }
            }
            return ret;
        }

        public static string GetWebSiteTitle(string currentLanguage)
        {
            var ret = "";
            if (WebSite != null)
            {
                if (WebSite.IsMultiLangual)
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, currentLanguage);
                    if (webInfo == null)
                    {
                        webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, WebSite.Language);
                    }
                    if (webInfo != null)
                    {
                        ret = webInfo.SiteTitle;
                    }
                }
                else
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, SetupHelper.Language);
                    if (webInfo != null)
                    {
                        ret = webInfo.SiteTitle;
                    }
                }
            }
            return ret;
        }

        public static string GetWebSiteTagline(string currentLanguage)
        {
            var ret = "";
            if (WebSite != null)
            {
                if (WebSite.IsMultiLangual)
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, currentLanguage);
                    if (webInfo == null)
                    {
                        webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, WebSite.Language);
                    }
                    if (webInfo != null)
                    {
                        ret = webInfo.Tagline;
                    }
                }
                else
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, SetupHelper.Language);
                    if (webInfo != null)
                    {
                        ret = webInfo.Tagline;
                    }
                }
            }
            return ret;
        }

        public static string GetWebSiteFaviconUrl(string currentLanguage)
        {
            var ret = "";
            if (WebSite != null)
            {
                if (WebSite.IsMultiLangual)
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, currentLanguage);
                    if (webInfo == null)
                    {
                        webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, WebSite.Language);
                    }
                    if (webInfo != null)
                    {
                        ret = webInfo.FaviconUrl;
                    }
                }
                else
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, SetupHelper.Language);
                    if (webInfo != null)
                    {
                        ret = webInfo.FaviconUrl;
                    }
                }
            }
            return ret;
        }

        public static string GetWebSiteLogoUrl(string currentLanguage)
        {
            var ret = "";
            if (WebSite != null)
            {
                if (WebSite.IsMultiLangual)
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, currentLanguage);
                    //if (webInfo == null)
                    //{
                    //    webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, WebSite.Language);
                    //}
                    if (webInfo != null)
                    {
                        ret = webInfo.SiteLogoUrl;
                    }
                }
                else
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, SetupHelper.Language);
                    if (webInfo != null)
                    {
                        ret = webInfo.SiteLogoUrl;
                    }
                }
            }
            return ret;
        }

        public static string GetWebSiteCopyright(string currentLanguage)
        {
            var ret = "";
            if (WebSite != null)
            {
                if (WebSite.IsMultiLangual)
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, currentLanguage);
                    if (webInfo == null)
                    {
                        webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, WebSite.Language);
                    }
                    if (webInfo != null)
                    {
                        ret = webInfo.Copyrights;
                    }
                }
                else
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, SetupHelper.Language);
                    if (webInfo != null)
                    {
                        ret = webInfo.Copyrights;
                    }
                }
            }
            return ret;
        }

        public static string GetWebSitePrivacyPolicyUrl(string currentLanguage)
        {
            var ret = "";
            if (WebSite != null)
            {
                if (WebSite.IsMultiLangual)
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, currentLanguage);
                    if (webInfo == null)
                    {
                        webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, WebSite.Language);
                    }
                    if (webInfo != null)
                    {
                        ret = webInfo.PrivacyPolicyUrl;
                    }
                }
                else
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, SetupHelper.Language);
                    if (webInfo != null)
                    {
                        ret = webInfo.PrivacyPolicyUrl;
                    }
                }
            }
            return ret;
        }

        public static string GetWebSiteTermsAndConditionsUrl(string currentLanguage)
        {
            var ret = "";
            if (WebSite != null)
            {
                if (WebSite.IsMultiLangual)
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, currentLanguage);
                    if (webInfo == null)
                    {
                        webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, WebSite.Language);
                    }
                    if (webInfo != null)
                    {
                        ret = webInfo.TermsAndConditionsUrl;
                    }
                }
                else
                {
                    var webInfo = GetWebSiteInfoForLanguae(WebSite.WebSiteInfos, SetupHelper.Language);
                    if (webInfo != null)
                    {
                        ret = webInfo.TermsAndConditionsUrl;
                    }
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
    }
}
