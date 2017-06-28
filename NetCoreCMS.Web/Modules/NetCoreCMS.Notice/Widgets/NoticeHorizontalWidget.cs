using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Notice.Controllers;
using NetCoreCMS.Notice.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Notice.Widgets
{
    public class NoticeHorizontalWidget : IWidget
    {
        string _title;
        string _body;
        string _footer;
        string _configJson;
        string _description;
        string _viewFileName;
        IViewRenderService _viewRenderService;
        NccNoticeService _noticeService;

        public string WidgetId { get { return "NetCoreCMS.Notice.Widgets.NoticeHorizontalWidget"; } }

        public string Title { get { return _title; } }

        public string Description { get { return _description; } }

        public string Body { get { return _body; } }

        public string Footer { get { return _footer; } }

        public string ConfigJson { get { return _configJson; } }

        public string ViewFileName { get { return _viewFileName; } }

        public NoticeHorizontalWidget(IViewRenderService viewRenderService, NccNoticeService noticeService)
        {
            _viewRenderService = viewRenderService;
            _noticeService = noticeService;
        }

        public void Init()
        {
            _title = "Notice Horizontal Widget";
            _description = "This is a sample widget which will scroll notices.";
            _footer = "Footer";
            _viewFileName = "Widgets/NoticeHorizontal";
        }

        public string RenderBody()
        {
            var notices = _noticeService.LoadTopNoticesForWebSite(10);
            _body = _viewRenderService.RenderToStringAsync<NoticeWidgetController>(_viewFileName, notices).Result;
            return _body;
        }

        public string RenderBody(string html = "")
        {
            if (!string.IsNullOrEmpty(html))
            {
                html.Replace(Widget.BODY_REPLACE_TEXT, _body);
            }
            else
            {
                html = _body;
            }
            
            return html;
        }

        public string RenderFooter(string html = "")
        {
            if (!string.IsNullOrEmpty(html))
            {
                html.Replace(Widget.FOOTER_REPLACE_TEXT, _body);
            }
            else
            {
                html = _footer;
            }

            return html;
        }

        public string RenderTitle(string html = "")
        {
            if (!string.IsNullOrEmpty(html))
            {
                html.Replace(Widget.TITLE_REPLACE_TEXT, _body);
            }
            else
            {
                html = _title;
            }

            return html;
        }

        public string RenderConfig()
        {
            throw new NotImplementedException();
        }
    }
}
