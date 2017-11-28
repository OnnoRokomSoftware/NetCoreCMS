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
    /// <summary>
    /// Convert one type into another type
    /// </summary>
    public class TypeConverter
    {
        /// <summary>
        /// Parse safly integer value from string
        /// </summary>
        /// <param name="input">string like "100"</param>
        /// <returns>int value like 100</returns>
        public static int TryParseInt(string input)
        {
            int value = 0;
            int.TryParse(input, out value);
            return value;
        }

        /// <summary>
        /// From enum text convert into enum
        /// </summary>
        /// <param name="input">enum field name string</param>
        /// <returns>enum object</returns>
        public static SupportedDatabases TryParseDatabaseEnum(string input)
        {
            SupportedDatabases dbEnum;
            Enum.TryParse<SupportedDatabases>(input, out dbEnum);
            return dbEnum;
        }

        /// <summary>
        /// Convert Menu Action Type name text into enum
        /// </summary>
        /// <param name="input">string action type name</param>
        /// <returns>ActionType enum</returns>
        public static NccMenuItem.ActionType TryParseActionTypeEnum(string input)
        {
            NccMenuItem.ActionType atEnum;
            Enum.TryParse<NccMenuItem.ActionType>(input, out atEnum);
            return atEnum;
        }

        /// <summary>
        /// Convert LogLevel Type name text into enum
        /// </summary>
        /// <param name="input">string LogLevel type name</param>
        /// <returns>ActionType enum</returns>
        public static LogLevel TryParseLogLevel(int loggingLevel)
        {
            LogLevel logLevel;
            Enum.TryParse<LogLevel>(loggingLevel.ToString(), out logLevel);
            return logLevel;
        }
    }
}
