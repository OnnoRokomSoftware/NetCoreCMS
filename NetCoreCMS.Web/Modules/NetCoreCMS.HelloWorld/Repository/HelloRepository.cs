using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.HelloWorld.Models;
using System;
using System.Collections.Generic;
using System.Text;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.IoC;

namespace NetCoreCMS.HelloWorld.Repository
{
    public class HelloRepository : BaseRepository<HelloModel, long>, ISingleton
    {
        public HelloRepository(NccDbContext context) : base(context)
        {
        }
    }
}
