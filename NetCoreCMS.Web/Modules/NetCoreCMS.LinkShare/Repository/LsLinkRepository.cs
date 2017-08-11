using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.LinkShare.Models;

namespace NetCoreCMS.LinkShare.Repository
{
    public class LsLinkRepository : BaseRepository<LsLink, long>
    {
        public LsLinkRepository(NccDbContext context) : base(context)
        {
        }
    }
}
