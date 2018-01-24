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
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NetCoreCMS.Framework.Core.Models.ViewModels;
using NetCoreCMS.Framework.Core.Mvc.Service;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccModuleService : BaseService<NccModule>
    {
        private readonly NccModuleRepository _entityRepository;

        public NccModuleService(NccModuleRepository entityRepository) : base(entityRepository, new List<string>() { "Dependencies" })
        {
            _entityRepository = entityRepository;
        }

        internal NccModule GetByModuleName(string moduleName)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.ModuleName == moduleName);
        }

        public List<NccModule> LoadByModuleStatus(NccModule.NccModuleStatus active)
        {
            return _entityRepository.LoadByModuleStatus(active);
        }

        /// <summary>
        /// Execure a raw SQL query
        /// </summary>
        /// <param name="query">Raw SQL query</param>
        /// <returns></returns>
        public string ExecuteQuery(NccDbQueryText query)
        {
            string retVal = "";
            using (var txn = _entityRepository.BeginTransaction())
            {
                try
                {
                    var ret = _entityRepository.ExecuteSqlCommand(query);
                    retVal = ret.ToString();
                    txn.Commit();
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                }
            }

            return retVal;
        }

        /// <summary>
        /// Generate Database table using Entity Model
        /// </summary>
        /// <param name="modelType">Entity Type [typeof(ModelT)]</param>
        /// <param name="DeleteUnusedColumns"></param>
        /// <returns></returns>
        public int CreateOrUpdateTable(Type modelType, bool DeleteUnusedColumns = true)
        {
            NccDbQueryText query = new NccDbQueryText();
            string currentTableName = GlobalContext.GetTableName(modelType);
            string assemblyname = modelType.Assembly.GetName().Name;
            var currentColumnList = GetTableColumns(modelType);
            if (currentColumnList.Count > 0)
            {
                if (currentColumnList.FirstOrDefault().ColumnComment == assemblyname)
                {
                    return UpdateTable(modelType, DeleteUnusedColumns);
                }

                throw new Exception($"{currentTableName} Table already exists in module {currentColumnList.FirstOrDefault().ColumnComment}.");
            }

            if (SetupHelper.SelectedDatabase == "SqLite")
            {

            }
            else if (SetupHelper.SelectedDatabase == "MSSQL")
            {

            }
            else if (SetupHelper.SelectedDatabase == "MySql")
            {
                bool primaryFound = false;
                List<string> columnList = new List<string>();
                string qStart = $"CREATE TABLE `{currentTableName}` ( ";//IF NOT EXISTS 
                string qPrimaryColumn = "";
                string qBody = "";
                string qPrimarykey = "";
                string qIndexkey = "";
                string qForeginkey = "";
                string qEnd = ") ENGINE = InnoDB DEFAULT CHARSET=ucs2 COLLATE=ucs2_unicode_ci;";
                var entityProperties = modelType.GetProperties().OrderBy(x => x.Name).ToArray();
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    var prop = entityProperties[i];
                    var propInfo = modelType.GetProperty(prop.Name);
                    //If property has NotMapped Attribute then skip column.
                    if (propInfo.GetCustomAttributes(true).Where(x => x is NotMappedAttribute).Count() > 0)
                    {
                        continue;
                    }
                    if (propInfo.PropertyType.Name.StartsWith("List"))
                    {
                        continue;
                    }
                    //If column already added then skip
                    if (columnList.Where(x => x == prop.Name).Count() > 0)
                    {
                        continue;
                    }
                    if (qBody.Trim() != "" && qBody.Trim().EndsWith(",") == false) qBody += ", ";

                    if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(Int16) || prop.PropertyType == typeof(int?) || prop.PropertyType == typeof(Int16?))
                    {
                        if (propInfo.GetCustomAttributes(true).Where(x => x is KeyAttribute).Count() > 0)
                        {
                            qPrimaryColumn = $"{Environment.NewLine}    `{prop.Name}` INT NOT NULL AUTO_INCREMENT COMMENT '{assemblyname}', ";
                            qPrimarykey += (qPrimarykey == "") ? $"`{prop.Name}`" : $", `{prop.Name}`";
                            primaryFound = true;
                        }
                        else
                        {
                            qBody += $"{Environment.NewLine}    `{prop.Name}` INT NOT NULL COMMENT '{assemblyname}' ";
                        }
                        columnList.Add(prop.Name);
                    }
                    else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long))
                    {
                        if (propInfo.GetCustomAttributes(true).Where(x => x is KeyAttribute).Count() > 0)
                        {
                            qPrimaryColumn = $"{Environment.NewLine}    `{prop.Name}` BIGINT NOT NULL AUTO_INCREMENT COMMENT '{assemblyname}', ";
                            qPrimarykey += (qPrimarykey == "") ? $"`{prop.Name}`" : $", `{prop.Name}`";
                            primaryFound = true;
                        }
                        else
                        {
                            qBody += $"{Environment.NewLine}    `{prop.Name}` BIGINT NOT NULL COMMENT '{assemblyname}' ";
                        }
                        columnList.Add(prop.Name);
                    }
                    else if (prop.PropertyType == typeof(float) || prop.PropertyType == typeof(float))
                    {
                        if (propInfo.GetCustomAttributes(true).Where(x => x is KeyAttribute).Count() > 0)
                        {
                            qPrimaryColumn = $"{Environment.NewLine}    `{prop.Name}` float DEFAULT '0' COMMENT '{assemblyname}', ";
                            qPrimarykey += (qPrimarykey == "") ? $"`{prop.Name}`" : $", `{prop.Name}`";
                            primaryFound = true;
                        }
                        else
                        {
                            qBody += $"{Environment.NewLine}    `{prop.Name}` float DEFAULT '0' COMMENT '{assemblyname}' ";
                        }
                        columnList.Add(prop.Name);
                    }
                    else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double))
                    {
                        if (propInfo.GetCustomAttributes(true).Where(x => x is KeyAttribute).Count() > 0)
                        {
                            qPrimaryColumn = $"{Environment.NewLine}    `{prop.Name}` double DEFAULT '0' COMMENT '{assemblyname}', ";
                            qPrimarykey += (qPrimarykey == "") ? $"`{prop.Name}`" : $", `{prop.Name}`";
                            primaryFound = true;
                        }
                        else
                        {
                            qBody += $"{Environment.NewLine}    `{prop.Name}` double DEFAULT '0' COMMENT '{assemblyname}' ";
                        }
                        columnList.Add(prop.Name);
                    }
                    else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal))
                    {
                        if (propInfo.GetCustomAttributes(true).Where(x => x is KeyAttribute).Count() > 0)
                        {
                            qPrimaryColumn = $"{Environment.NewLine}    `{prop.Name}` decimal(10,2) DEFAULT '0.00' COMMENT '{assemblyname}', ";
                            qPrimarykey += (qPrimarykey == "") ? $"`{prop.Name}`" : $", `{prop.Name}`";
                            primaryFound = true;
                        }
                        else
                        {
                            qBody += $"{Environment.NewLine}    `{prop.Name}` decimal(10,2) DEFAULT '0.00' COMMENT '{assemblyname}' ";
                        }
                        columnList.Add(prop.Name);
                    }
                    else if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(String))
                    {
                        if (propInfo.GetCustomAttributes(true).Where(x => x is KeyAttribute).Count() > 0)
                        {
                            if (propInfo.GetCustomAttributes(true).Where(x => x is MaxLengthAttribute).Count() > 0)
                            {
                                var temp = (MaxLengthAttribute)propInfo.GetCustomAttributes(true).Where(x => x is MaxLengthAttribute).FirstOrDefault();
                                long length = temp.Length;
                                if (length > 4000)
                                    qPrimaryColumn = $"{Environment.NewLine}    `{prop.Name}` longtext COLLATE utf8_unicode_ci NOT NULL COMMENT '{assemblyname}', ";
                                else
                                    qPrimaryColumn = $"{Environment.NewLine}    `{prop.Name}` VARCHAR({temp.Length}) COLLATE utf8_unicode_ci NOT NULL COMMENT '{assemblyname}', ";
                            }
                            else
                            {
                                qBody += $"{Environment.NewLine}    `{prop.Name}` VARCHAR(1024) COLLATE utf8_unicode_ci NOT NULL COMMENT '{assemblyname}' ";
                            }
                            qPrimarykey += (qPrimarykey == "") ? $"`{prop.Name}`" : $", `{prop.Name}`";
                            primaryFound = true;
                        }
                        else
                        {
                            if (propInfo.GetCustomAttributes(true).Where(x => x is MaxLengthAttribute).Count() > 0)
                            {
                                var temp = (MaxLengthAttribute)propInfo.GetCustomAttributes(true).Where(x => x is MaxLengthAttribute).FirstOrDefault();
                                long length = temp.Length;
                                if (length > 4000)
                                    qBody += $"{Environment.NewLine}    `{prop.Name}` longtext COLLATE utf8_unicode_ci NULL COMMENT '{assemblyname}' ";
                                else
                                    qBody += $"{Environment.NewLine}    `{prop.Name}` VARCHAR({temp.Length}) COLLATE utf8_unicode_ci NULL COMMENT '{assemblyname}' ";
                            }
                            else
                            {
                                qBody += $"{Environment.NewLine}    `{prop.Name}` VARCHAR(1024) COLLATE utf8_unicode_ci NULL COMMENT '{assemblyname}' ";
                            }
                        }
                        columnList.Add(prop.Name);
                    }
                    else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                    {
                        qBody += $"{Environment.NewLine}    `{prop.Name}` bit(1) NOT NULL COMMENT '{assemblyname}' ";
                        columnList.Add(prop.Name);
                    }
                    else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    {
                        if (propInfo.GetCustomAttributes(true).Where(x => x is KeyAttribute).Count() > 0)
                        {
                            qPrimaryColumn = $"{Environment.NewLine}    `{prop.Name}` datetime(6) NOT NULL COMMENT '{assemblyname}', ";
                            qPrimarykey += (qPrimarykey == "") ? $"`{prop.Name}`" : $", `{prop.Name}`";
                            primaryFound = true;
                        }
                        else
                        {
                            qBody += $"{Environment.NewLine}    `{prop.Name}` datetime(6) NULL COMMENT '{assemblyname}' ";
                        }
                        columnList.Add(prop.Name);
                    }
                    else
                    {
                        if (prop.PropertyType.BaseType == typeof(Enum))
                        {
                            qBody += $"{Environment.NewLine}    `{prop.Name}` INT NOT NULL COMMENT '{assemblyname}' ";
                            columnList.Add(prop.Name);
                        }
                        else
                        {
                            if (columnList.Where(x => x == string.Concat(prop.Name, "Id")).Count() <= 0)
                            {
                                //var temp = prop.PropertyType.GetProperties().OrderBy(x => x.Name).ToArray();
                                var propTypeTablename = GlobalContext.GetTableName(prop.PropertyType);
                                var tempIndexKeyName = $"IX_{propTypeTablename}_{prop.Name}Id";
                                if (tempIndexKeyName.Length > 60) tempIndexKeyName = tempIndexKeyName.Replace("_", "");
                                if (tempIndexKeyName.Length > 60) tempIndexKeyName = $"IX_{Guid.NewGuid().ToString()}";
                                if (tempIndexKeyName.Length > 60) tempIndexKeyName = tempIndexKeyName.Substring(0, 60);
                                var temp = "";
                                var tempi = 0;
                                while (true)
                                {
                                    if (qIndexkey.Contains(tempIndexKeyName + temp) == false)
                                    {
                                        tempIndexKeyName = tempIndexKeyName + temp;
                                        break;
                                    }
                                    tempi++;
                                    temp = "_" + tempi.ToString();
                                }

                                var tempForeginKeyName = $"FK_{currentTableName}_{propTypeTablename}_{prop.Name}Id";
                                if (tempForeginKeyName.Length > 60) tempForeginKeyName = tempForeginKeyName.Replace("_", "");
                                if (tempForeginKeyName.Length > 60) tempForeginKeyName = $"FK_{Guid.NewGuid().ToString()}";
                                if (tempForeginKeyName.Length > 60) tempForeginKeyName = tempForeginKeyName.Substring(0, 60);
                                temp = "";
                                tempi = 0;
                                while (true)
                                {
                                    if (qForeginkey.Contains(tempForeginKeyName + temp) == false)
                                    {
                                        tempForeginKeyName = tempForeginKeyName + temp;
                                        break;
                                    }
                                    tempi++;
                                    temp = "_" + tempi.ToString();
                                }

                                qIndexkey += $", {Environment.NewLine} KEY `{tempIndexKeyName}` (`{prop.Name}Id`) ";
                                qForeginkey += $", {Environment.NewLine} CONSTRAINT `{tempForeginKeyName}` FOREIGN KEY (`{prop.Name}Id`) REFERENCES `{propTypeTablename}` (`Id`) ON DELETE NO ACTION ";

                                qBody += $"{Environment.NewLine}    `{prop.Name}Id` BIGINT NULL COMMENT '{assemblyname}' ";
                                columnList.Add(string.Concat(prop.Name, "Id"));
                                continue;
                            }
                        }
                    }
                }
                if (primaryFound)
                    qPrimarykey = $",{Environment.NewLine}PRIMARY KEY ({qPrimarykey})";

                query.MySql_QueryText = qStart + qPrimaryColumn + qBody + qPrimarykey + qIndexkey + qForeginkey + qEnd;
            }
            else
            {
                throw new System.Exception("No supported database found.");
            }
            return _entityRepository.ExecuteSqlCommand(query); //from base repository
        }

        /// <summary>
        /// Update Database table using Entity Model
        /// </summary>
        /// <param name="modelType">Entity Type [typeof(ModelT)]</param>
        /// <param name="DeleteUnusedColumns"></param>
        /// <returns></returns>
        public int UpdateTable(Type modelType, bool DeleteUnusedColumns = true)
        {
            int retVal = 0;
            NccDbQueryText query = new NccDbQueryText();
            string currentTableName = GlobalContext.GetTableName(modelType);
            string assemblyname = modelType.Assembly.GetName().Name;
            List<DbTableViewModel> currentColumnList = GetTableColumns(modelType);
            if (currentColumnList.Count <= 0)
            {
                throw new Exception($"{currentTableName} Table does not exists.");
            }
            if (currentColumnList.FirstOrDefault().ColumnComment != assemblyname)
            {
                throw new Exception($"{currentTableName} Table already exists in module {currentColumnList.FirstOrDefault().ColumnComment}.");
            }
            List<System.Reflection.PropertyInfo> unchangedCloumnList = new List<System.Reflection.PropertyInfo>();
            List<System.Reflection.PropertyInfo> newCloumnList = new List<System.Reflection.PropertyInfo>();

            var entityProperties = modelType.GetProperties().OrderBy(x => x.Name).ToArray();
            var deletedColumnList = currentColumnList;
            foreach (var item in entityProperties)
            {
                if (currentColumnList.Where(x => x.ColumnName == item.Name).Count() > 0)
                {
                    unchangedCloumnList.Add(item);
                    deletedColumnList = deletedColumnList.Where(x => x.ColumnName != item.Name).ToList();
                }
                else if (currentColumnList.Where(x => string.IsNullOrEmpty(x.ColumnKey) == false && x.ColumnName == string.Concat(item.Name, "Id")).Count() > 0)
                {
                    unchangedCloumnList.Add(item);
                    deletedColumnList = deletedColumnList.Where(x => string.IsNullOrEmpty(x.ColumnKey) == true && x.ColumnName != string.Concat(item.Name, "Id")).ToList();
                }
                else
                {
                    newCloumnList.Add(item);
                }
            }

            if (SetupHelper.SelectedDatabase == "SqLite")
            {

            }
            else if (SetupHelper.SelectedDatabase == "MSSQL")
            {

            }
            else if (SetupHelper.SelectedDatabase == "MySql")
            {
                List<string> columnList = new List<string>();
                query.MySql_QueryText = "";
                string qIndexkey = "";
                string qForeginkey = "";
                foreach (var item in newCloumnList)
                {
                    var prop = item;
                    var propInfo = modelType.GetProperty(prop.Name);
                    //If property has NotMapped Attribute then skip column.
                    if (propInfo.GetCustomAttributes(true).Where(x => x is NotMappedAttribute).Count() > 0)
                    {
                        continue;
                    }
                    if (propInfo.PropertyType.Name.StartsWith("List"))
                    {
                        continue;
                    }
                    //If column already added then skip
                    if (columnList.Where(x => x == prop.Name).Count() > 0)
                    {
                        continue;
                    }

                    if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(Int16) || prop.PropertyType == typeof(int?) || prop.PropertyType == typeof(Int16?))
                    {
                        if (propInfo.GetCustomAttributes(true).Where(x => x is KeyAttribute).Count() > 0)
                        {
                            query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` INT AUTO_INCREMENT PRIMARY KEY COMMENT '{assemblyname}';";
                        }
                        else
                        {
                            query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` INT NOT NULL COMMENT '{assemblyname}';";
                        }
                        columnList.Add(prop.Name);
                    }
                    else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
                    {
                        if (propInfo.GetCustomAttributes(true).Where(x => x is KeyAttribute).Count() > 0)
                        {
                            query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` BIGINT AUTO_INCREMENT PRIMARY KEY COMMENT '{assemblyname}';";
                        }
                        else
                        {
                            query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` BIGINT NOT NULL COMMENT '{assemblyname}';";
                        }
                        columnList.Add(prop.Name);
                    }
                    else if (prop.PropertyType == typeof(float) || prop.PropertyType == typeof(float?))
                    {
                        if (propInfo.GetCustomAttributes(true).Where(x => x is KeyAttribute).Count() > 0)
                        {
                            query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` float PRIMARY KEY COMMENT '{assemblyname}';";
                        }
                        else
                        {
                            query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` float DEFAULT '0' COMMENT '{assemblyname}';";
                        }
                        columnList.Add(prop.Name);
                    }
                    else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                    {
                        if (propInfo.GetCustomAttributes(true).Where(x => x is KeyAttribute).Count() > 0)
                        {
                            query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` double PRIMARY KEY COMMENT '{assemblyname}';";
                        }
                        else
                        {
                            query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` double DEFAULT '0' COMMENT '{assemblyname}';";
                        }
                        columnList.Add(prop.Name);
                    }
                    else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                    {
                        if (propInfo.GetCustomAttributes(true).Where(x => x is KeyAttribute).Count() > 0)
                        {
                            query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` decimal(10,2) DEFAULT '0.00' PRIMARY KEY COMMENT '{assemblyname}';";
                        }
                        else
                        {
                            query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` decimal(10,2) DEFAULT '0.00' COMMENT '{assemblyname}';";
                        }
                        columnList.Add(prop.Name);
                    }
                    else if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(String))
                    {
                        if (propInfo.GetCustomAttributes(true).Where(x => x is KeyAttribute).Count() > 0)
                        {
                            if (propInfo.GetCustomAttributes(true).Where(x => x is MaxLengthAttribute).Count() > 0)
                            {
                                var temp = (MaxLengthAttribute)propInfo.GetCustomAttributes(true).Where(x => x is MaxLengthAttribute).FirstOrDefault();
                                long length = temp.Length;
                                if (length > 4000)
                                    query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` longtext COLLATE utf8_unicode_ci NOT NULL PRIMARY KEY COMMENT '{assemblyname}';";
                                else
                                    query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` VARCHAR({temp.Length}) COLLATE utf8_unicode_ci NOT NULL PRIMARY KEY COMMENT '{assemblyname}';";
                            }
                            else
                            {
                                query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` VARCHAR(1024) COLLATE utf8_unicode_ci NOT NULL PRIMARY KEY COMMENT '{assemblyname}';";
                            }
                        }
                        else
                        {
                            if (propInfo.GetCustomAttributes(true).Where(x => x is MaxLengthAttribute).Count() > 0)
                            {
                                var temp = (MaxLengthAttribute)propInfo.GetCustomAttributes(true).Where(x => x is MaxLengthAttribute).FirstOrDefault();
                                long length = temp.Length;
                                if (length > 4000)
                                    query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` longtext COLLATE utf8_unicode_ci NULL COMMENT '{assemblyname}';";
                                else
                                    query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` VARCHAR({temp.Length}) COLLATE utf8_unicode_ci NULL COMMENT '{assemblyname}';";
                            }
                            else
                            {
                                query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` VARCHAR(1024) COLLATE utf8_unicode_ci NULL COMMENT '{assemblyname}';";
                            }
                        }
                        columnList.Add(prop.Name);
                    }
                    else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                    {
                        query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` bit(1) NOT NULL COMMENT '{assemblyname}';";
                        columnList.Add(prop.Name);
                    }
                    else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    {
                        if (propInfo.GetCustomAttributes(true).Where(x => x is KeyAttribute).Count() > 0)
                        {
                            query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` datetime(6) NOT NULL PRIMARY KEY COMMENT '{assemblyname}';";
                        }
                        else
                        {
                            query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` datetime(6) NULL COMMENT '{assemblyname}';";
                        }
                        columnList.Add(prop.Name);
                    }
                    else
                    {
                        if (prop.PropertyType.BaseType == typeof(Enum))
                        {
                            query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` ADD `{prop.Name}` INT NOT NULL COMMENT '{assemblyname}';";
                            columnList.Add(prop.Name);
                        }
                        else
                        {
                            if (columnList.Where(x => x == string.Concat(prop.Name, "Id")).Count() <= 0)
                            {
                                var propTypeTablename = GlobalContext.GetTableName(prop.PropertyType);
                                var tempIndexKeyName = $"IX_{propTypeTablename}_{prop.Name}Id";
                                if (tempIndexKeyName.Length > 60) tempIndexKeyName = tempIndexKeyName.Replace("_", "");
                                if (tempIndexKeyName.Length > 60) tempIndexKeyName = $"IX_{Guid.NewGuid().ToString()}";
                                if (tempIndexKeyName.Length > 60) tempIndexKeyName = tempIndexKeyName.Substring(0, 60);
                                var temp = "";
                                var tempi = 0;
                                while (true)
                                {
                                    if (qIndexkey.Contains(tempIndexKeyName + temp) == false)
                                    {
                                        tempIndexKeyName = tempIndexKeyName + temp;
                                        break;
                                    }
                                    tempi++;
                                    temp = "_" + tempi.ToString();
                                }

                                var tempForeginKeyName = $"FK_{currentTableName}_{propTypeTablename}_{prop.Name}Id";
                                if (tempForeginKeyName.Length > 60) tempForeginKeyName = tempForeginKeyName.Replace("_", "");
                                if (tempForeginKeyName.Length > 60) tempForeginKeyName = $"FK_{Guid.NewGuid().ToString()}";
                                if (tempForeginKeyName.Length > 60) tempForeginKeyName = tempForeginKeyName.Substring(0, 60);
                                temp = "";
                                tempi = 0;
                                while (true)
                                {
                                    if (qForeginkey.Contains(tempForeginKeyName + temp) == false)
                                    {
                                        tempForeginKeyName = tempForeginKeyName + temp;
                                        break;
                                    }
                                    tempi++;
                                    temp = "_" + tempi.ToString();
                                }

                                query.MySql_QueryText += $"ALTER TABLE `{currentTableName}` {Environment.NewLine}ADD COLUMN `{prop.Name}Id` BIGINT NULL COMMENT '{assemblyname}', {Environment.NewLine}ADD INDEX (`{prop.Name}Id`) {Environment.NewLine}ADD CONSTRAINT `{tempForeginKeyName}` FOREIGN KEY(`{prop.Name}Id`) {Environment.NewLine}REFERENCES `{propTypeTablename}`(`Id`); ";

                                columnList.Add(string.Concat(prop.Name, "Id"));
                            }
                        }
                    }
                }

                if (DeleteUnusedColumns)
                {
                    foreach (var item in deletedColumnList)
                    {
                        query.MySql_QueryText += $"{Environment.NewLine}ALTER TABLE `{currentTableName}` DROP COLUMN `{item.ColumnName}`;";
                    }
                }
            }
            else
            {
                throw new System.Exception("No supported database found.");
            }

            if (string.IsNullOrWhiteSpace(query.SQLite_QueryText) == false || string.IsNullOrWhiteSpace(query.MSSql_QueryText) == false || string.IsNullOrWhiteSpace(query.MySql_QueryText) == false)
            {
                retVal = _entityRepository.ExecuteSqlCommand(query); //from base repository
            }
            return retVal;
        }

        /// <summary>
        /// Delete database table using Entity Model
        /// </summary>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public int DeleteTable(Type modelType)
        {
            NccDbQueryText query = new NccDbQueryText();

            if (SetupHelper.SelectedDatabase == "SqLite")
            {

            }
            else if (SetupHelper.SelectedDatabase == "MSSQL")
            {

            }
            else if (SetupHelper.SelectedDatabase == "MySql")
            {
                query.MySql_QueryText = "DROP TABLE IF EXISTS `" + GlobalContext.GetTableName(modelType) + @"`;";

            }
            else
            {
                throw new System.Exception("No supported database found.");
            }
            return _entityRepository.ExecuteSqlCommand(query); //from base repository
        }

        /// <summary>
        /// Get table column list
        /// </summary>
        /// <param name="modelType">Entity Type [typeof(ModelT)]</param>
        /// <returns></returns>
        public List<DbTableViewModel> GetTableColumns(Type modelType)
        {
            NccDbQueryText query = new NccDbQueryText();
            #region FindDatabaseName
            string databaseName = "";
            var connectionParameters = SetupHelper.ConnectionString.Split(";");
            foreach (var item in connectionParameters)
            {
                if (item.StartsWith("database"))
                {
                    databaseName = item.Split("=")[1];
                }
            }
            if (databaseName.Trim() == "")
            {
                throw new Exception("Database Name not found.");
            }
            #endregion

            string tableName = GlobalContext.GetTableName(modelType);

            if (SetupHelper.SelectedDatabase == "SqLite")
            {

            }
            else if (SetupHelper.SelectedDatabase == "MSSQL")
            {

            }
            else if (SetupHelper.SelectedDatabase == "MySql")
            {
                query.MySql_QueryText = $"SELECT table_schema `Database`, table_name TableName, column_name ColumnName, column_type ColumnType, column_key ColumnKey, extra `Extra`, column_comment ColumnComment FROM information_schema.columns WHERE  table_schema = '{databaseName}' AND table_name = '{tableName}'";
            }
            else
            {
                throw new Exception("No supported database found.");
            }

            return _entityRepository.ExecuteSqlQuery<DbTableViewModel>(query);
        }
    }
}