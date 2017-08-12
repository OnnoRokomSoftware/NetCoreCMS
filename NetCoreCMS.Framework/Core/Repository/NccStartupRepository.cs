using System;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using System.Linq;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccStartupRepository : BaseRepository<NccStartup, long>
    {
        public NccStartupRepository(NccDbContext context) : base(context)
        {
        }        
    }
}
