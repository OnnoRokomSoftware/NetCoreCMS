using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetCoreCMS.Framework.Core.Data
{
    public class DatabaseFactory 
    {
        private static string _sqLiteConString = "Data Source={0}\\{1}.db";
        private static string _sqlLocalDb = "Server=(localdb)\\mssqllocaldb;Database=NetCoreCMS.Web.db;Trusted_Connection=True;MultipleActiveResultSets=true";
        private static string _mySqlConString = "server={0};port={1};database={2};userid={3};pwd={4};sslmode=none;";
        private static string _msSqlConString = "Data Source={0}; Initial Catalog={1}; User Id = {2}; Password = {3}; MultipleActiveResultSets=true";
        private static string _pgSqlConString = "Host={0}; Port={1}; Database={2}; User ID={3}; Password={4}; Pooling=true;";

        public static string GetConnectionString(IHostingEnvironment env, DatabaseEngine engine, string server, string port, string database, string username, string password)
        {
            switch (engine)
            {
                case DatabaseEngine.MsSql:
                    if (string.IsNullOrEmpty(port))
                        return string.Format(_msSqlConString, server, database, username, password);
                    else
                        return string.Format(_msSqlConString, server+","+port, database, username, password);

                case DatabaseEngine.MsSqlLocalStorage:
                    return _sqlLocalDb;

                case DatabaseEngine.MySql:
                    if (string.IsNullOrEmpty(port))
                        return string.Format(_mySqlConString, server, "3306", database, username, password);
                    else
                        return string.Format(_mySqlConString, server, port, database, username, password);

                case DatabaseEngine.PgSql:
                    if (string.IsNullOrEmpty(port))
                        return string.Format(_pgSqlConString, server, "5432", database, username, password);
                    else
                        return string.Format(_pgSqlConString, server, port, database, username, password);
                case DatabaseEngine.SqLite:
                    var path = GlobalConfig.ContentRootPath;
                    return string.Format(_sqLiteConString, Path.Combine(path,"Data"), "NetCoreCMS.Database.SqLite");
                default:
                    return "";

            }
        }

        public static bool CreateDatabase(IHostingEnvironment env, DatabaseEngine database, DatabaseInfo databaseInfo)
        {
            switch (database)
            {
                case DatabaseEngine.MsSql:
                    break;
                case DatabaseEngine.MsSqlLocalStorage:
                    break;
                case DatabaseEngine.MySql:
                    break;
                case DatabaseEngine.PgSql:
                    break;
                case DatabaseEngine.SqLite:
                    string path = GlobalConfig.ContentRootPath;
                    path = Path.Combine(path, "Data");
                    string dbFileName = Path.Combine(path, "NetCoreCMS.Database.SqLite.db");
                    var dbFile = new FileInfo(dbFileName);
                    if (!dbFile.Exists)
                    {
                        dbFile.Create();
                    }
                    var builder = new DbContextOptionsBuilder<NccDbContext>();
                    var conStr = GetConnectionString(env, DatabaseEngine.SqLite, "","","","","");
                    builder.UseSqlite( conStr, options => options.MigrationsAssembly("NetCoreCMS.Web"));
                    var dbContext = new NccDbContext(builder.Options);
                    var migrator = dbContext.Database.GetPendingMigrations();
                    dbContext.Database.Migrate();
                    var created = dbContext.Database.EnsureCreated();
                    return File.Exists(dbFileName);

            }
            return false;
        }

        private static void RegisterEntities(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
        {
            var entityTypes = typeToRegisters.Where(x => x.GetTypeInfo().IsSubclassOf(typeof(BaseModel)) && !x.GetTypeInfo().IsAbstract);
            foreach (var type in entityTypes)
            {
                modelBuilder.Entity(type);
            }
        }

        

    }
}
