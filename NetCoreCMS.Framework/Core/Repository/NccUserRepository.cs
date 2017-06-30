using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccUserRepository : BaseRepository<NccUser, long>
    {
        public NccUserRepository(NccDbContext context) : base(context)
        {
        }
    }
}
