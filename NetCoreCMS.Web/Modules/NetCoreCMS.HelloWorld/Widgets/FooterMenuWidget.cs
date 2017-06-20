using NetCoreCMS.Framework.Modules.Widgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.HelloWorld.Widgets
{
    public class FooterMenuWidget : IWidget
    {
        string _title;
        string _body;
        string _footer;
        string _configJson;
        string _description;
        string _viewFileName;

        public string WidgetId { get { return "76983E5A-0E4C-4753-B8BC-1C63F6733GFC"; } }

        public string Title { get { return _title; } }

        public string Description { get { return _description; } }

        public string Body { get { return _body; } }

        public string Footer { get { return _footer; } }

        public string ConfigJson { get { return _configJson; } }
        public string ViewFileName { get { return _viewFileName; } }
        public FooterMenuWidget()
        {

        }

        public void Init()
        {
            _title = "Footer menu";
            _description = "This is a sample widget which will show an image.";
            _body = "<div style='height:40px; width:100%; text-align:center; padding: 5px 10px;'> <a href='/CmsHome'>Home</a> | <a href='/CmsHome/About'>About</a> |<a href='/CmsHome/Contact'>Contact</a> |<a href='/Admin'>Admin</a>   </div>";
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
