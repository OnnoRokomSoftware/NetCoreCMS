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
    public class NccCommentsRepository : BaseRepository<NccComment, long>
    {
        public NccCommentsRepository(NccDbContext context) : base(context)
        {

        }

        public List<NccComment> LoadApproved(long postId, int page, int count)
        {
            var list = Query()
                .Where(x => x.Post.Id == postId && x.CommentStatus == NccComment.NccCommentStatus.Approved)
                .OrderByDescending(x => x.CreationDate)
                .Skip(page * count)
                .Take(count)
                .ToList();
            return list;
        }

        public List<NccComment> LoadApproved(int page, int count)
        {
            var list = Query()
                .Where(x => x.CommentStatus == NccComment.NccCommentStatus.Approved)
                .OrderByDescending(x => x.CreationDate)
                .Skip(page * count)
                .Take(count)
                .ToList();
            return list;
        }

        public List<NccComment> Load(long postId, int count)
        {
            var list = Query().Include("Post").Include("Author")
                .Where(x => x.Post.Id == postId && x.CommentStatus == NccComment.NccCommentStatus.Approved)
                .OrderByDescending(x => x.CreationDate)
                .Take(count)
                .ToList();

            if (count < 0)
            {
                list = Query().Include("Post").Include("Author")
                .Where(x => x.Post.Id == postId && x.CommentStatus == NccComment.NccCommentStatus.Approved)
                .OrderByDescending(x => x.CreationDate)
                .ToList();
            }
            return list;
        }

        public List<NccComment> LoadRecentComments(int count)
        {
            var list = Query().Include("Post").Include("Post.PostDetails").Include("Author")
                .Where(x => x.CommentStatus == NccComment.NccCommentStatus.Approved)
                .OrderByDescending(x => x.CreationDate)
                .Take(count)
                .ToList();
            return list;
        }

        /// <summary>
        /// Use this function to count post
        /// </summary>
        /// <param name="isActive">Load active records</param>
        /// <param name="createBy">To load by user</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <returns></returns>
        public long Count(bool isActive, long createBy = 0, string keyword = "")
        {
            return GetBaseQuery(isActive, createBy, keyword).Count();
        }

        /// <summary>
        /// Use this function to lead post
        /// </summary>
        /// <param name="from">Starting index</param>
        /// <param name="total">Total record you want</param>
        /// <param name="isActive">Load active records</param>
        /// <param name="createBy">To load by user</param>
        /// <param name="keyword">To load by keyword(search in title)</param>
        /// <param name="orderBy">Order by field name</param>
        /// <param name="orderDir">Order by (asc / desc)</param>
        /// <returns></returns>
        public List<NccComment> Load(int from, int total, bool isActive, long createBy = 0, string keyword = "", string orderBy = "", string orderDir = "")
        {
            var query = GetBaseQuery(isActive, createBy, keyword);
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
        private IQueryable<NccComment> GetBaseQuery(bool isActive, long createBy = 0, string keyword = "")
        {
            var baseQuery = Query().Include("Post").Include("Author")
                .Where(x => x.Status != EntityStatus.Deleted);

            if (isActive == true)
            {
                baseQuery = baseQuery.Where(x => x.Status == EntityStatus.Active);
            }
            if (createBy > 0)
            {
                baseQuery = baseQuery.Where(x => x.Post.CreateBy == createBy);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                baseQuery = baseQuery.Where(x => x.AuthorName.Contains(keyword) || x.Content.Contains(keyword));
            }
            return baseQuery;
        }
        #endregion
    }
}