using NetCoreCMS.Framework.Modules.Widgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.HelloWorld.Widgets
{
    public class NoticeNavBarWidget : IWidget
    {
        string _title;
        string _body;
        string _footer;
        string _configJson;
        string _description;
        string _viewFileName;

        public string WidgetId { get { return "23983E5A-3E4C-472E-B8BC-045D643565"; } }

        public string Title { get { return _title; } }

        public string Description { get { return _description; } }

        public string Body { get { return _body; } }

        public string Footer { get { return _footer; } }

        public string ConfigJson { get { return _configJson; } }
        public string ViewFileName { get { return _viewFileName; } }

        public NoticeNavBarWidget()
        {

        }

        public void Init()
        {
            _title = "NavBar Widget";
            _description = "This is a sample widget which will show an image.";
            _body = "<div style='height:20px; width:100%; text-align:center; padding: 5px 10px;'> " +
                "<marquee behavior=\"scroll\" direction=\"left\"><img src=\"http://www.html.am/images/html-codes/marquees/fish-swimming.gif\" width=\"74\" height=\"30\" alt=\"Swimming fish\" /></marquee>" +
                "</div>";
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
