/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.News.Models;

namespace NetCoreCMS.Modules.News.Repository
{    
    public class NeCategoryRepository : BaseRepository<NeCategory, long>
    {
        public NeCategoryRepository(NccDbContext context) : base(context)
        {
        }
    }
}
