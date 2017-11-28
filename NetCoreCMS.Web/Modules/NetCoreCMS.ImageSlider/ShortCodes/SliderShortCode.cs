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
using NetCoreCMS.Framework.Core.ShotCodes;
using NetCoreCMS.ImageSlider.Controllers;
using NetCoreCMS.ImageSlider.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.ImageSlider.ShortCodes
{
    public class SliderShortCode : IShortCode
    {
        NccImageSliderService _nccImageSliderService;
        IViewRenderService _viewRenderService;
        string ViewFileName = "Widgets/ImageSlider";

        public SliderShortCode(NccImageSliderService nccImageSliderService, IViewRenderService viewRenderService)
        {
            _nccImageSliderService = nccImageSliderService;
            _viewRenderService = viewRenderService;
        }
        public string ShortCodeName { get { return "NccImageSlider"; } }

        public string Render(params object[] paramiters)
        {
            var id = paramiters[0].ToString().Trim();
            var slider = _nccImageSliderService.Get(long.Parse(id));
            var content = "";
            if(slider != null)
            {
                content = _viewRenderService.RenderToStringAsync<ImageSliderWidgetController>(ViewFileName, slider).Result;
            }
            return content;
        }
    }
}
