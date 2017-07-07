using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.ImageSlider.Models;

namespace NetCoreCMS.Modules.ImageSlider.Models
{
    public class ImageSliderModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NccImageSlider>().ToTable("Ncc_Module_ImageSlider");                       
        }
    }
}
