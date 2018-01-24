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
using NetCoreCMS.EasyNews.Models.Entities;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.EasyNews.Models
{
    public class Builder : IModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().ToTable(GlobalContext.GetTableName<Category>());
            modelBuilder.Entity<CategoryDetails>().ToTable(GlobalContext.GetTableName<CategoryDetails>());
            modelBuilder.Entity<News>().ToTable(GlobalContext.GetTableName<News>());
            modelBuilder.Entity<NewsDetails>().ToTable(GlobalContext.GetTableName<NewsDetails>());

            modelBuilder.Entity<NewsCategory>(b =>
            {
                b.ToTable(GlobalContext.GetTableName<NewsCategory>());
                b.HasKey(bc => new { bc.CategoryId, bc.NewsId });
            });

            modelBuilder.Entity<NewsCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(b => b.NewsList)
                .HasForeignKey(bc => bc.CategoryId);

            modelBuilder.Entity<NewsCategory>()
                .HasOne(bc => bc.News)
                .WithMany(c => c.CategoryList)
                .HasForeignKey(bc => bc.NewsId);
        }
    }
}
