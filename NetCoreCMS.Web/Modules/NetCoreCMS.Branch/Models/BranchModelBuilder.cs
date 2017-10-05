/*
 * Author: OnnoRokom Software Ltd
 * Website: http://onnorokomsoftware.com
 * Copyright (c) onnorokomsoftware.com
 * License: Commercial
*/
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Branch.Models;

namespace NetCoreCMS.Modules.Branch.Models
{
    public class BranchModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NccBranch>().ToTable("Ncc_Branch");                       
        }
    }
}
