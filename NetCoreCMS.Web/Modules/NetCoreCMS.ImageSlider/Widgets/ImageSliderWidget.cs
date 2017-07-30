using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.ImageSlider.Controllers;
using NetCoreCMS.ImageSlider.Models;
using Newtonsoft.Json;
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
        private NccSettingsService _nccSettingsService;

        public string WidgetId { get { return "NetCoreCMS.ImageSlider.Widgets.ImageSliderWidget"; } }

        public string Title { get { return _title; } }

        public string Description { get { return _description; } }

        public string Body { get { return _body; } }

        public string Footer { get { return _footer; } }

        public string ConfigJson { get { return _configJson; } }

        public string ViewFileName { get { return _viewFileName; } }

        public ImageSliderWidget(IViewRenderService viewRenderService, NccSettingsService nccSettingsService)
        {
            _viewRenderService = viewRenderService;

            _nccSettingsService = nccSettingsService;
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
            NccImageSlider nccImageSlider=new NccImageSlider();
            List<NccImageSliderItem> nccImageSliderItemList = new List<NccImageSliderItem>();
            var tempSettings = _nccSettingsService.GetByKey("NccImageSlider_Settings");
            if (tempSettings != null)
            {
                nccImageSlider = JsonConvert.DeserializeObject<NccImageSlider>(tempSettings.Value);

                var tempSettings2 = _nccSettingsService.GetByKey("NccImageSlider_Items");
                if (tempSettings2 != null)
                {
                    nccImageSlider.ImageItems = new List<NccImageSliderItem>();
                    nccImageSliderItemList = JsonConvert.DeserializeObject<List<NccImageSliderItem>>(tempSettings2.Value);
                    foreach (var item in nccImageSliderItemList)
                    {
                        nccImageSlider.ImageItems.Add(item);
                    }
                }
            }
            _body = _viewRenderService.RenderToStringAsync<ImageSliderWidgetController>(_viewFileName, nccImageSlider).Result;
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

        public string RenderConfig(long websiteWidgetId)
        {
            return "";
        }
    }
}
