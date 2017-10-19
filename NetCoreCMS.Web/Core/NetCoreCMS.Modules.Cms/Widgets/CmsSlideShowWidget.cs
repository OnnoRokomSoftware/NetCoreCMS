/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Core.Modules.Cms.Controllers;
using Newtonsoft.Json;
 
namespace NetCoreCMS.Core.Modules.Cms.Widgets
{
    public class CmsSlideShowWidget : Widget
    {
    
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;

        public CmsSlideShowWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService):base("NetCoreCms.Modules.Cms.CmsSlideShow", "Slide Show", "Image Slide Show", "")
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
        }

        public override void Init(long websiteWidgetId)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/CmsSlideShow";
            var webSiteWidget = _websiteWidgetService.Get(websiteWidgetId, true);
            if (webSiteWidget != null && !string.IsNullOrEmpty(webSiteWidget.WidgetConfigJson))
            {
                var configJson = webSiteWidget.WidgetConfigJson;
                var config = JsonConvert.DeserializeObject<dynamic>(configJson);
                DisplayTitle = config.title;
                Language = config.language;
                Footer = config.footer;
            }
        }

        public override string RenderBody()
        {
            var body = _viewRenderService.RenderToStringAsync<CmsWidgetController>(ViewFileName, null).Result;
            return body;
        }
    }
}
