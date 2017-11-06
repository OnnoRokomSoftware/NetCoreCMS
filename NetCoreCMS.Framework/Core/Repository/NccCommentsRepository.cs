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
                .Where(x => x.Post.Id == postId && x.CommentStatus == NccComment.StatusEnum.Approved)
                .OrderByDescending(x => x.CreationDate)
                .Skip(page * count)
                .Take(count)
                .ToList();
            return list;
        }

        public List<NccComment> LoadApproved(int page, int count)
        {
            var list = Query()
                .Where(x => x.CommentStatus == NccComment.StatusEnum.Approved)
                .OrderByDescending(x => x.CreationDate)
                .Skip(page * count)
                .Take(count)
                .ToList();
            return list;
        }
    }
}