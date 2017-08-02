using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.MatGallery.Models;

namespace NetCoreCMS.MatGallery.Repository
{
    public class NccUserModuleRepository : BaseRepository<NccUserModule, long>
    {
        public NccUserModuleRepository(NccDbContext context) : base(context)
        {
        }
    }

    public class NccUserModuleLogRepository : BaseRepository<NccUserModuleLog, long>
    {
        public NccUserModuleLogRepository(NccDbContext context) : base(context)
        {
        }
    }
}
