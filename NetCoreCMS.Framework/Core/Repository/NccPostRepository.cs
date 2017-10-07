using System;
using System.Linq;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Mvc.Models;

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

        public List<NccPost> LoadPublished(int from, int total, bool withStiky, bool withFeatured)
        {
            var query = Query()
                .Include("PostDetails")
                .Include("Author")
                .Include("Comments")
                .Include("Categories")
                .Include("Categories.Category")
                .Include("Categories.Category.CategoryDetails")
                .Include("Tags")
                .Include("Tags.Tag")
                .Where(x => x.PostStatus == NccPost.NccPostStatus.Published && x.PublishDate <= DateTime.Now);

                if (withStiky == false)
                {
                    query = query.Where(x => x.IsStiky == withStiky);
                }

                if (withFeatured == false)
                {
                    query = query.Where(x => x.IsFeatured == withFeatured);
                }

                return query.OrderByDescending(x => x.PublishDate)
                .Skip(from * total)
                .Take(total)
                .ToList();
        }

        public List<NccPost> LoadPosts(bool isSticky = false, bool isFeature = false)
        {
            var query = Query()
                .Include("PostDetails")
                .Include("Author")
                .Include("Comments")
                .Include("Categories")
                .Include("Categories.Category")
                .Include("Categories.Category.CategoryDetails")
                .Include("Tags")
                .Include("Tags.Tag")
                .Where(x => x.PostStatus == NccPost.NccPostStatus.Published && x.PublishDate <= DateTime.Now && x.IsFeatured);

            if (isSticky)
            {
                query.Where(x => x.IsStiky);
            }
            if (isFeature)
            {
                query.Where(x => x.IsFeatured);
            }                
            query.OrderByDescending(x => x.PublishDate);
            return query.ToList();
        }

        public long GetCount(NccPost.NccPostStatus status)
        {
            return Query().Where(x => x.PostStatus ==  status && x.Status == EntityStatus.Active).Count();
        }
    }
}
