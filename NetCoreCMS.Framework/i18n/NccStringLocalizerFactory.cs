/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using NetCoreCMS.Framework.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.i18n
{ 
    public class NccStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizerFactory _factory;
        public NccStringLocalizerFactory(IStringLocalizerFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            _factory = factory;
            _httpContextAccessor = httpContextAccessor;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new NccStringLocalizer<SharedResource>(_factory,_httpContextAccessor);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new NccStringLocalizer<SharedResource>(_factory, _httpContextAccessor);
        }
    }
}
