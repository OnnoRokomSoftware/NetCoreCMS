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

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccPageRepository : BaseRepository<NccPage, long>
    {
        public NccPageRepository(NccDbContext context) : base(context)
        {
        }

        public List<NccPage> LoadRecentPages(int count)
        {
            var list = Query()
                .Where(x=>x.PageStatus == NccPage.NccPageStatus.Published)
                .OrderByDescending(x => x.CreationDate)
                .Take(count)
                .ToList();
            return list;
        }
    }
}
