using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Framework.Core.Data
{
    public enum Databases
    {
        SqLite,
        MySql,
        MsSqlLocalStorage,
        MsSql,
        PgSql,
        InMemory
    }
}
