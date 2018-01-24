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
using NetCoreCMS.Framework.Core.Mvc.Service;
using NetCoreCMS.Framework.Core.Repository;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccScheduleTaskHistoryService : BaseService<NccScheduleTaskHistory>
    {
        private readonly NccScheduleTaskHistoryRepository _entityRepository;
         
        public NccScheduleTaskHistoryService(NccScheduleTaskHistoryRepository entityRepository) : base(entityRepository)
        {
            _entityRepository = entityRepository;
        }

    }
}
