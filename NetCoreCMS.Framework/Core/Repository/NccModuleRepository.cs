using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccModuleRepository : BaseRepository<NccModule, long>
    {
        public NccModuleRepository(NccDbContext context) : base(context)
        {

        }
    }
}
