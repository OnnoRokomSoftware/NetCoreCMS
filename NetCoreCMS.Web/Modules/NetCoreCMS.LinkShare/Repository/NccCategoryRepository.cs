using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.LinkShare.Models;

namespace NetCoreCMS.LinkShare.Repository
{    
    public class NccCategoryRepository : BaseRepository<NccCategory, long>
    {
        public NccCategoryRepository(NccDbContext context) : base(context)
        {
        }
    }
}
