using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.ImageSlider.Controllers;
using NetCoreCMS.ImageSlider.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.ImageSlider.Widgets
{
    public class ImageSliderWidget : IWidget
    {
        string _title;
        string _body;
        string _footer;
        string _configJson;
        string _description;
        string _viewFileName;
        IViewRenderService _viewRenderService;
        NccImageSliderService _imageSliderService;

        public string WidgetId { get { return "NetCoreCMS.Modules.ImageSlider.Widgets.NoticeVerticalWidget"; } }

        public string Title { get { return _title; } }

        public string Description { get { return _description; } }

        public string Body { get { return _body; } }

        public string Footer { get { return _footer; } }

        public string ConfigJson { get { return _configJson; } }

        public string ViewFileName { get { return _viewFileName; } }

        public ImageSliderWidget(IViewRenderService viewRenderService, NccImageSliderService imageSliderService)
        {
            _viewRenderService = viewRenderService;
            _imageSliderService = imageSliderService;
        }

        public void Init()
        {
            _title = "Image Slider Widget";
            _description = "This is a sample widget which will scroll image.";
            _footer = "Footer";
            _viewFileName = "Widgets/ImageSlider";
        }

        public string RenderBody()
        {
            var itemList = _imageSliderService.LoadAllActive();
            _body = _viewRenderService.RenderToStringAsync<ImageSliderWidgetController>(_viewFileName, itemList).Result;
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
