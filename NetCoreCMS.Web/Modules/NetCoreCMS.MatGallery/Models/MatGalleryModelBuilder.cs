using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.MatGallery.Models;

namespace NetCoreCMS.MatGallery.Models
{
    public class MatGalleryModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NccUserModule>().ToTable("Ncc_MTG_User_Module");
            modelBuilder.Entity<NccUserModuleLog>().ToTable("Ncc_MTG_User_Module_Log");

            modelBuilder.Entity<NccUserTheme>().ToTable("Ncc_MTG_User_Theme");
            modelBuilder.Entity<NccUserThemeLog>().ToTable("Ncc_MTG_User_Theme_Log");
        }
    }
}
