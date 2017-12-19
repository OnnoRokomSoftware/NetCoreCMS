/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Core.Blog.Controllers;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using Core.Blog.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetCoreCMS.Framework.Core.Serialization;

namespace Core.Blog.Widgets
{
    public class RecentPostWidget : Widget
    {
        NccPostService _nccPostService;
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;
        int PostCount = 5;
        bool IsDateShow = false;

        public RecentPostWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService,
            NccPostService nccPostService) : base(                
                "Recent Post",
                "This is a widget to display recent blog posts.",
                "",
                true
            )
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
            _nccPostService = nccPostService;
        }

        public override void Init(long websiteWidgetId, bool renderConfig = false)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/RecentPost";

            var webSiteWidget = _websiteWidgetService.Get(websiteWidgetId, true);
            if (webSiteWidget != null && !string.IsNullOrEmpty(webSiteWidget.WidgetConfigJson))
            {
                var configJson = webSiteWidget.WidgetConfigJson;
                var config = JsonHelper.Deserilize<dynamic>(configJson);
                DisplayTitle = config.title;
                Footer = config.footer;
                Language = config.language;

                try
                {
                    string pc = config.postCount;
                    PostCount = string.IsNullOrEmpty(pc) ? 5 : Convert.ToInt32(pc);
                    string ds = config.isDateShow;
                    if (ds == "on")
                        IsDateShow = true;
                    else
                        IsDateShow = false;
                }
                catch (Exception) { PostCount = 5; }

            }
            if (renderConfig)
            {
                ConfigViewFileName = "Widgets/RecentPostConfig";
                ConfigHtml = _viewRenderService.RenderToStringAsync<BlogController>(ConfigViewFileName, webSiteWidget).Result;
            } 
        }

        public override string RenderBody()
        {
            var postList = _nccPostService.LoadRecentPages(PostCount);
            RecentPostViewModel item = new RecentPostViewModel();
            item.IsDateShow = IsDateShow;
            item.PostList = postList;
            var body = _viewRenderService.RenderToStringAsync<BlogController>(ViewFileName, item).Result;
            return body;
        }
    }
}