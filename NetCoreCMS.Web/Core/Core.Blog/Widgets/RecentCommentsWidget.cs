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
    public class RecentCommentsWidget : Widget
    {
        NccCommentsService _nccCommentsService;
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;
        int CommentsCount = 5;

        public RecentCommentsWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService,
            NccCommentsService nccCommentsService) : base(                
                "Recent Comments",
                "This is a widget to display recent blog Comments.",
                "",
                true
            )
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
            _nccCommentsService = nccCommentsService;
        }

        public override void Init(long websiteWidgetId, bool renderConfig = false)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/RecentComments";

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
                    string cc = config.commentsCount;
                    CommentsCount = string.IsNullOrEmpty(cc) ? 5 : Convert.ToInt32(cc);
                }
                catch (Exception) { CommentsCount = 5; }

            }

            if (renderConfig)
            {
                ConfigViewFileName = "Widgets/RecentCommentsConfig";
                ConfigHtml = _viewRenderService.RenderToStringAsync<BlogController>(ConfigViewFileName, webSiteWidget).Result;
            } 
        }

        public override string RenderBody()
        {
            List<NccComment> commentsList = _nccCommentsService.LoadRecentComments(CommentsCount);
            var body = _viewRenderService.RenderToStringAsync<BlogController>(ViewFileName, commentsList).Result;
            return body;
        }
    }
}