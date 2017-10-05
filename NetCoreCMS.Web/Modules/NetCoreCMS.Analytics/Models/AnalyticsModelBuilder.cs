using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Modules.Analytics.Models;

namespace NetCoreCMS.Modules.Analytics.Models
{
    public class AnalyticsModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnalyticsModel>().ToTable("Ncc_Analytics");
            modelBuilder.Entity<AnalyticsLogModel>().ToTable("Ncc_Analytics_Log");
        }
    }
}
