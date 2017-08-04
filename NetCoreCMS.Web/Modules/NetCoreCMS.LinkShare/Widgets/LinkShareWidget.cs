using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.LinkShare.Controllers;
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
        NccLinkShareService _imageSliderService;
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;
        string selectedImageSliderName;

        public LinkShareWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService,
            NccLinkShareService imageSliderService) : base(
                "NetCoreCMS.Modules.Widgets.LinkShare",
                "Link Share Widget",
                "This is a widget to display links.",
                ""
            )
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
            _imageSliderService = imageSliderService;
        }

        public override void Init(long websiteWidgetId)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/LinkShare";

            var webSiteWidget = _websiteWidgetService.Get(websiteWidgetId);
            if (webSiteWidget != null && !string.IsNullOrEmpty(webSiteWidget.WidgetConfigJson))
            {
                var configJson = webSiteWidget.WidgetConfigJson;
                var config = JsonConvert.DeserializeObject<dynamic>(configJson);
                DisplayTitle = config.title;
                Footer = config.footer;
                selectedImageSliderName = config.name;
            }

            ConfigViewFileName = "Widgets/LinkShareConfig";
            ConfigHtml = _viewRenderService.RenderToStringAsync<LinkShareWidgetController>(ConfigViewFileName, webSiteWidget).Result;
        }

        public override string RenderBody()
        {
            var itemList = _imageSliderService.LoadAllByName(selectedImageSliderName).FirstOrDefault();
            var body = _viewRenderService.RenderToStringAsync<LinkShareWidgetController>(ViewFileName, itemList).Result;
            return body;
        }
    }
}
