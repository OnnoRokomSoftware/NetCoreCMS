using NetCoreCMS.Framework.Modules.Widgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.HelloWorld.Widgets
{
    public class CmsSlideShowWidget : IWidget
    {
        string _title;
        string _body;
        string _footer;
        string _configJson;
        string _description;

        public string WidgetId { get { return "E082E185-2379-4C77-A026-FB73FFD09915"; } }

        public string Title { get { return _title; } }

        public string Description { get { return _description; } }

        public string Body { get { return _body; } }

        public string Footer { get { return _footer; } }

        public string ConfigJson { get { return _configJson; } }

        public CmsSlideShowWidget()
        {

        }

        public void Init()
        {
            _title = "Slide Show";
            _description = "Image Slide Show";
            _body = "<ul>" +
                        "<li><img src='/Media/Picture/?Id=32323' /></li>" + 
                        "<li><img src='/Media/Picture/?Id=32344' /></li>" +
                    "</ul>";
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

        public string RenderConfig()
        {
            throw new NotImplementedException();
        }
    }
}
