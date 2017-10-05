using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccWebSiteInfoRepository : BaseRepository<NccWebSiteInfo, long>
    {
        public NccWebSiteInfoRepository(NccDbContext context) : base(context)
        {
        }
    }
}
