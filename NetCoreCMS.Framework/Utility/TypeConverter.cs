/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using NetCoreCMS.Framework.Core.Models;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Data;

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

        public static NccMenuItem.ActionType TryParseActionTypeEnum(string input)
        {
            NccMenuItem.ActionType atEnum;
            Enum.TryParse<NccMenuItem.ActionType>(input, out atEnum);
            return atEnum;
        }

        public static LogLevel TryParseLogLevel(int loggingLevel)
        {
            LogLevel logLevel;
            Enum.TryParse<LogLevel>(loggingLevel.ToString(), out logLevel);
            return logLevel;
        }
    }
}
