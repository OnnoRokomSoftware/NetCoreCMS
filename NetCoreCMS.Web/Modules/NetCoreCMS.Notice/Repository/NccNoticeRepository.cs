using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.HelloWorld.Models;

namespace NetCoreCMS.HelloWorld.Repository
{
    public class NccNoticeRepository : BaseRepository<NccNotice, long>
    {
        public NccNoticeRepository(NccDbContext context) : base(context)
        {
        }
    }
}
