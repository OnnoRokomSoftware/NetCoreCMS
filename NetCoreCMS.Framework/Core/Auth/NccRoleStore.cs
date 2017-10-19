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
using System.Security.Claims;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NetCoreCMS.Framework.Core.Auth
{
    public class NccRoleStore : RoleStore<NccRole, NccDbContext, long, NccUserRole, IdentityRoleClaim<long>>
    {
        public NccRoleStore(NccDbContext context) : base(context)
        {
        }

        protected override IdentityRoleClaim<long> CreateRoleClaim(NccRole role, Claim claim)
        {
            return new IdentityRoleClaim<long> { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value };
        }

        internal object FindByNameAsync(object reader)
        {
            throw new NotImplementedException();
        }
    }
}
