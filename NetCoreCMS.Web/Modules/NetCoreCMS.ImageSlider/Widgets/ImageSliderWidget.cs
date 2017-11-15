/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.ImageSlider.Controllers;
using NetCoreCMS.ImageSlider.Models;
using NetCoreCMS.ImageSlider.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.ImageSlider.Widgets
{
    public class ImageSliderWidget : Widget
    {
        NccImageSliderService _imageSliderService;
        IViewRenderService _viewRenderService;
        NccWebSiteWidgetService _websiteWidgetService;
        string selectedImageSliderName = "";

        public ImageSliderWidget(
            IViewRenderService viewRenderService,
            NccWebSiteWidgetService websiteWidgetService,
            NccImageSliderService imageSliderService) : base(
                "NetCoreCMS.Modules.Widgets.ImageSlider",
                "Image Slider",
                "This is a widget to display responsive image slider.",
                ""
            )
        {
            _viewRenderService = viewRenderService;
            _websiteWidgetService = websiteWidgetService;
            _imageSliderService = imageSliderService;
        }

        public override void Init(long websiteWidgetId)
        {
            WebSiteWidgetId = websiteWidgetId;
            ViewFileName = "Widgets/ImageSlider";

            var webSiteWidget = _websiteWidgetService.Get(websiteWidgetId, true);
            if (webSiteWidget != null && !string.IsNullOrEmpty(webSiteWidget.WidgetConfigJson))
            {
                var configJson = webSiteWidget.WidgetConfigJson;
                var config = JsonConvert.DeserializeObject<dynamic>(configJson);
                Language = config.language;
                DisplayTitle = config.title;
                Footer = config.footer;
                selectedImageSliderName = config.name;
            }

            ConfigViewFileName = "Widgets/ImageSliderConfig";
            var itemList = _imageSliderService.LoadAll(true).ToList();
            ConfigHtml = _viewRenderService.RenderToStringAsync<ImageSliderWidgetController>(ConfigViewFileName, itemList /*webSiteWidget*/).Result;
        }

        public override string RenderBody()
        {
            var itemList = _imageSliderService.LoadAll(true, -1, selectedImageSliderName).FirstOrDefault();
            if (selectedImageSliderName.Trim() == "") { itemList = _imageSliderService.LoadAll().FirstOrDefault(); }
            var body = _viewRenderService.RenderToStringAsync<ImageSliderWidgetController>(ViewFileName, itemList).Result;
            return body;
        }
    }
}
