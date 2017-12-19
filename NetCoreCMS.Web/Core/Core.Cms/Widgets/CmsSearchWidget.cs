/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Core.Cms.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Serialization;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using Newtonsoft.Json;

namespace Core.Cms.Widgets
{
    public class CmsSearchWidget : Widget
    {
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;
        string body;
        public CmsSearchWidget(IViewRenderService viewRenderService, NccWebSiteWidgetService websiteWidgetService) : base("Search Widget", "Search form show", "")
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
        }

        public override void Init(long websiteWidgetId, bool renderConfig = false)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/CmsSearch";
            var webSiteWidget = _websiteWidgetService.Get(websiteWidgetId, true);
            if (webSiteWidget != null && !string.IsNullOrEmpty(webSiteWidget.WidgetConfigJson))
            {
                var configJson = webSiteWidget.WidgetConfigJson;
                var config = JsonHelper.Deserilize<dynamic>(configJson);
                DisplayTitle = config.title;
                Footer = config.footer;
                Language = config.language;
            }
        }

        public override string RenderBody()
        {
            var body = _viewRenderService.RenderToStringAsync<CmsHomeController>(ViewFileName, null).Result;
            return body;
        }
    }
}
