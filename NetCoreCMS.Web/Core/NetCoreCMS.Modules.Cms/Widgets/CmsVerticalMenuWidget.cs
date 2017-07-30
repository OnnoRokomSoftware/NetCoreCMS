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
    public class CmsVerticalMenuWidget : IWidget
    {
        string _title;
        string _body;
        string _footer;
        string _configJson;
        string _configHtml;
        string _description;
        string _viewFileName;
        string _configViewFileName;

        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;

        public string WidgetId { get { return "NetCoreCMS.Modules.Cms.CmsVerticalMenuWidget"; } }

        public string Title { get { return _title; } }

        public string Description { get { return _description; } }

        public string Body { get { return _body; } }

        public string Footer { get { return _footer; } }

        public string ConfigJson { get { return _configJson; } }
        public string ConfigHtml { get { return _configHtml; } }
        public string ViewFileName { get { return _viewFileName; } }

        public CmsVerticalMenuWidget(IViewRenderService viewRenderService, NccWebSiteWidgetService websiteWidgetService)
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
        }

        public void Init()
        {
            _title = "Vertical Menu";
            _description = "Vertical nevigation menu";
            _footer = "Footer";
            _viewFileName = "Widgets/CmsVerticalMenu";
            _configViewFileName = "Widgets/CmsVerticalMenuConfig";
        }

        public string RenderBody()
        {
            _body = _viewRenderService.RenderToStringAsync<CmsWidgetController>(_viewFileName, null).Result;
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
            var websiteWidget = _websiteWidgetService.Get(websiteWidgetId);
            _configHtml = _viewRenderService.RenderToStringAsync<CmsWidgetController>(_configViewFileName, websiteWidget).Result;
            return _configHtml;
        }
    }
}
