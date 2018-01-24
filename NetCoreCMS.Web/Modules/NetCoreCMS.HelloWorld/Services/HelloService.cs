/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.IoC;
using NetCoreCMS.HelloWorld.Models.Entity;
using NetCoreCMS.Framework.Core.Mvc.Service;
using NetCoreCMS.HelloWorld.Repositories;

namespace NetCoreCMS.HelloWorld.Services
{
    public class HelloService : BaseService<HelloModel>, ISingleton
    {
        public HelloService(HelloRepository entityRepository) : base(entityRepository)
        {

        }
    }
}
