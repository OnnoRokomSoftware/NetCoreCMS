using NetCoreCMS.Framework.Core.IoC;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.HelloWorld.Services
{
    public class HelloService : IBaseService<BaseModel>, ISingleton
    {
        public void DeletePermanently(long entityId)
        {
            throw new NotImplementedException();
        }

        public BaseModel Get(long entityId, bool isAsNoTracking = false)
        {
            throw new NotImplementedException();
        }

        public List<BaseModel> LoadAll(bool isActive = true, int status = 0, string name = "", bool isLikeSearch = false)
        {
            throw new NotImplementedException();
        }

        public void Remove(long entityId)
        {
            throw new NotImplementedException();
        }

        public BaseModel Save(BaseModel model)
        {
            throw new NotImplementedException();
        }

        public BaseModel Update(BaseModel model)
        {
            throw new NotImplementedException();
        }
    }
}
