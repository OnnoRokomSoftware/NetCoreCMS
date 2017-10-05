using System;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccCategoryRepository : BaseRepository<NccCategory, long>
    {
        public NccCategoryRepository(NccDbContext context) : base(context)
        {
        }

        public List<NccCategory> LoadByParrentId(long parrentId, bool isActive)
        {
            return DbSet.Where(x => x.Parent.Id == parrentId).ToList();
        }

        public NccCategory GetWithPost(string slug)
        {
            return DbSet.Include("CategoryDetails").Include("Posts").Include("Posts.Post").Include("Posts.Post.Author").Include("Posts.Post.PostDetails").Include("Posts.Post.Comments").Where(x => x.CategoryDetails.Any(d=>d.Slug == slug)).FirstOrDefault();            
        }
    }
}