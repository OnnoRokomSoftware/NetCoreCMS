/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Repository;
using NetCoreCMS.Framework.Core.Mvc.Service;

namespace NetCoreCMS.Framework.Core.Services
{
    /// <summary>
    /// Service for user permission details. 
    /// </summary>
    public class NccPermissionDetailsService : BaseService<NccPermissionDetails>
    {
        private readonly NccPermissionDetailsRepository _entityRepository;

        public NccPermissionDetailsService(NccPermissionDetailsRepository entityRepository) : base(entityRepository)
        {
            _entityRepository = entityRepository;
        }
    }
}