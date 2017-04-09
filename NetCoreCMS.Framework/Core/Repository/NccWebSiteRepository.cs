using System;
using System.Collections.Generic;
using System.Text;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccWebSiteRepository : BaseRepository<NccWebSite, long>
    {
        public NccWebSiteRepository(NccDbContext context) : base(context)
        {
        }
    }
}
