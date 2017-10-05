using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.MatGallery.Models;

namespace NetCoreCMS.MatGallery.Repository
{
    public class NccUserThemeRepository : BaseRepository<NccUserTheme, long>
    {
        public NccUserThemeRepository(NccDbContext context) : base(context)
        {
        }
    }

    public class NccUserThemeLogRepository : BaseRepository<NccUserThemeLog, long>
    {
        public NccUserThemeLogRepository(NccDbContext context) : base(context)
        {
        }
    }
}
