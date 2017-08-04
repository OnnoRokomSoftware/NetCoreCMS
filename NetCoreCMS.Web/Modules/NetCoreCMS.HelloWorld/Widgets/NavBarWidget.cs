using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.HelloWorld.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.HelloWorld.Widgets
{
    public class NavBarWidget : Widget
    {
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;

        public NavBarWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService):base("NetCoreCMS.Modules.Widgets.NavBarWidget", "NavBar", "NavBar Widget", "")
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
        }

        public override void Init(long websiteWidgetId)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/NavBar";
            var webSiteWidget = _websiteWidgetService.Get(websiteWidgetId);
            if (webSiteWidget != null && !string.IsNullOrEmpty(webSiteWidget.WidgetConfigJson))
            {
                var configJson = webSiteWidget.WidgetConfigJson;
                var config = JsonConvert.DeserializeObject<dynamic>(configJson);
                DisplayTitle = config.title;
                Footer = config.footer;
            }
        }

        public override string RenderBody()
        {
            var body = _viewRenderService.RenderToStringAsync<HelloWidgetController>(ViewFileName, null).Result;
            return body;
        }
    }
}
