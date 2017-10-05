using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.LinkShare.Controllers;
using NetCoreCMS.LinkShare.Models;
using NetCoreCMS.LinkShare.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.LinkShare.Widgets
{
    public class LinkShareWidget : Widget
    {
        LsLinkService _lsLinkService;
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;
        string headerTitle = "";
        string category = "";
        string columnClass = "";
        string columnColor = "";
        string columnBgColor = "";
        string footerTitle = "";

        public LinkShareWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService,
            LsLinkService lsLinkService) : base(
                "NetCoreCMS.Modules.Widgets.LinkShare",
                "Link Share Widget",
                "This is a widget to display links.",
                "",
                false
            )
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
            _lsLinkService = lsLinkService;
        }

        public override void Init(long websiteWidgetId)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/LinkShare";

            var webSiteWidget = _websiteWidgetService.Get(websiteWidgetId, true);
            if (webSiteWidget != null && !string.IsNullOrEmpty(webSiteWidget.WidgetConfigJson))
            {
                var configJson = webSiteWidget.WidgetConfigJson;
                var config = JsonConvert.DeserializeObject<dynamic>(configJson);
                Language = config.language;
                DisplayTitle = "";
                Footer = "";
                headerTitle = config.headerTitle;
                category = config.category;
                columnClass = config.columnClass;
                columnColor = config.columnColor;
                columnBgColor = config.columnBgColor;
                footerTitle = config.footerTitle;
            }

            ConfigViewFileName = "Widgets/LinkShareConfig";
            ConfigHtml = _viewRenderService.RenderToStringAsync<LinkShareWidgetController>(ConfigViewFileName, webSiteWidget).Result;
        }

        public override string RenderBody()
        {
            var itemList = _lsLinkService.LoadAllByCategory(category);
            if (category.Trim() == "") { itemList = _lsLinkService.LoadAll().OrderByDescending(x => x.Id).Take(10).OrderBy(x => x.Order).ToList(); }
            var item = new LsLinkViewModel()
            {
                HeaderTitle = headerTitle,
                ColumnClass = columnClass.Trim() == "" ? "col-md-12" : columnClass,
                ColumnColor = columnColor,
                ColumnBgColor = columnBgColor,
                FooterTitle = footerTitle,
                LsLinkList = itemList
            };
            var body = _viewRenderService.RenderToStringAsync<LinkShareWidgetController>(ViewFileName, item).Result;
            return body;
        }
    }
}