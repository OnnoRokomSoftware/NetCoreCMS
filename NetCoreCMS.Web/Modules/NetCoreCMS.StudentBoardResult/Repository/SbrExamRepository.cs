using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.StudentBoardResult.Models;

namespace NetCoreCMS.Modules.StudentBoardResult.Repository
{    
    public class SbrExamRepository : BaseRepository<SbrExam, long>
    {
        public SbrExamRepository(NccDbContext context) : base(context)
        {
        }
    }
}
