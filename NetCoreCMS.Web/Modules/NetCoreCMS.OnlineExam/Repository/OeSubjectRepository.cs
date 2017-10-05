using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.OnlineExam.Models;

namespace NetCoreCMS.Modules.OnlineExam.Repository
{    
    public class OeSubjectRepository : BaseRepository<OeSubject, long>
    {
        public OeSubjectRepository(NccDbContext context) : base(context)
        {
        }
    }
}
