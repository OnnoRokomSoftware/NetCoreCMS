/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using System.Security.Claims;
using System;

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
