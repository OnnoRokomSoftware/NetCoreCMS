using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.Analytics.Models;

namespace NetCoreCMS.Modules.Analytics.Repository
{    
    public class AnalyticsLogRepository : BaseRepository<AnalyticsLogModel, long>
    {
        public AnalyticsLogRepository(NccDbContext context) : base(context)
        {
        }
    }
}
