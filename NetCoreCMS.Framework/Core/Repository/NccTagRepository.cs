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
    }
}