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
using NetCoreCMS.Framework.Core.Mvc.Repository;
using System.Linq;

using NetCoreCMS.Framework.Core.IoC;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccPermissionRepository : BaseRepository<NccPermission, long>
    {
        public NccPermissionRepository(NccDbContext context) : base(context)
        {
        }

        public int RemoveById(List<long> removePermissionDetailsIdList)
        {
            var idList = string.Join(",", removePermissionDetailsIdList);
            var count = ExecuteSqlCommand(new NccDbQueryText() {
                MySql_QueryText = $"delete from Ncc_Permission_Details where Id in ({idList})"
            });
            return count;
        }
    }
}