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

        public List<NccPost> LoadPublished(int from, int total, bool withStiky, bool withFeatured, DateTime? dateFrom = null, DateTime? dateTo = null)
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

            if (dateFrom != null && dateTo != null)
            {
                query = Query()
                .Include("PostDetails")
                .Include("Author")
                .Include("Comments")
                .Include("Categories")
                .Include("Categories.Category")
                .Include("Categories.Category.CategoryDetails")
                .Include("Tags")
                .Include("Tags.Tag")
                .Where(x => x.PostStatus == NccPost.NccPostStatus.Published && x.PublishDate <= DateTime.Now && x.PublishDate >= dateFrom && x.PublishDate <= dateTo);
            }

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

        public long GetCount(NccPost.NccPostStatus status, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            if (dateFrom != null && dateTo != null)
            {
                return Query().Where(x => x.PostStatus == status && x.Status == EntityStatus.Active && x.PublishDate >= dateFrom && x.PublishDate <= dateTo).Count();
            }

            return Query().Where(x => x.PostStatus == status && x.Status == EntityStatus.Active).Count();
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
    }
}