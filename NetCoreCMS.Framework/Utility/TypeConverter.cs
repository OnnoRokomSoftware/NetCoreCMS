using Microsoft.EntityFrameworkCore.Storage;
using NetCoreCMS.Framework.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Utility
{
    public class TypeConverter
    {
        public static int TryParseInt(string input)
        {
            int value = 0;
            int.TryParse(input, out value);
            return value;
        }

        public static DatabaseEngine TryParseDatabaseEnum(string input)
        {
            DatabaseEngine dbEnum;
            Enum.TryParse<DatabaseEngine>(input, out dbEnum);
            return dbEnum;
        }
    }
}
