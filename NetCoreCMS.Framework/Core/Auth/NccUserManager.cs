/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Identity;
using NetCoreCMS.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NetCoreCMS.Framework.Core.Auth
{
    public class NccUserManager : UserManager<NccUser>
    {
        public NccUserManager(IUserStore<NccUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<NccUser> passwordHasher, IEnumerable<IUserValidator<NccUser>> userValidators, IEnumerable<IPasswordValidator<NccUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<NccUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
