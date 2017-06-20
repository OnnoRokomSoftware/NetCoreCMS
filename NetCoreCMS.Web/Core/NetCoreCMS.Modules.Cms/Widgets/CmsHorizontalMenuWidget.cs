using NetCoreCMS.Framework.Modules.Widgets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.HelloWorld.Widgets
{
    public class CmsHorizontalMenuWidget : IWidget
    {
        string _title;
        string _body;
        string _footer;
        string _configJson;
        string _description;
        string _viewFileName;

        public string WidgetId { get { return "NetCoreCMS.Modules.Cms.CmsHorizontalMenuWidget"; } }

        public string Title { get { return _title; } }

        public string Description { get { return _description; } }

        public string Body { get { return _body; } }

        public string Footer { get { return _footer; } }

        public string ConfigJson { get { return _configJson; } }
        public string ViewFileName { get { return _viewFileName; } }

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
