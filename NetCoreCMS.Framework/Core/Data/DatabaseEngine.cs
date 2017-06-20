/*
 * Author: TecRT
 * Website: http://tecrt.com
 * Copyright (c) tecrt.com
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
        MSSQL,
        PgSql
    }
}
