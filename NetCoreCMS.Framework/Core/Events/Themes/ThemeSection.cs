namespace NetCoreCMS.Framework.Core.Events.Themes
{
    public class ThemeSection
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public object Model { get; set; }
        public string ViewFileName { get; set; }
        public string Language { get; set; }

        public static class Sections
        {
            public static string Head { get { return "Head"; } }
            public static string HeaderCss { get { return "HeaderCss"; } }
            public static string HeaderScripts { get { return "HeaderScripts"; } }
            public static string Header { get { return "Header"; } }
            public static string LoadingMask { get { return "LoadingMask"; } }
            public static string GlobalMessageContainer { get { return "GlobalMessageContainer"; } }
            public static string Navigation { get { return "Navigation"; } }
            public static string Featured { get { return "Featured"; } }
            public static string LeftColumn { get { return "LeftColumn"; } }
            public static string RightColumn { get { return "RightColumn"; } }
            public static string Body { get { return "Body"; } }
            public static string Footer { get { return "Footer"; } }
            public static string FooterCss { get { return "FooterCss"; } }
            public static string FooterScripts { get { return "FooterScripts"; } }
            public static string PartialView { get { return "PartialView"; } }
        }
    }
}
