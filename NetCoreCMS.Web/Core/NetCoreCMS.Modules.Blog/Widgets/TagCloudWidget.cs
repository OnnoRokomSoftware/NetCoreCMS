/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Core.Modules.Blog.Controllers;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Modules.Blog.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.News.Widgets
{
    public class TagCloudWidget : Widget
    {
        NccTagService _nccTagService;
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;
        bool ShowTagHasPost = false;
        bool ShowPostCount = false;

        public TagCloudWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService,
            NccTagService nccTagService) : base(
                "NetCoreCMS.Core.Modules.Blog.Widgets.TagCloud",
                "Tag Cloud",
                "This is a widget to display Tags Cloud.",
                "",
                true
            )
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
            _nccTagService = nccTagService;
        }

        public override void Init(long websiteWidgetId)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/TagCloud";

            var webSiteWidget = _websiteWidgetService.Get(websiteWidgetId, true);
            if (webSiteWidget != null && !string.IsNullOrEmpty(webSiteWidget.WidgetConfigJson))
            {
                var configJson = webSiteWidget.WidgetConfigJson;
                var config = JsonConvert.DeserializeObject<dynamic>(configJson);
                DisplayTitle = config.title;
                Footer = config.footer;
                Language = config.language;

                try
                {
                    string temp = config.showPostCount;
                    ShowPostCount = (temp == "on") ? true : false;

                    temp = config.showTagHasPost;
                    ShowTagHasPost = (temp == "on") ? true : false;
                }
                catch (Exception) { }
            }

            ConfigViewFileName = "Widgets/TagCloudConfig";
            ConfigHtml = _viewRenderService.RenderToStringAsync<BlogController>(ConfigViewFileName, webSiteWidget).Result;
        }

        public override string RenderBody()
        {
            var itemList = _nccTagService.LoadTagCloud();
            if (itemList != null && itemList.Count > 0)
            {
                TagCloudViewModel item = new TagCloudViewModel();
                item.ShowTagHasPost = ShowTagHasPost;
                item.ShowPostCount = ShowPostCount;
                item.ItemList = itemList;
                var body = _viewRenderService.RenderToStringAsync<BlogController>(ViewFileName, item).Result;
                return body;
            }

            return "";
        }
    }
}