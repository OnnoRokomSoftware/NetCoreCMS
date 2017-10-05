namespace NetCoreCMS.Framework.Utility
{
    public static class Constants
    {
        public static string SiteLayoutName { get; } = "_SiteLayout";
        public static string AdminLayoutName { get; } = "_AdminLayout";
        public static string SimpleLayoutName { get; } = "_SimpleLayout";
        
        public static string ModuleConfigFileName { get; } = "Module.json";
        public static string ThemeConfigFileName { get; } = "Theme.json";

        public static string SiteUrl { get;} = "CmsHome";
        public static string AdminUrl { get;} = "Admin";
        public static string NotFoundUrl { get; } = "/CmsHome/ResourceNotFound";
        public static string NccSiteKey { get { return "E546C8DF278CD5931069B522E695D4F2"; } }
        public static string SMTPSettingsKey = "NetCoreCMS_SMTP_Settings";

    }
}
