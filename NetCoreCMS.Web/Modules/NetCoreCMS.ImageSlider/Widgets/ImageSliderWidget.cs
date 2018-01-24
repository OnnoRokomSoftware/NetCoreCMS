/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System.Linq;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.ImageSlider.Controllers;
using NetCoreCMS.ImageSlider.Services;

namespace NetCoreCMS.ImageSlider.Widgets
{
    public class ImageSliderWidget : Widget
    {
        NccImageSliderService _imageSliderService;        
        string selectedImageSliderName = "";

        public ImageSliderWidget(NccImageSliderService imageSliderService) : base(
            typeof(ImageSliderHomeController),
            "Image Slider",
            "This is a widget to display responsive image slider.",
            "", 
            "Widgets/ImageSlider", 
            "Widgets/ImageSliderConfig" )
        {
            _imageSliderService = imageSliderService;
        }

        public override void InitConfig(dynamic config)
        {   
            selectedImageSliderName = config.name;
        }

        public override object PrepareConfigModel()
        {
            var model = _imageSliderService.LoadAll(true);            
            return model;
        }

        public override object PrepareViewModel()
        {
            var model = _imageSliderService.LoadAll(true, -1, selectedImageSliderName).FirstOrDefault();
            if (selectedImageSliderName.Trim() == "") { model = _imageSliderService.LoadAll().FirstOrDefault(); }            
            return model;
        }
    }
}
