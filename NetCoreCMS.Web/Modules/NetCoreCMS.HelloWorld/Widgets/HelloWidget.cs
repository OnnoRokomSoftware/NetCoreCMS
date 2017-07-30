using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.HelloWorld.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.HelloWorld.Widgets
{
    public class HelloWidget : IWidget
    {
        string _title;
        string _body;
        string _footer;
        string _configJson;
        string _viewFileName;
        string _description;
        NccWebSiteService _nccWebSiteService;
        IViewRenderService _viewRenderService;

        public string WidgetId { get { return "HelloWord.HelloWidget"; } }

        public string Title { get { return _title; } }

        public string Description { get { return _description; } }

        public string Body { get { return _body; } }

        public string Footer { get { return _footer; } }

        public string ConfigJson { get { return _configJson; } }

        public string ViewFileName { get { return _viewFileName; } }

        public HelloWidget(NccWebSiteService nccWebSiteService, IViewRenderService viewRenderService)
        {
            _nccWebSiteService = nccWebSiteService;
            _viewRenderService = viewRenderService;
        }

        public void Init()
        {
            _title = "Hellow Widget world";
            _description = "This is a sample widget which will show an image.";            
            _footer = "Footer";
            _viewFileName = "Widgets/Hello";
        }

        public string RenderBody()
        {
            var webSite = _nccWebSiteService.LoadAll().FirstOrDefault();
            _body = _viewRenderService.RenderToStringAsync<HelloWidgetController>(_viewFileName, null).Result;             
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

        public string RenderConfig(long websiteWidgetId)
        {
            return "";
        }
    }
}
