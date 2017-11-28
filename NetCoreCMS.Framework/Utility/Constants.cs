/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

namespace NetCoreCMS.Framework.Utility
{
    /// <summary>
    /// Common constants used at all of NetCoreCMS projects.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Default site layout for post or page.
        /// </summary>
        public static string SiteLayoutName { get; } = "_SiteLayout";
        /// <summary>
        /// Default admin layout.
        /// </summary>
        public static string AdminLayoutName { get; } = "_AdminLayout";
        /// <summary>
        /// Another simple layout for front end.
        /// </summary>
        public static string SimpleLayoutName { get; } = "_SimpleLayout";
        /// <summary>
        /// This name is used to find modules configuration.
        /// </summary>
        public static string ModuleConfigFileName { get; } = "Module.json";
        /// <summary>
        /// This name is used to find theme configuration.
        /// </summary>
        public static string ThemeConfigFileName { get; } = "Theme.json";
        /// <summary>
        /// CMS home url
        /// </summary>
        public static string SiteUrl { get;} = "CmsHome";
        /// <summary>
        /// CMS admin url
        /// </summary>
        public static string AdminUrl { get;} = "Admin";
        /// <summary>
        /// When no resource founded at requested path then request redirected into this path.
        /// </summary>
        public static string NotFoundUrl { get; } = "/CmsHome/ResourceNotFound";
        /// <summary>
        /// Used for cryptography
        /// </summary>
        public static string NccSiteKey { get { return "E546C8DF278CD5931069B522E695D4F2"; } }

        /// <summary>
        /// Modules dependend library location. Depelopers have to copy their dependend library or nuget packages into this folder containing inside module folder.
        /// </summary>
        public static string ModuleDepencencyFolder { get { return "Dependency"; } } 

        /// <summary>
        /// Email server config saving settings key
        /// </summary>
        public static string SMTPSettingsKey = "NetCoreCMS_SMTP_Settings";

    }
}
