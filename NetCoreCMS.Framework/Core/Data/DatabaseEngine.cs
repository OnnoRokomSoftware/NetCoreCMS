/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Framework.Core.Data
{
    public enum DatabaseEngine
    {
        SqLite,
        MsSqlLocalStorage,
        MySql,        
        MsSql,
        PgSql
    }
}
