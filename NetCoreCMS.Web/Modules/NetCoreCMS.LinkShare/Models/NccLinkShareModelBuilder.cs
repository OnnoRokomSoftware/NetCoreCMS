using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.LinkShare.Models;

namespace NetCoreCMS.LinkShare.Models
{
    public class NccLinkShareModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NccCategory>().ToTable("Ncc_LinkShare_Category");
            modelBuilder.Entity<NccLinkShare>().ToTable("Ncc_LinkShare");
        }
    }
}
