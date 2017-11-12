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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Views;
using System.Security.Claims;

namespace NetCoreCMS.Framework.Core.Events.Themes
{
    public class ThemeSection
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public object Model { get; set; }
        public object ExtraData { get; set; }
        public string ViewFileName { get; set; }
        public string Language { get; set; }        
        public dynamic ViewBag { get; set; }
        public string Path { get; set; }
        public HttpContext HttpContext { get; set; }
        public ViewContext ViewContext { get; set; }
        public ClaimsPrincipal User { get; set; }
        public ITempDataDictionary TempData { get; set; }

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
