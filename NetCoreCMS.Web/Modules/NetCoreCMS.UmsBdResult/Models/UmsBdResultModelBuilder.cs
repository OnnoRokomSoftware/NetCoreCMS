/*
 * Author: OnnoRokom Software Ltd
 * Website: http://onnorokomsoftware.com
 * Copyright (c) onnorokomsoftware.com
 * License: Commercial
*/
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.UmsBdResult.Models;

namespace NetCoreCMS.UmsBdResult.Models
{
    public class UmsBdResultModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UmsBdResultSettings>().ToTable("Ncc_UmsBdResult_settings");                       
        }
    }
}
