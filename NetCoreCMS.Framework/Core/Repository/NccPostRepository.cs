using System;
using System.Linq;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;

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
    }
}
