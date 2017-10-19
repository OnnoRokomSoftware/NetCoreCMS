/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccPostDetailsRepository : BaseRepository<NccPostDetails, long>
    {
        public NccPostDetailsRepository(NccDbContext context) : base(context)
        {
        }

        public List<NccPostDetails> LoadRecentPosts(int count)
        {
            var list = Query()
                .Include("Post")
                .Where(x => x.Post.PostStatus == NccPost.NccPostStatus.Published)
                .OrderByDescending(x => x.Post.PublishDate)
                .Take(count)
                .ToList();
            return list;
        }

        public NccPostDetails Get(string slug, string language = "")
        {
            var query = Query().Include("Post").Include("Post.PostDetails").Include("Post.Categories").Include("Post.Tags").Include("Post.Comments").Where(x => x.Slug == slug);
            if (!string.IsNullOrEmpty(language))
            {
                query = query.Where(x => x.Language == language);
            }
            return query.FirstOrDefault();
        }

        public List<NccPostDetails> Search(string name, string language, int count = 20)
        {

            var query = Query().Include("Post").Where(x => x.Name.Contains(name) || x.Title.Contains(name) || x.Slug.Contains(name) || x.Post.Name.Contains(name));
            if (!string.IsNullOrEmpty(language))
            {
                query = query.Where(x => x.Language == language);
            }
            return query.Take(count).ToList();
        }

        public List<NccPostDetails> LoadRecentPostDetails(int count, string language)
        {
            var query = Query().Include("Post").Include("Post.PostDetails").Where(x => x.Post.PostStatus == NccPost.NccPostStatus.Published);
            if (!string.IsNullOrEmpty(language))
            {
                query = query.Where(x => x.Language == language);
            }
            return query.OrderByDescending(x => x.Post.PublishDate).Take(count).ToList();
        }
    }
}