using System;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccSettingsRepository : BaseRepository<NccSettings, long>
    {
        public NccSettingsRepository(NccDbContext context) : base(context)
        {
        }        
    }
}
