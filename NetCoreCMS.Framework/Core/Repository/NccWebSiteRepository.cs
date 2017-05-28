using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccWebSiteRepository : BaseRepository<NccWebSite, long>
    {
        public NccWebSiteRepository(NccDbContext context) : base(context)
        {
        }
    }
}
