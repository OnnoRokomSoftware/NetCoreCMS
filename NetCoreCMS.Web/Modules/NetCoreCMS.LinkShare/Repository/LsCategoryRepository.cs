using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.LinkShare.Models;

namespace NetCoreCMS.LinkShare.Repository
{    
    public class LsCategoryRepository : BaseRepository<LsCategory, long>
    {
        public LsCategoryRepository(NccDbContext context) : base(context)
        {
        }
    }
}
