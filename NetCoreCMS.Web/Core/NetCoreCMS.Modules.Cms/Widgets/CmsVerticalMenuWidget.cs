using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Modules.Cms.Controllers;
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
        string _description;
        string _viewFileName;
        IViewRenderService _viewRenderService;

        public string WidgetId { get { return "NetCoreCMS.Modules.Cms.CmsVerticalMenuWidget"; } }

        public string Title { get { return _title; } }

        public string Description { get { return _description; } }

        public string Body { get { return _body; } }

        public string Footer { get { return _footer; } }

        public string ConfigJson { get { return _configJson; } }
        public string ViewFileName { get { return _viewFileName; } }

        public CmsVerticalMenuWidget(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }

        public void Init()
        {
            _title = "Vertical Menu";
            _description = "Vertical nevigation menu";
            _footer = "Footer";
            _viewFileName = "Widgets/CmsVerticalMenu";
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

        public string RenderConfig()
        {
            throw new NotImplementedException();
        }
    }
}
