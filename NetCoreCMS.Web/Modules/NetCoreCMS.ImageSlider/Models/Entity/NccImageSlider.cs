/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.ImageSlider.Models.Entity
{
    public class NccImageSlider : BaseModel<long>
    {
        public NccImageSlider()
        { 
            Interval = 6;
            ShowNav = true;
            ShowSideNav = true;
            ImageWidth = "100%";
            ImageItems = new List<NccImageSliderItem>();
        }
          
        public string ContainerStyle { get; set; }
        //public int TotalSlide { get; set; }
        public int Interval { get; set; }
        public bool ShowNav { get; set; }
        public bool ShowSideNav { get; set; }
        public string ImageWidth { get; set; }
        public string ImageHeight { get; set; }
        public List<NccImageSliderItem> ImageItems { get; set; }
    }

    public class NccImageSliderItem: BaseModel<long>
    {
        public NccImageSliderItem()
        {
            
        }
          
        [Required]
        public string Path { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}