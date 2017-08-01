using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.ImageSlider.Models;

namespace NetCoreCMS.ImageSlider.Models
{
    public class NccImageSliderModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NccImageSlider>().ToTable("Ncc_Image_Slider");
            modelBuilder.Entity<NccImageSliderItem>().ToTable("Ncc_Image_Slider_Title");
        }
    }
}
