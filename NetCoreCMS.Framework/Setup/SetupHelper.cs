using Microsoft.AspNetCore.Hosting;
using NetCoreCMS.Framework.Core.Data;
using Newtonsoft.Json;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Framework.Setup
{
    public class SetupHelper
    {
        private static string _configFileName = "setup.json";
        public static bool IsComplete { get; set; }
        public static string SelectedDatabase { get; set; }
        public static string ConnectionString { get; set; }
        public static SetupConfig LoadSetup(IHostingEnvironment env)
        {
            var config = new SetupConfig();
            var rootDir = env.ContentRootPath;
            var file = File.Open(Path.Combine(rootDir, _configFileName), FileMode.OpenOrCreate);
            using (StreamReader sr = new StreamReader(file))
            {
                var content = sr.ReadToEnd();
                if (!string.IsNullOrEmpty(content))
                {
                    config = JsonConvert.DeserializeObject<SetupConfig>(content);
                    IsComplete = config.IsComplete;
                    SelectedDatabase = config.SelectedDatabase;
                    ConnectionString = config.ConnectionString;
                }
            }
            return config;
        }

        //public static bool CreateAdminUser(IHostingEnvironment env, string userName, string password)
        //{
        //    NccDbContext dbContext = new NccDbContext();
        //}

        public static SetupConfig SaveSetup(IHostingEnvironment env)
        {
            var config = new SetupConfig();
            var rootDir = env.ContentRootPath;
            var file = File.Open(Path.Combine(rootDir, _configFileName), FileMode.Create);
            using (StreamWriter sw = new StreamWriter(file))
            {
                var content = JsonConvert.SerializeObject(new SetupConfig()
                {
                    IsComplete = IsComplete,
                    SelectedDatabase = SelectedDatabase,
                    ConnectionString = ConnectionString
                }, Formatting.Indented);

                if (!string.IsNullOrEmpty(content))
                {
                    sw.Write(content.ToCharArray());
                }
            }
            return config;
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
                    string path = env.ContentRootPath;
                    path = Path.Combine(path, "Data");
                    string dbFileName = Path.Combine(path, "NetCoreCMS.Database.SqLite.db");
                    File.Create(dbFileName);
                    return File.Exists(dbFileName);

            }
            return false;
        }
    }
}
