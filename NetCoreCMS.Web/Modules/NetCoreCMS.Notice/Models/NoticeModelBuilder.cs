using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Notice.Models;

namespace NetCoreCMS.Core.Modules.Cms.Models
{
    public class NoticeModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NccNotice>().ToTable("Ncc_Notice");
        }
    }
}
