using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.OnlineExam.Models;

namespace NetCoreCMS.Modules.OnlineExam.Repository
{
    public class OeExamRepository : BaseRepository<OeExam, long>
    {
        public OeExamRepository(NccDbContext context) : base(context)
        {
        }
    }
}
