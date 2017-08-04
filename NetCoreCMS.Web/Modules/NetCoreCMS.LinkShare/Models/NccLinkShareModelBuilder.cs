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
            modelBuilder.Entity<NccCategory>().ToTable("Ncc_LS_Category");
            modelBuilder.Entity<NccLinkShare>().ToTable("Ncc_LS_LinkShare");

            modelBuilder.Entity<NccCategoryLinkShare>(b =>
            {
                b.ToTable("Ncc_LS_Category_LinkShare");
                b.HasKey(bc => new { bc.NccCategoryId, bc.NccLinkShareId });
            });

            modelBuilder.Entity<NccCategoryLinkShare>()
                .HasOne(bc => bc.Category)
                .WithMany(b => b.LinkShares)
                .HasForeignKey(bc => bc.NccCategoryId);

            modelBuilder.Entity<NccCategoryLinkShare>()
                .HasOne(bc => bc.NccLinkShare)
                .WithMany(c => c.Categories)
                .HasForeignKey(bc => bc.NccLinkShareId);
        }
    }
}
