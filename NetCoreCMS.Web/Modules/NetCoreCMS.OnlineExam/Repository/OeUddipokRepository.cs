using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.OnlineExam.Models;

namespace NetCoreCMS.Modules.OnlineExam.Repository
{
    public class OeUddipokRepository : BaseRepository<OeUddipok, long>
    {
        public OeUddipokRepository(NccDbContext context) : base(context)
        {
        }
    }
}
