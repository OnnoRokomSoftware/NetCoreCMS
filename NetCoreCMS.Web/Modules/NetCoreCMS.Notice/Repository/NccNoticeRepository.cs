using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Notice.Models;

namespace NetCoreCMS.Notice.Repository
{
    public class NccNoticeRepository : BaseRepository<NccNotice, long>
    {
        public NccNoticeRepository(NccDbContext context) : base(context)
        {
        }
    }
}
