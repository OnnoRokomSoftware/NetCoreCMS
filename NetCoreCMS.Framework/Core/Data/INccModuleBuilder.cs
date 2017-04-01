using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Data
{
    public interface INccModuleBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}
