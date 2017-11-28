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
    public class NccPermissionDetailsRepository : BaseRepository<NccPermissionDetails, long>
    {
        public NccPermissionDetailsRepository(NccDbContext context) : base(context)
        {
        } 
    }
}