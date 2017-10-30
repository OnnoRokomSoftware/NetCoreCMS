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

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccUserAuthPolicyRepository : BaseRepository<NccUserAuthPolicy, long>
    {
        public NccUserAuthPolicyRepository(NccDbContext context) : base(context)
        {
        } 
    }
}