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
using Microsoft.EntityFrameworkCore.Query;

using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccUserRepository : BaseRepository<NccUser, long>
    {
        public NccUserRepository(NccDbContext context) : base(context)
        {

        }

        public List<NccUser> Search(string searchKey)
        {
            return Query()
                .Include("Permissions")
                .Include("ExtraPermissions")
                .Include("ExtraDenies")
                .Include("Permissions.Permission")
                .Include("Permissions.User")
                .Where(
                        x => x.Email.Contains(searchKey) 
                        || x.FullName.Contains(searchKey) 
                        || x.Mobile.Contains(searchKey) 
                        || x.Name.Contains(searchKey)
                        || x.NormalizedUserName.Contains(searchKey)
                        || x.PhoneNumber.Contains(searchKey)
                        || x.UserName.Contains(searchKey)
                    )
                .ToList();
        }

        public void RemoveUserPermission(List<NccUserPermission> items)
        {
            Context.RemoveRange(items);
        }
    }
}
