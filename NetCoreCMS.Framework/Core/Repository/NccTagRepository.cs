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
using NetCoreCMS.Framework.Core.Models.ViewModels;
using NetCoreCMS.Framework.Core.Mvc.Models;

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

        public List<TagCloudItemViewModel> LoadTagCloud()
        {
            NccDbQueryText query = new NccDbQueryText();
            query.MySql_QueryText = @"SELECT nt.Name, COUNT(*) TotalPost
                                        FROM ncc_tag AS nt
                                        INNER JOIN ncc_post_tag AS npt ON npt.TagId= nt.Id
                                        INNER JOIN ncc_post AS np ON np.Id = npt.Postid
                                        WHERE np.PublishDate<=CURRENT_TIME()
	                                        AND np.PostStatus = " + ((int)NccPost.NccPostStatus.Published).ToString() + @"
                                        GROUP BY nt.Id
                                        ORDER BY nt.Name ASC ";
            return ExecuteSqlQuery<TagCloudItemViewModel>(query).ToList();
        }

        /// <summary>
        /// Use this function to count tags
        /// </summary>
        /// <param name="isActive">Load active records</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <returns></returns>
        public long Count(bool isActive, string keyword = "")
        {
            return GetBaseQuery(isActive, keyword).Count();
        }

        /// <summary>
        /// Use this function to lead tags
        /// </summary>
        /// <param name="from">Starting index</param>
        /// <param name="total">Total record you want</param>
        /// <param name="isActive">Load active records</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <param name="orderBy">Order by field name</param>
        /// <param name="orderDir">Order by (asc / desc)</param>
        /// <returns></returns>
        public List<NccTag> Load(int from, int total, bool isActive, string keyword = "", string orderBy = "", string orderDir = "")
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
        private IQueryable<NccTag> GetBaseQuery(bool isActive, string keyword)
        {
            var baseQuery = Query().Include("Posts").Where(x => x.Status != EntityStatus.Deleted);

            if (isActive == true)
            {
                baseQuery = baseQuery.Where(x => x.Status == EntityStatus.Active);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                baseQuery = baseQuery.Where(x => x.Name.Contains(keyword));
            }
            return baseQuery;
        }
        #endregion
    }
}