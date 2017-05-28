using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccWebSiteWidgetRepository : BaseRepository<NccWebSiteWidget, long>
    {
        public NccWebSiteWidgetRepository(NccDbContext context) : base(context)
        {
        }
        
    }
}
