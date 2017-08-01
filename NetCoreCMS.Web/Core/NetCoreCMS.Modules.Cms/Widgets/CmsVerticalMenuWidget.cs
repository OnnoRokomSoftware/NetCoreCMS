using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Modules.Cms.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.HelloWorld.Widgets
{
    public class CmsVerticalMenuWidget : Widget
    {

        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;                

        public CmsVerticalMenuWidget(
            IViewRenderService viewRenderService, 
            NccWebSiteWidgetService websiteWidgetService):base("NetCoreCMS.Modules.Cms.CmsVerticalMenuWidget", "Vertical Menu", "Vertical nevigation menu", "Footer")
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
        }

        public override void Init(long websiteWidgetId)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/CmsVerticalMenu";
            var webSiteWidget = _websiteWidgetService.Get(websiteWidgetId);
            if (webSiteWidget != null && !string.IsNullOrEmpty(webSiteWidget.WidgetConfigJson))
            {
                var configJson = webSiteWidget.WidgetConfigJson;
                var config = JsonConvert.DeserializeObject<dynamic>(configJson);
                DisplayTitle = config.title;
                Footer = config.footer;
            }
            
            ConfigViewFileName = "Widgets/CmsVerticalMenuConfig";
            ConfigHtml = _viewRenderService.RenderToStringAsync<CmsWidgetController>(ConfigViewFileName, webSiteWidget).Result;
        }

        public override string RenderBody()
        {
            var body = _viewRenderService.RenderToStringAsync<CmsWidgetController>(ViewFileName, null).Result;
            return body;
        }
        
    }
}
