/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using NetCoreCMS.Framework.Core.IoC;
using NetCoreCMS.Framework.Core.Mvc.Services;
using System.Collections.Generic;
using NetCoreCMS.HelloWorld.Models.Entity;

namespace NetCoreCMS.HelloWorld.Services
{
    public class HelloService : IBaseService<HelloModel>, ISingleton
    {
        public void DeletePermanently(long entityId)
        {
            throw new NotImplementedException();
        }

        public HelloModel Get(long entityId, bool isAsNoTracking = false)
        {
            throw new NotImplementedException();
        }

        public List<HelloModel> LoadAll(bool isActive = true, int status = 0, string name = "", bool isLikeSearch = false)
        {
            throw new NotImplementedException();
        }

        public void Remove(long entityId)
        {
            throw new NotImplementedException();
        }

        public HelloModel Save(HelloModel model)
        {
            throw new NotImplementedException();
        }

        public HelloModel Update(HelloModel model)
        {
            throw new NotImplementedException();
        }
    }
}
