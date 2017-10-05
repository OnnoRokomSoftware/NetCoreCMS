using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.StudentBoardResult.Models;

namespace NetCoreCMS.Modules.StudentBoardResult.Repository
{    
    public class SbrBoardRepository : BaseRepository<SbrBoard, long>
    {
        public SbrBoardRepository(NccDbContext context) : base(context)
        {
        }
    }
}
