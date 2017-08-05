using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.LinkShare.Models;

namespace NetCoreCMS.LinkShare.Models
{
    public class LsModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LsCategory>().ToTable("Ncc_LS_Category");
            modelBuilder.Entity<LsLink>().ToTable("Ncc_LS_Link");

            modelBuilder.Entity<LsLinkCategory>(b =>
            {
                b.ToTable("Ncc_LS_Link_Category");
                b.HasKey(bc => new { bc.LsCategoryId, bc.LsLinkId });
            });

            modelBuilder.Entity<LsLinkCategory>()
                .HasOne(bc => bc.LsCategory)
                .WithMany(b => b.Links)
                .HasForeignKey(bc => bc.LsCategoryId);

            modelBuilder.Entity<LsLinkCategory>()
                .HasOne(bc => bc.LsLink)
                .WithMany(c => c.Categories)
                .HasForeignKey(bc => bc.LsLinkId);
        }
    }
}
