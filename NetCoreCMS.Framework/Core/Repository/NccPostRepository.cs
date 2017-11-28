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
using System.Linq;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Models.ViewModels;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccPostRepository : BaseRepository<NccPost, long>
    {
        public NccPostRepository(NccDbContext context) : base(context)
        {
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
                .Where(x => x.PostStatus == NccPost.NccPostStatus.Published && x.PublishDate <= DateTime.Now);

            if (isSticky)
            {
                query = query.Where(x => x.IsStiky);
            }
            if (isFeature)
            {
                query = query.Where(x => x.IsFeatured);
            }
            query = query.OrderByDescending(x => x.PublishDate);
            return query.ToList();
        }
        public List<NccPost> LoadRecentPosts(int count)
        {
            var list = Query()
                .Include("PostDetails")
                .Where(x => x.PostStatus == NccPost.NccPostStatus.Published)
                .OrderByDescending(x => x.PublishDate)
                .Take(count)
                .ToList();
            return list;
        }
        public List<ArchiveItemViewModel> LoadArchive(bool decendingOrder = true)
        {
            NccDbQueryText query = new NccDbQueryText();
            query.MySql_QueryText = @"SELECT DATE_FORMAT(`PublishDate`, '%Y') `Year`, DATE_FORMAT(`PublishDate`, '%M') `Month`, CAST(DATE_FORMAT(`PublishDate`, '%m') AS UNSIGNED) `MonthValue`, COUNT(*) TotalPost
                                    FROM `ncc_post`
                                    WHERE `PublishDate`<=CURRENT_TIME()
	                                    AND `PostStatus` = " + ((int)NccPost.NccPostStatus.Published).ToString() + @"
                                    GROUP BY DATE_FORMAT(`PublishDate`, '%Y'), DATE_FORMAT(`PublishDate`, '%M') ";
            if (decendingOrder) query.MySql_QueryText += " ORDER BY PublishDate DESC ";
            else query.MySql_QueryText += " ORDER BY PublishDate ASC ";
            return ExecuteSqlQuery<ArchiveItemViewModel>(query).ToList();
        }

        /// <summary>
        /// Use this function to count post
        /// </summary>
        /// <param name="isActive">Load active records</param>
        /// <param name="isPublished">Load Published Records</param>
        /// <param name="withSticky">True to count with sticky and False to count without sticky post</param>
        /// <param name="withFeatured">True to count with featured and False to count without featured post</param>
        /// <param name="dateFrom">Pass value if you want to search between dates</param>
        /// <param name="dateTo">Pass value if you want to search between dates</param>
        /// <param name="categoryId">To count by category</param>
        /// <param name="tagId">To count by tag</param>
        /// <param name="createBy">To load by user</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <returns></returns>
        public long Count(bool isActive, bool isPublished, bool withSticky, bool withFeatured, DateTime? dateFrom = null, DateTime? dateTo = null, long categoryId = 0, long tagId = 0, long createBy = 0, string keyword = "")
        {
            return GetBaseQuery(isActive, isPublished, withSticky, withFeatured, dateFrom, dateTo, categoryId, tagId, createBy, keyword).Count();
        }

        /// <summary>
        /// Use this function to lead post
        /// </summary>
        /// <param name="from">Starting index</param>
        /// <param name="total">Total record you want</param>
        /// <param name="isActive">Load active records</param>
        /// <param name="isPublished">Load Published Records</param>
        /// <param name="withSticky">True to load with sticky and False to load without sticky post</param>
        /// <param name="withFeatured">True to load with featured and False to load without featured post</param>
        /// <param name="dateFrom">Pass value if you want to search between dates</param>
        /// <param name="dateTo">Pass value if you want to search between dates</param>
        /// <param name="categoryId">To load by category</param>
        /// <param name="tagId">To load by tag</param>
        /// <param name="createBy">To load by user</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <param name="orderBy">Order by field name</param>
        /// <param name="orderDir">Order by (asc / desc)</param>
        /// <returns></returns>
        public List<NccPost> Load(int from, int total, bool isActive, bool isPublished, bool withSticky, bool withFeatured, DateTime? dateFrom = null, DateTime? dateTo = null, long categoryId = 0, long tagId = 0, long createBy = 0, string keyword = "", string orderBy = "", string orderDir = "")
        {
            var query = GetBaseQuery(isActive, isPublished, withSticky, withFeatured, dateFrom, dateTo, categoryId, tagId, createBy, keyword);

            if (orderBy.ToLower() == "name")
            {
                if (orderDir.ToLower() == "asc")
                    query = query.OrderBy(x => x.Name);
                else
                    query = query.OrderByDescending(x => x.Name);
            }
            else if (orderBy.ToLower() == "publishdate")
            {
                if (orderDir.ToLower() == "asc")
                    query = query.OrderBy(x => x.PublishDate);
                else
                    query = query.OrderByDescending(x => x.PublishDate);
            }
            else if (orderBy.ToLower() == "creationdate")
            {
                if (orderDir.ToLower() == "asc")
                    query = query.OrderBy(x => x.CreationDate);
                else
                    query = query.OrderByDescending(x => x.CreationDate);
            }
            else
            {
                query = query.OrderByDescending(x => x.CreationDate);
            }

            return query.Skip(from).Take(total).ToList();
        }
        

        #region Helper
        private IQueryable<NccPost> GetBaseQuery(bool isActive, bool isPublished, bool withSticky, bool withFeatured, DateTime? dateFrom = null, DateTime? dateTo = null, long categoryId = 0, long tagId = 0, long createBy = 0, string keyword = "")
        {
            var baseQuery = Query()
                .Include("PostDetails")
                .Include("Author")
                .Include("Comments")
                .Include("Categories")
                .Include("Categories.Category")
                .Include("Categories.Category.CategoryDetails")
                .Include("Tags")
                .Include("Tags.Tag")
                .Where(x => x.Status != EntityStatus.Deleted);
            if (isActive == true)
            {
                baseQuery = baseQuery.Where(x => x.Status == EntityStatus.Active);
            }
            if (isPublished == true)
            {
                baseQuery = baseQuery.Where(x => x.PostStatus == NccPost.NccPostStatus.Published && x.PublishDate <= DateTime.Now);
            }
            if (dateFrom != null && dateTo != null)
            {
                dateTo = dateTo?.Date.AddDays(1).AddMinutes(-1);
                baseQuery = baseQuery.Where(x => x.PublishDate >= dateFrom && x.PublishDate <= dateTo);
            }
            if (withSticky == false)
            {
                baseQuery = baseQuery.Where(x => x.IsStiky == withSticky);
            }
            if (withFeatured == false)
            {
                baseQuery = baseQuery.Where(x => x.IsFeatured == withFeatured);
            }
            if (categoryId > 0)
            {
                baseQuery = baseQuery.Where(x => x.Categories.Any(y => y.CategoryId == categoryId));
            }
            if (tagId > 0)
            {
                baseQuery = baseQuery.Where(x => x.Tags.Any(y => y.TagId == tagId));
            }
            if (createBy > 0)
            {
                baseQuery = baseQuery.Where(x => x.CreateBy == createBy);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                baseQuery = baseQuery.Where(x => x.PostDetails.Any(y => y.Title.Contains(keyword)));
            }
            return baseQuery;
        }
        #endregion
    }
}