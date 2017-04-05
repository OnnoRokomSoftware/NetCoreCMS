using Microsoft.AspNetCore.Hosting;
using NetCoreCMS.Framework.Core.Data;
using Newtonsoft.Json;
using System.IO;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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

        public static bool CreateAdminUser(IHostingEnvironment env, string userName, string password, string email, DatabaseEngine database, string connectionString)
        {
            var builder = new DbContextOptionsBuilder<NccDbContext>();
            builder.UseSqlite(connectionString);
            NccDbContext dbContext = new NccDbContext(builder.Options);

            NccRoleStore roleStore = new NccRoleStore(dbContext);
            NccUserStore userStore = new NccUserStore(dbContext);
            CreateCmsDefaultRoles(roleStore);

            var adminUser = new NccUser()
            {
                Email = email,
                FullName = "Site Admin",
                Name = "Administrator",
                UserName = userName
            };

            return false;
        }

        private static void CreateCmsDefaultRoles(NccRoleStore roleStore)
        {
            NccRole administrator = new NccRole() { Name = NccCmsRoles.Administrator, NormalizedName = NccCmsRoles.Administrator };
            NccRole author = new NccRole() { Name = NccCmsRoles.Author, NormalizedName = NccCmsRoles.Author };
            NccRole contributor = new NccRole() { Name = NccCmsRoles.Contributor, NormalizedName = NccCmsRoles.Contributor };
            NccRole editor = new NccRole() { Name = NccCmsRoles.Editor, NormalizedName = NccCmsRoles.Editor };
            NccRole subscriber = new NccRole() { Name = NccCmsRoles.Subscriber, NormalizedName = NccCmsRoles.Subscriber };
            NccRole reader = new NccRole() { Name = NccCmsRoles.Reader, NormalizedName = NccCmsRoles.Reader };
            
            roleStore.CreateAsync(administrator);
            roleStore.CreateAsync(author);
            roleStore.CreateAsync(contributor);
            roleStore.CreateAsync(editor);
            roleStore.CreateAsync(subscriber);
            roleStore.CreateAsync(reader);
        }

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
            return DatabaseFactory.CreateDatabase(env, database, databaseInfo);
        }
    }
}
