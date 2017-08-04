using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.LinkShare.Models;

namespace NetCoreCMS.LinkShare.Repository
{
    public class NccLinkShareRepository : BaseRepository<NccLinkShare, long>
    {
        public NccLinkShareRepository(NccDbContext context) : base(context)
        {
        }
    }
}
