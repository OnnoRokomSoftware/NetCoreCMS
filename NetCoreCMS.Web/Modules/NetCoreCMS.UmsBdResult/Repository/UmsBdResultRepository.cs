/*
 * Author: OnnoRokom Software Ltd
 * Website: http://onnorokomsoftware.com
 * Copyright (c) onnorokomsoftware.com
 * License: Commercial
*/
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.UmsBdResult.Models;

namespace NetCoreCMS.UmsBdResult.Repository
{
    public class UmsBdResultRepository : BaseRepository<UmsBdResultSettings, long>
    {
        public UmsBdResultRepository(NccDbContext context) : base(context)
        {
        }
    }
}
