using NetCoreCMS.Framework.Modules.Widgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.HelloWorld.Widgets
{
    public class HelloWidget : IWidget
    {
        string _title;
        string _body;
        string _footer;
        
        public string WidgetID { get { return "76983E5A-0E4C-473E-B8BC-0C63F6733FCF"; } }

        public string Title { get { return _title; } }

        public string Body { get { return _body; } }

        public string Footer { get { return _footer; } }

        public HelloWidget()
        {

        }

        public void Prepare()
        {
            _title = "Hellow Widget world";
            _body = "<img src='https://static.pexels.com/photos/158607/cairn-fog-mystical-background-158607.jpeg' width='100' height='100' />";
            _footer = "Footer";
        }

        public string Render()
        {
            var html = "<div class='panel panel-default'>" +
                            "<div class='panel-heading'>" +
                                _title +
                            "</div>"+
                            "<div class='panel-body'> " +
                                _body +
                            "</div>" + 
                            "<div class='panel-footer'>" +
                                _footer + 
                            "</div>" +
                        "</div>";
            return html;
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
    }
}
