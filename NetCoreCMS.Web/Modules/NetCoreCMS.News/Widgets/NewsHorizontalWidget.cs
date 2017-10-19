/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Modules.News.Controllers;
using NetCoreCMS.Modules.News.Models;
using NetCoreCMS.Modules.News.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.News.Widgets
{
    public class NewsHorizontalWidget : Widget
    {
        NeNewsService _neNewsService;
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;
        string headerTitle = "";
        string category = "";
        string columnClass = "";
        string columnColor = "";
        string columnBgColor = "";
        string footerTitle = "";

        public NewsHorizontalWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService,
            NeNewsService neNewsService) : base(
                "NetCoreCMS.Modules.Widgets.NewsHorizontal",
                "Horizontal News Widget",
                "This is a widget to scroll news horizontally.",
                "",
                false
            )
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
            _neNewsService = neNewsService;
        }

        public override void Init(long websiteWidgetId)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/NewsHorizontal";

            var webSiteWidget = _websiteWidgetService.Get(websiteWidgetId, true);
            if (webSiteWidget != null && !string.IsNullOrEmpty(webSiteWidget.WidgetConfigJson))
            {
                var configJson = webSiteWidget.WidgetConfigJson;
                var config = JsonConvert.DeserializeObject<dynamic>(configJson);
                DisplayTitle = "";
                Footer = "";
                Language = config.language;
                headerTitle = config.headerTitle;
                category = config.category;
                columnClass = config.columnClass;
                columnColor = config.columnColor;
                columnBgColor = config.columnBgColor;
                footerTitle = config.footerTitle;
            }

            ConfigViewFileName = "Widgets/NewsConfig";
            ConfigHtml = _viewRenderService.RenderToStringAsync<NewsWidgetController>(ConfigViewFileName, webSiteWidget).Result;
        }

        public override string RenderBody()
        {
            var itemList = _neNewsService.LoadAllByCategory(category);
            if (category.Trim() == "")
            {
                itemList = _neNewsService.LoadAll()
                    .Where(x => x.Status >= EntityStatus.Active && (x.HasDateRange == false || (x.PublishDate >= DateTime.Now && x.ExpireDate <= DateTime.Now)))
                    .OrderByDescending(x => x.Id)
                    .Take(10)
                    .OrderBy(x => x.Order)
                    .ToList();
            }
            var item = new NeNewsViewModel()
            {
                HeaderTitle = headerTitle,
                ColumnClass = columnClass.Trim() == "" ? "" : columnClass,
                ColumnColor = columnColor,
                ColumnBgColor = columnBgColor,
                FooterTitle = footerTitle,
                NeNewsList = itemList
            };
            var body = _viewRenderService.RenderToStringAsync<NewsWidgetController>(ViewFileName, item).Result;
            return body;
        }
    }
}