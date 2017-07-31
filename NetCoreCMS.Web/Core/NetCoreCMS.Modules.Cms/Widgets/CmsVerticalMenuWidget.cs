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
            ConfigViewFileName = "Widgets/CmsVerticalMenuConfig";
            var websiteWidget = _websiteWidgetService.Get(websiteWidgetId);
            ConfigHtml = _viewRenderService.RenderToStringAsync<CmsWidgetController>(ConfigViewFileName, websiteWidget).Result;
        }

        public override string RenderBody()
        {
            var body = _viewRenderService.RenderToStringAsync<CmsWidgetController>(ViewFileName, null).Result;
            return body;
        }
        
    }
}
