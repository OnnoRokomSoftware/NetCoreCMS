using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Modules.Settings.Models;

namespace NetCoreCMS.Core.Modules.Settings.Models
{
    public class SettingsModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WebSite>().ToTable("Ncc_WebSite");
            
            //modelBuilder.Entity<BaseModel>(e =>
            //{
            //    e.HasKey(x => x.Id);
            //    e.Property(x => x.CreateBy);
            //    e.Property(x => x.CreationDate);
            //    e.Property(x => x.ModificationDate);
            //    e.Property(x => x.ModifyBy);
            //    e.Property(x => x.Name);
            //    e.Property(x => x.Status);
            //    e.Property(x => x.VersionNumber);
            //});
            
        }
    }
}
