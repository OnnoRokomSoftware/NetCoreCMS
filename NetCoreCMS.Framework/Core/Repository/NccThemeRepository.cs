using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccThemeRepository : BaseRepository<NccTheme, long>
    {
        public NccThemeRepository(NccDbContext context) : base(context)
        {
        }
    }
}
