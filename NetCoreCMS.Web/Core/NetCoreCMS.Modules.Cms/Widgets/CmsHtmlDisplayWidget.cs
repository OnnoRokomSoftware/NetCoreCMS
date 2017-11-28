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
    public class CmsHtmlDisplayWidget : Widget
    {
    
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;
        string body;
        public CmsHtmlDisplayWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService):base("NetCoreCms.Modules.Cms.CmsHtmlDisplay", "Html Display", "Html or text Display Show", "")
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
        }

        public override void Init(long websiteWidgetId)
        {
            WebSiteWidgetId = websiteWidgetId;
            //ViewFileName = "Widgets/CmsVerticalMenu";
            var webSiteWidget = _websiteWidgetService.Get(websiteWidgetId, true);
            if (webSiteWidget != null && !string.IsNullOrEmpty(webSiteWidget.WidgetConfigJson))
            {
                var configJson = webSiteWidget.WidgetConfigJson;
                var config = JsonConvert.DeserializeObject<dynamic>(configJson);
                DisplayTitle = config.title;
                Footer = config.footer;
                body = config.bodyContent;
                Language = config.language;
            }

            ConfigViewFileName = "Widgets/CmsHtmlDisplayConfig";
            ConfigHtml = _viewRenderService.RenderToStringAsync<CmsHomeController>(ConfigViewFileName, webSiteWidget).Result;
        }

        public override string RenderBody()
        {
            return body;
        }
    }
}
