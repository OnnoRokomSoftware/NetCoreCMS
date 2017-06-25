using NetCoreCMS.Framework.Modules.Widgets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.HelloWorld.Widgets
{
    public class CmsSlideShowWidget : IWidget
    {
        string _title;
        string _body;
        string _footer;
        string _configJson;
        string _description;
        string _viewFileName;

        public string WidgetId { get { return "E082E185-2379-4C77-A026-FB73FFD09915"; } }

        public string Title { get { return _title; } }

        public string Description { get { return _description; } }

        public string Body { get { return _body; } }

        public string Footer { get { return _footer; } }

        public string ConfigJson { get { return _configJson; } }
        public string ViewFileName { get { return _viewFileName; } }

        public CmsSlideShowWidget()
        {

        }

        public void Init()
        {
            _title = "Slide Show";
            _description = "Image Slide Show";
            //_body = "<ul>" +
            //            "<li><img src='/Media/Picture/?Id=32323' /></li>" + 
            //            "<li><img src='/Media/Picture/?Id=32344' /></li>" +
            //        "</ul>";
            _body = "<div id=\"carousel-example-generic\" class=\"carousel slide\" data-ride=\"carousel\">" +
                    "    <ol class=\"carousel-indicators\">" +
                    "        <li data-target=\"#carousel-example-generic\" data-slide-to=\"0\" class=\"active\"></li>" +
                    "        <li data-target=\"#carousel-example-generic\" data-slide-to=\"1\" class=\"\"></li>" +
                    "        <li data-target=\"#carousel-example-generic\" data-slide-to=\"2\" class=\"\"></li>" +
                    "        <li data-target=\"#carousel-example-generic\" data-slide-to=\"3\" class=\"\"></li>" +
                    "    </ol>" +
                    "    <div class=\"carousel-inner\" role=\"listbox\">" +
                    "        <div class=\"item active\">" +
                    "            <img src=\"/media/Images/2017/06/image-slider-0.jpg\" width=\"100%\" />" +
                    "        </div>" +
                    "        <div class=\"item\">" +
                    "            <img src=\"/media/Images/2017/06/image-slider-1.jpg\" width=\"100%\" />" +
                    "        </div>" +
                    "        <div class=\"item\">" +
                    "            <img src=\"/media/Images/2017/06/image-slider-2.jpg\" width=\"100%\" />" +
                    "        </div>" +
                    "        <div class=\"item\">" +
                    "            <img src=\"/media/Images/2017/06/image-slider-3.jpg\" width=\"100%\" />" +
                    "        </div>" +
                    "    </div>" +
                    "    <!--<a class=\"left carousel-control\" href=\"#carousel-example-generic\" role=\"button\" data-slide=\"prev\">" +
                    "        <span class=\"glyphicon glyphicon-chevron-left\" aria-hidden=\"true\"></span>" +
                    "        <span class=\"sr-only\">Previous</span>" +
                    "    </a>" +
                    "    <a class=\"right carousel-control\" href=\"#carousel-example-generic\" role=\"button\" data-slide=\"next\">" +
                    "        <span class=\"glyphicon glyphicon-chevron-right\" aria-hidden=\"true\"></span>" +
                    "        <span class=\"sr-only\">Next</span>" +
                    "    </a>-->" +
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
