using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Modules.News.Models;

namespace NetCoreCMS.Modules.News.Models
{
    public class NeModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NeCategory>().ToTable("Ncc_Ne_Category");
            modelBuilder.Entity<NeNews>().ToTable("Ncc_Ne_News");

            modelBuilder.Entity<NeNewsCategory>(b =>
            {
                b.ToTable("Ncc_Ne_News_Category");
                b.HasKey(bc => new { bc.NeCategoryId, bc.NeNewsId });
            });

            modelBuilder.Entity<NeNewsCategory>()
                .HasOne(bc => bc.NeCategory)
                .WithMany(b => b.NewsList)
                .HasForeignKey(bc => bc.NeCategoryId);

            modelBuilder.Entity<NeNewsCategory>()
                .HasOne(bc => bc.NeNews)
                .WithMany(c => c.CategoryList)
                .HasForeignKey(bc => bc.NeNewsId);
        }
    }
}
