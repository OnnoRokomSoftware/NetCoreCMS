using NetCoreCMS.Framework.Modules.Widgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.HelloWorld.Widgets
{
    public class TopBarWidget : IWidget
    {
        string _title;
        string _body;
        string _footer;
        string _configJson;
        string _description;
        string _viewFileName;

        public string WidgetId { get { return "75983E5A-0E4C-473E-B8BC-0C23F6733ABA"; } }

        public string Title { get { return _title; } }

        public string Description { get { return _description; } }

        public string Body { get { return _body; } }

        public string Footer { get { return _footer; } }

        public string ConfigJson { get { return _configJson; } }
        public string ViewFileName { get { return _viewFileName; } }

        public TopBarWidget()
        {

        }

        public void Init()
        {
            _title = "Top Bar Widget";
            _description = "This is a sample widget which will show an image.";
            _body = "<div style='height:20px; width:100%; text-alight:right; padding: 5px 10px;' ><div style='float:right;'><a href = 'http://google.com/+' > Google Plus </a> |<a href = 'http://facebook.com/xonaki' > Facebook </a> | <a href='http://twitter.com/xonakibd' > Twitter </a> </div> </div> ";
            _footer = "Footer";
        }

        public string RenderBody()
        { 
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
