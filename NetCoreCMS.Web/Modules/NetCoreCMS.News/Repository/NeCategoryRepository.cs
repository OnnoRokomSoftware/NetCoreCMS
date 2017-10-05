using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.News.Models;

namespace NetCoreCMS.Modules.News.Repository
{    
    public class NeCategoryRepository : BaseRepository<NeCategory, long>
    {
        public NeCategoryRepository(NccDbContext context) : base(context)
        {
        }
    }
}
