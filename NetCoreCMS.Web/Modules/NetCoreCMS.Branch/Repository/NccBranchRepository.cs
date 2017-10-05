/*
 * Author: OnnoRokom Software Ltd
 * Website: http://onnorokomsoftware.com
 * Copyright (c) onnorokomsoftware.com
 * License: Commercial
*/
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Branch.Models;

namespace NetCoreCMS.Branch.Repository
{
    public class NccBranchRepository : BaseRepository<NccBranch, long>
    {
        public NccBranchRepository(NccDbContext context) : base(context)
        {
        }
    }
}
