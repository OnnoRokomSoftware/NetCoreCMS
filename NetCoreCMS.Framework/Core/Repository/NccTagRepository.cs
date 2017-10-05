using System;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccTagRepository : BaseRepository<NccTag, long>
    {
        public NccTagRepository(NccDbContext context) : base(context)
        {
        }

        public List<NccTag> LoadRecentTag(int count)
        {
            var query = Query().Include("Posts");
            return query.OrderByDescending(x => x.CreationDate).Take(count).ToList();
        }

        public List<NccTag> Search(string name, int count = 20)
        {
            var query = Query().Include("Posts").Where(x => x.Name.Contains(name));
            return query.OrderByDescending(x => x.CreationDate).Take(count).ToList();
        }
    }
}