/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/


namespace NetCoreCMS.ImageSlider.Models.Entities
{
    public class NccImageSliderSettings
    {
        public NccImageSliderSettings()
        {
            ModuleVersion = "1.0";
        }

        public string ModuleVersion { get; set; }
    }
}