/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccCategoryDetailsRepository : BaseRepository<NccCategoryDetails, long>
    {
        public NccCategoryDetailsRepository(NccDbContext context) : base(context)
        {
        }

        public NccCategoryDetails Get(string slug, string language)
        {
            var query = Query().Include("Category").Include("Category.CategoryDetails").Where(x => x.Slug == slug);
            if (!string.IsNullOrEmpty(language))
            {
                query = query.Where(x => x.Language == language);
            }
            return query.FirstOrDefault();
        }

        public List<NccCategoryDetails> LoadByParrentId(long parrentId, bool isActive)
        {
            return DbSet.Where(x => x.Category.Id == parrentId).ToList();
        }
        
        public List<NccCategoryDetails> LoadRecentCategoryDetails(int count, string language = "")
        {
            var query = Query().Include("Category").Include("Category.CategoryDetails");
            if (!string.IsNullOrEmpty(language))
            {
                query = query.Where(x => x.Language == language);
            }
            return query.OrderByDescending(x => x.Category.CreationDate).Take(count).ToList();
        }

        public List<NccCategoryDetails> Search(string name, string language = "", int count = 20)
        {
            var query = Query().Include("Category").Include("Category.CategoryDetails").Where(x => x.Name.Contains(name) || x.Title.Contains(name) || x.Slug.Contains(name) || x.Category.Name.Contains(name));
            if (!string.IsNullOrEmpty(language))
            {
                query = query.Where(x => x.Language == language);
            }
            return query.OrderByDescending(x => x.Category.CreationDate).Take(count).ToList();
        }
    }
}