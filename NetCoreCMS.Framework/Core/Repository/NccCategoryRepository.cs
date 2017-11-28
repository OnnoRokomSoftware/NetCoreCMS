/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Models.ViewModels;

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
            return DbSet.Include("CategoryDetails").Include("Posts").Include("Posts.Post").Include("Posts.Post.Author").Include("Posts.Post.PostDetails")
                .Where(x => x.CategoryDetails.Any(d => d.Slug == slug))
                .FirstOrDefault();
        }

        public NccCategory GetBySlug(string slug)
        {
            return DbSet.Include("CategoryDetails")
                .Where(x => x.CategoryDetails.Any(d => d.Slug == slug))
                .FirstOrDefault();
        }

        /// <summary>
        /// Use this function to count post
        /// </summary>
        /// <param name="isActive">Load active records</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <returns></returns>
        public long Count(bool isActive, string keyword = "")
        {
            return GetBaseQuery(isActive, keyword).Count();
        }

        /// <summary>
        /// Use this function to lead post
        /// </summary>
        /// <param name="from">Starting index</param>
        /// <param name="total">Total record you want</param>
        /// <param name="isActive">Load active records</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <param name="orderBy">Order by field name</param>
        /// <param name="orderDir">Order by (asc / desc)</param>
        /// <returns></returns>
        public List<NccCategory> Load(int from, int total, bool isActive, string keyword = "", string orderBy = "", string orderDir = "")
        {
            var query = GetBaseQuery(isActive, keyword);
            if (orderBy.ToLower() == "name")
            {
                if (orderDir.ToLower() == "asc")
                    query = query.OrderBy(x => x.Name);
                else
                    query = query.OrderByDescending(x => x.Name);
            }

            query = query.OrderByDescending(x => x.CreationDate);
            return query.Skip(from).Take(total).ToList();
        }



        #region Helper
        private IQueryable<NccCategory> GetBaseQuery(bool isActive, string keyword)
        {
            var baseQuery = Query()
                .Include("CategoryDetails")
                .Include("Parent")
                .Include("Parent.CategoryDetails")
                .Include("Posts")
                .Where(x => x.Status != EntityStatus.Deleted);

            if (isActive == true)
            {
                baseQuery = baseQuery.Where(x => x.Status == EntityStatus.Active);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                baseQuery = baseQuery.Where(x => x.CategoryDetails.Any(y => y.Title.Contains(keyword)));
            }
            return baseQuery;
        }
        #endregion
    }
}