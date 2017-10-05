using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.StudentBoardResult.Models;

namespace NetCoreCMS.Modules.StudentBoardResult.Repository
{    
    public class SbrStudentRepository : BaseRepository<SbrStudent, long>
    {
        public SbrStudentRepository(NccDbContext context) : base(context)
        {
        }
    }
}
