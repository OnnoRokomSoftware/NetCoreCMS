using System;
using System.Linq;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccPostRepository : BaseRepository<NccPost, long>
    {
        public NccPostRepository(NccDbContext context) : base(context)
        {
        }

        public List<NccPost> LoadRecentPosts(int count)
        {
            var list = Query()
                .Where(x=>x.PostStatus == NccPost.NccPostStatus.Published)
                .OrderByDescending(x => x.PublishDate)
                .Take(count)
                .ToList();
            return list;
        }

        public List<NccPost> LoadPublished(int from, int total)
        {
            return Query().Include("PostDetails").Include("Author").Include("Comments").Include("Categories").Include("Categories.Category").Include("Categories.Category.CategoryDetails").Include("Tags").Include("Tags.Tag")
                .Where(x => x.PostStatus == NccPost.NccPostStatus.Published && x.PublishDate <= DateTime.Now)
                .OrderByDescending(x => x.PublishDate)
                .Skip(from)
                .Take(total)
                .ToList();
        }
    }
}
