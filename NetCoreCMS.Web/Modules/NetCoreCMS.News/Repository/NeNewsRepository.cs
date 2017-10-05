using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.News.Models;

namespace NetCoreCMS.Modules.News.Repository
{
    public class NeNewsRepository : BaseRepository<NeNews, long>
    {
        public NeNewsRepository(NccDbContext context) : base(context)
        {
        }
    }
}
