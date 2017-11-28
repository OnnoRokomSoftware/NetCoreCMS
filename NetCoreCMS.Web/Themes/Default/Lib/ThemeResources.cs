/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using NetCoreCMS.Framework.Themes;

namespace NetCoreCMS.Themes.Default.Lib
{
    public class ThemeResources
    {
        public static string RegisterResources(string layoutName = "")
        {
            ThemeHelper.RegisterCss("/css/ncc-common.css", NccResource.IncludePosition.Header);
            ThemeHelper.RegisterCss("/lib/css-loader/dist/css-loader.css", NccResource.IncludePosition.Header);
            ThemeHelper.RegisterCss(string.Concat("/Themes/Default/css/", ThemeHelper.ActiveTheme.Settings["style"]), NccResource.IncludePosition.Header, ThemeHelper.ActiveTheme.Settings["version"].ToString());

            ThemeHelper.RegisterResource(NccResource.JQuery);
            ThemeHelper.RegisterResource(NccResource.Bootstrap);

            ThemeHelper.RegisterJs("/js/ncc-common.min.js", NccResource.IncludePosition.Header);            
            ThemeHelper.RegisterJs("/lib/notifyjs/notify.min.js", NccResource.IncludePosition.Header);
            
            return string.Empty;
        }

        public static class MenuLocation
        {
            public static string Top { get { return "Top"; } }
            public static string Navigation { get { return "Navigation"; } }
            public static string LeftSidebar { get { return "LeftSidebar"; } }
            public static string RightSidebar { get { return "RightSidebar"; } }
            public static string Footer { get { return "Footer"; } }
        }

        public static class Zones
        {
            public static string TopBar { get { return "TopBar"; } }
            public static string Featured { get { return "Featured"; } }
            public static string FullWidthSlider { get { return "FullWidthSlider"; } }
            public static string LeftSidebar { get { return "LeftSidebar"; } }
            public static string RightSidebar { get { return "RightSidebar"; } }
            public static string Footer { get { return "Footer"; } }
        }

        public static class Layouts
        {
            public static string SiteLayout { get { return "SiteLayout"; } }
            public static string HomeLayout { get { return "HomeLayout"; } }
            public static string FullWidthLayout { get { return "FullWidthLayout"; } }
            public static string LoginLayout { get { return "LoginLayout"; } } 
        }
    }
}
