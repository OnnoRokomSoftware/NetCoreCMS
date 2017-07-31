using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.HelloWorld.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.HelloWorld.Widgets
{
    public class FooterMenuWidget : Widget
    {
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;

        public FooterMenuWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService):base("NetCoreCMS.HelloWorld.Widgets.FooterMenuWidget", "Footer Menu", "Footer nevigation menu", "Footer")
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
        }

        public override void Init(long websiteWidgetId)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/FooterMenu";            
            var websiteWidget = _websiteWidgetService.Get(websiteWidgetId);            
        }

        public override string RenderBody()
        {
            var body = _viewRenderService.RenderToStringAsync<HelloWidgetController>(ViewFileName, null).Result;
            return body;
        }
    }
}
