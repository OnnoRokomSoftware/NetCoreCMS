/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Modules.ShortCode;
using NetCoreCMS.ImageSlider.Controllers;
using NetCoreCMS.ImageSlider.Services;

namespace NetCoreCMS.ImageSlider.ShortCodes
{
    public class SliderShortCode : BaseShortCode
    {
        NccImageSliderService _nccImageSliderService;

        public SliderShortCode(NccImageSliderService nccImageSliderService) : base(typeof(ImageSliderWidgetController), "NccImageSlider", "ShortCodes/ImageSlider")
        {
            _nccImageSliderService = nccImageSliderService;
        }
        public override object PrepareViewModel(params object[] paramiters)
        {
            var id = paramiters[0].ToString().Trim();
            var model = _nccImageSliderService.Get(long.Parse(id));

            return model;
        }
    }
}