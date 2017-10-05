using System;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using System.Linq;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccScheduleTaskHistoryRepository : BaseRepository<NccScheduleTaskHistory, long>
    {
        public NccScheduleTaskHistoryRepository(NccDbContext context) : base(context)
        {
        } 
    }
}
