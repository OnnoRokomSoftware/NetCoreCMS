using Microsoft.AspNetCore.Hosting;
using NetCoreCMS.Framework.Core.Data;
using Newtonsoft.Json;
using System.IO;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using NetCoreCMS.Framework.Utility;
using System;

namespace NetCoreCMS.Framework.Setup
{
    public class SetupHelper
    {
        private static string _configFileName = "setup.json";
        public static bool IsDbCreateComplete { get; set; }
        public static bool IsAdminCreateComplete { get; set; }
        public static string SelectedDatabase { get; set; }
        public static string ConnectionString { get; set; }

        public static SetupConfig LoadSetup(IHostingEnvironment env)
        {
            var config = new SetupConfig();
            var rootDir = GlobalConfig.ContentRootPath;
            var file = File.Open(Path.Combine(rootDir, _configFileName), FileMode.OpenOrCreate);
            using (StreamReader sr = new StreamReader(file))
            {
                var content = sr.ReadToEnd();
                if (!string.IsNullOrEmpty(content))
                {
                    config = JsonConvert.DeserializeObject<SetupConfig>(content);
                    IsDbCreateComplete = config.IsDbCreateComplete;
                    IsAdminCreateComplete = config.IsAdminCreateComplete;
                    SelectedDatabase = config.SelectedDatabase;
                    ConnectionString = config.ConnectionString;
                }
            }
            return config;
        }

        public static async Task<bool> CreateAdminUser(
            IHostingEnvironment env, 
            UserManager<NccUser> userManager,
            RoleManager<NccRole> roleManager,
            SignInManager<NccUser> signInManager,
            SetupInfo setupInfo
            )
        {   
            
            CreateCmsDefaultRoles(roleManager);
            NccRole admin = await roleManager.FindByNameAsync(NccCmsRoles.Administrator);
            var adminUser = new NccUser()
            {
                Email = setupInfo.Email,
                FullName = "Site Admin",
                Name = "Administrator",
                UserName = setupInfo.AdminUserName,
            };
            
            await userManager.CreateAsync(adminUser, setupInfo.AdminPassword);
            NccUser user = await userManager.FindByNameAsync(setupInfo.AdminUserName);
            await userManager.AddToRoleAsync(user, NccCmsRoles.Administrator);
            await signInManager.SignInAsync(user, false);

            return false;
        }

        private static void CreateCmsDefaultRoles(RoleManager<NccRole> roleManager)
        {
            NccRole administrator = new NccRole() { Name = NccCmsRoles.Administrator, NormalizedName = NccCmsRoles.Administrator };
            NccRole author = new NccRole() { Name = NccCmsRoles.Author, NormalizedName = NccCmsRoles.Author };
            NccRole contributor = new NccRole() { Name = NccCmsRoles.Contributor, NormalizedName = NccCmsRoles.Contributor };
            NccRole editor = new NccRole() { Name = NccCmsRoles.Editor, NormalizedName = NccCmsRoles.Editor };
            NccRole subscriber = new NccRole() { Name = NccCmsRoles.Subscriber, NormalizedName = NccCmsRoles.Subscriber };
            NccRole reader = new NccRole() { Name = NccCmsRoles.Reader, NormalizedName = NccCmsRoles.Reader };
            
            roleManager.CreateAsync(administrator);
            roleManager.CreateAsync(author);
            roleManager.CreateAsync(contributor);
            roleManager.CreateAsync(editor);
            roleManager.CreateAsync(subscriber);
            roleManager.CreateAsync(reader);
        }

        public static void InitilizeDatabase()
        {
            DatabaseFactory.InitilizeDatabase((DatabaseEngine)Enum.Parse(typeof(DatabaseEngine),SelectedDatabase), ConnectionString);
        }

        public static SetupConfig SaveSetup(IHostingEnvironment env)
        {
            var config = new SetupConfig();
            var rootDir = GlobalConfig.ContentRootPath;
            var file = File.Open(Path.Combine(rootDir, _configFileName), FileMode.Create);
            using (StreamWriter sw = new StreamWriter(file))
            {
                var content = JsonConvert.SerializeObject(new SetupConfig()
                {
                    IsDbCreateComplete = IsDbCreateComplete,
                    IsAdminCreateComplete = IsAdminCreateComplete,
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
            DatabaseFactory.CreateDatabase(env, database, databaseInfo);            
            return true;
        }
    }
}
