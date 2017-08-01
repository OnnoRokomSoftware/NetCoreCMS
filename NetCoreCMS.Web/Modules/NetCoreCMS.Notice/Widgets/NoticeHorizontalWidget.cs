using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Notice.Controllers;
using NetCoreCMS.Notice.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Notice.Widgets
{
    public class NoticeHorizontalWidget : Widget
    { 
        NccNoticeService _noticeService;
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;

        public NoticeHorizontalWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService, 
            NccNoticeService noticeService) : base(
                "NetCoreCMS.Modules.Notice.Widgets.NoticeHorizontalWidget", 
                "Notice Horizontal Widget", 
                "This is a sample widget which will scroll notices.", 
                "Footer"
            )
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
            _noticeService = noticeService;
        }

        public override void Init(long websiteWidgetId)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/NoticeHorizontal";

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
            var notices = _noticeService.LoadTopNoticesForWebSite(10);
            var body = _viewRenderService.RenderToStringAsync<NoticeWidgetController>(ViewFileName, notices).Result;
            return body;
        }
    }
}
