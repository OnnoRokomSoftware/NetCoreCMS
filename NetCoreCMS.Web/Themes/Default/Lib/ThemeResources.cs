using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Themes.Default.Lib
{
    public class ThemeResources
    {
        public static class MenuLocation
        {
            public static string Top { get { return "Top"; } }
            public static string Navigation { get { return "Navigation"; } }
            public static string LeftRight { get { return "LeftColumn"; } }
            public static string RightColumn { get { return "RightColumn"; } }
            public static string Footer { get { return "Footer"; } }
        }

        public static class Layouts
        {
            public static class SiteLayout
            {
                public static string Name { get { return "SiteLayout"; } }
                public static class Zone
                {
                    public static string TopBar { get { return "TopBar"; } }
                    public static string Featured { get { return "Featured"; } }
                    public static string LeftSidebar { get { return "LeftSidebar"; } }
                    public static string RightSidebar { get { return "RightSidebar"; } }
                    public static string Footer { get { return "Footer"; } }
                }
            }

            public static class HomeLayout
            {
                public static string Name { get { return "HomeLayout"; } }
                public static class Zone
                {
                    public static string TopBar { get { return "TopBar"; } }
                    public static string FullWidthSlider { get { return "FullWidthSlider"; } }                   
                    public static string Featured { get { return "Featured"; } }
                    public static string LeftSidebar { get { return "LeftSidebar"; } }
                    public static string RightSidebar { get { return "RightSidebar"; } }
                    public static string Footer { get { return "Footer"; } }
                } 
            }

            public static class FullWidthLayout
            {
                public static string Name { get { return "FullWidthLayout"; } }
                public static class Zone
                {
                    public static string TopBar { get { return "TopBar"; } }
                    public static string Featured { get { return "Featured"; } }
                    public static string Footer { get { return "Footer"; } }
                }
            }

            public static class LoginLayout
            {
                public static string Name { get { return "LoginLayout"; } }
                public static class Zone
                {
                    public static string TopBar { get { return "TopBar"; } }
                     
                    public static string LeftSidebar { get { return "LeftSidebar"; } }
                    public static string RightSidebar { get { return "RightSidebar"; } }
                    public static string Footer { get { return "Footer"; } }
                }
            }

        }
    }
}
