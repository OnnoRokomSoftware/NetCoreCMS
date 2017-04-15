using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccPageRepository : BaseRepository<NccPage, long>
    {
        public NccPageRepository(NccDbContext context) : base(context)
        {
        }
    }
}
