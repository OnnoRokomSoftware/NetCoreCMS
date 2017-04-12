using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccMenuRepository : BaseRepository<NccMenu, long>
    {
        public NccMenuRepository(NccDbContext context) : base(context)
        {
        }
    }
}
