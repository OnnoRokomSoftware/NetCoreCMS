/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Modules.News.Models.Entity;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Modules.News.Models
{
    public class Builder : IModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NeCategory>().ToTable(GlobalContext.GetTableName<NeCategory>());
            modelBuilder.Entity<NeCategoryDetails>().ToTable(GlobalContext.GetTableName<NeCategoryDetails>());
            modelBuilder.Entity<NeNews>().ToTable(GlobalContext.GetTableName<NeNews>());
            modelBuilder.Entity<NeNewsDetails>().ToTable(GlobalContext.GetTableName<NeNewsDetails>());

            modelBuilder.Entity<NeNewsCategory>(b =>
            {
                b.ToTable(GlobalContext.GetTableName<NeNewsCategory>());
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
