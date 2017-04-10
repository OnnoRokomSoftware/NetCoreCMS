/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;


namespace NetCoreCMS.Core.Modules.Settings.Models
{
    public class SettingsModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {                        
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
