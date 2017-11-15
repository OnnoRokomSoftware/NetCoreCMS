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
    public class ArchiveWidget : Widget
    {
        NccPostService _nccPostService;
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;
        bool ShowPostCount = false;
        bool DisplayOrder = true;
        bool DisplayAsDropdown = false;

        public ArchiveWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService,
            NccPostService nccPostService) : base(
                "NetCoreCMS.Core.Modules.Blog.Widgets.Archive",
                "Archive",
                "This is a widget to display archive.",
                "",
                true
            )
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
            _nccPostService = nccPostService;
        }

        public override void Init(long websiteWidgetId)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/Archive";

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

                    temp = config.displayOrder;
                    DisplayOrder = (temp == "1") ? true : false;

                    temp = config.displayAsDropdown;
                    DisplayAsDropdown = (temp == "on") ? true : false;
                }
                catch (Exception) { }
            }

            ConfigViewFileName = "Widgets/ArchiveConfig";
            ConfigHtml = _viewRenderService.RenderToStringAsync<BlogController>(ConfigViewFileName, webSiteWidget).Result;
        }

        public override string RenderBody()
        {
            var itemList = _nccPostService.LoadAtchive(DisplayOrder);
            ArchiveViewModel item = new ArchiveViewModel();
            item.ShowPostCount = ShowPostCount;
            item.DisplayAsDropdown = DisplayAsDropdown;
            item.ItemList = itemList;
            var body = _viewRenderService.RenderToStringAsync<BlogController>(ViewFileName, item).Result;
            return body;
        }
    }
}