using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.StudentBoardResult.Models;

namespace NetCoreCMS.Modules.StudentBoardResult.Repository
{    
    public class SbrStudentResultDetailsRepository : BaseRepository<SbrStudentResultDetails, long>
    {
        public SbrStudentResultDetailsRepository(NccDbContext context) : base(context)
        {
        }
    }
}
