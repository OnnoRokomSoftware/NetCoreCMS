using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.StudentBoardResult.Models;

namespace NetCoreCMS.Modules.StudentBoardResult.Repository
{    
    public class SbrExamSubjectRepository : BaseRepository<SbrExamSubject, long>
    {
        public SbrExamSubjectRepository(NccDbContext context) : base(context)
        {
        }
    }
}
