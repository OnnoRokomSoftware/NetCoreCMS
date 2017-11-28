/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.IoC;
using NetCoreCMS.HelloWorld.Models.Entity;

namespace NetCoreCMS.HelloWorld.Repository
{
    public class HelloRepository : BaseRepository<HelloModel, long>, ISingleton
    {
        public HelloRepository(NccDbContext context) : base(context)
        {
        }
    }
}
