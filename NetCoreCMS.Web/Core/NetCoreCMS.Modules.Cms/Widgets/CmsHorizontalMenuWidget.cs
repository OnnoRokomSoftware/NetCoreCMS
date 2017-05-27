using NetCoreCMS.Framework.Modules.Widgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.HelloWorld.Widgets
{
    public class CmsHorizontalMenuWidget : IWidget
    {
        string _title;
        string _body;
        string _footer;
        string _configJson;
        string _description;

        public string WidgetId { get { return "E75C5E82-A7CC-4E5E-8142-CC7DECA64C1F"; } }

        public string Title { get { return _title; } }

        public string Description { get { return _description; } }

        public string Body { get { return _body; } }

        public string Footer { get { return _footer; } }

        public string ConfigJson { get { return _configJson; } }

        public CmsHorizontalMenuWidget()
        {

        }

        public void Init()
        {
            _title = "Horizontal Menu";
            _description = "Horizontal nevigation menu";
            _body = "<ul>" +
                        "<li><a href='/CmsHome'>Home</a></li>" +
                        "<li><a href='/CmsPage'>Pages</a></li>" +
                        "<li><a href='/CmsBlog'>Posts</a></li>" +
                        "<li><a href='/CmsPage/View/?Id=About'>About</a></li>" +
                        "<li><a href='/CmsPage/View/?Id=Contact'>Contact</a></li>" +
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
