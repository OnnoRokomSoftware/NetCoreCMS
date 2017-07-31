using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Modules.Cms.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.HelloWorld.Widgets
{
    public class CmsSlideShowWidget : Widget
    {
    
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;

        public CmsSlideShowWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService):base("NetCoreCms.Modules.Cms.CmsSlideShow", "Slide Show", "Image Slide Show", "Footer")
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
        }

        public override void Init(long websiteWidgetId)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/CmsSlideShow";
            var websiteWidget = _websiteWidgetService.Get(websiteWidgetId);            
        }

        public override string RenderBody()
        {
            var body = _viewRenderService.RenderToStringAsync<CmsWidgetController>(ViewFileName, null).Result;
            return body;
        }
    }
}
