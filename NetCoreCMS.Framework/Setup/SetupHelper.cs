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
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Core.Models;
using Microsoft.AspNetCore.Identity;
using NetCoreCMS.Framework.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Repository;
using NetCoreCMS.Framework.Core.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Extensions;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Core;
using System.Linq;
using NetCoreCMS.Framework.Resources;
using System.Reflection;
using NetCoreCMS.Framework.i18n;
using static NetCoreCMS.Framework.Core.Models.NccPage;
using static NetCoreCMS.Framework.Core.Models.NccPost;

namespace NetCoreCMS.Framework.Setup
{
    public class SetupHelper
    {
        public SetupHelper()
        {
            LoadSetup();
        }

        private static string _configFileName = "setup.json";
        public static bool IsDbCreateComplete { get; set; }
        public static bool IsAdminCreateComplete { get; set; }
        public static string SelectedDatabase { get; set; }
        public static string ConnectionString { get; set; }
        public static int LoggingLevel { get; set; } = (int) LogLevel.Warning;
        public static string Language { get; set; }
        public static string StartupType { get; set; } = StartupTypeText.Url;
        public static string StartupData { get; set; } = "/CmsHome";
        public static string StartupUrl { get; set; } = "/CmsHome";
        public static string TablePrefix { get; set; } = "ncc_";
        public static bool EnableCache { get; set; } = false;

        public static SetupConfig SaveSetup()
        {
            var config = new SetupConfig();
            var rootDir = GlobalContext.ContentRootPath;
            using (var file = File.Open(Path.Combine(rootDir, _configFileName), FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    var content = JsonConvert.SerializeObject(new SetupConfig()
                    {
                        IsDbCreateComplete = IsDbCreateComplete,
                        IsAdminCreateComplete = IsAdminCreateComplete,
                        SelectedDatabase = SelectedDatabase,
                        ConnectionString = ConnectionString,
                        LoggingLevel = LoggingLevel,
                        Language = Language,
                        StartupUrl = StartupUrl,
                        TablePrefix = TablePrefix,
                        StartupData = StartupData,
                        StartupType = StartupType,
                        EnableCache = EnableCache
                    }, Formatting.Indented);

                    if (!string.IsNullOrEmpty(content))
                    {
                        sw.Write(content.ToCharArray());
                    }
                }
            }
            return config;
        }

        public static SetupConfig LoadSetup()
        {
            var rootDir = GlobalContext.ContentRootPath;
            if(rootDir == null)
            {
                rootDir = Directory.GetCurrentDirectory();
            }
            
            var config = new SetupConfig();
            
            var file = File.Open(Path.Combine(rootDir, _configFileName), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
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
                    LoggingLevel = config.LoggingLevel;
                    Language = config.Language;
                    StartupType = config.StartupType;
                    StartupData = config.StartupData;
                    StartupUrl = config.StartupUrl;
                    TablePrefix = config.TablePrefix;
                    EnableCache = config.EnableCache;
                }
            }
            return config;
        }

        public static void UpdateSetup(SetupConfig setupConfig)
        {
            var rootDir = GlobalContext.ContentRootPath;
            using (var file = File.Open(Path.Combine(rootDir, _configFileName), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    var content = JsonConvert.SerializeObject(setupConfig, Formatting.Indented);

                    if (!string.IsNullOrEmpty(content))
                    {
                        sw.Write(content.ToCharArray());
                    }
                }
            }
        }

        public static void DeleteSetup()
        {
            File.Delete(_configFileName);
        }

        public static async Task<NccUser> CreateSuperAdminUser(  
            NccDbContext nccDbContext,
            UserManager<NccUser> userManager,
            RoleManager<NccRole> roleManager,
            SignInManager<NccUser> signInManager,
            WebSiteInfo setupInfo
            )
        {   
            
            CreateCmsDefaultRoles(nccDbContext, roleManager);
            NccRole superAdmin = await roleManager.FindByNameAsync(NccCmsRoles.SuperAdmin);
            if(superAdmin != null)
            {
                var adminUser = new NccUser()
                {
                    Email = setupInfo.Email,
                    FullName = "Site Super Admin",
                    Name = "Super Admin",
                    UserName = setupInfo.AdminUserName,
                };

                await userManager.CreateAsync(adminUser, setupInfo.AdminPassword);
                NccUser user = await userManager.FindByNameAsync(setupInfo.AdminUserName);
                await userManager.AddToRoleAsync(user, NccCmsRoles.SuperAdmin);

                return user;
            }

            return null;
        }
        
        private static void CreateCmsDefaultRoles(NccDbContext nccDbContext, RoleManager<NccRole> roleManager)
        {
            var nccPermissionService = new NccPermissionService(new NccPermissionRepository(nccDbContext));
            var administrator = CreatePermissionObject("Administrator");
            var manager = CreatePermissionObject("Manager");
            var editor = CreatePermissionObject("Editor");
            var author = CreatePermissionObject("Author");
            var contributor = CreatePermissionObject("Contributor");
            var subscriber = CreatePermissionObject("Subscriber");
            
            nccPermissionService.Save(
                new List<NccPermission>() {
                    administrator,
                    manager,
                    editor,
                    author,
                    contributor,
                    subscriber
                }
            );
            
            NccRole superAdmin = new NccRole() { Name = NccCmsRoles.SuperAdmin, NormalizedName = NccCmsRoles.SuperAdmin };
            var sa = roleManager.CreateAsync(superAdmin).Result;
        }
         
        private static NccPermission CreatePermissionObject(string roleName)
        {
            var assembly = typeof(SharedResource).GetTypeInfo().Assembly;
            using (Stream resource = assembly.GetManifestResourceStream($"NetCoreCMS.Framework.Resources.DefaultPermissions.{roleName}.json"))
            {
                var sr = new StreamReader(resource);
                var admJson = sr.ReadToEndAsync().Result;
                var permission = JsonConvert.DeserializeObject<NccPermission>(admJson);
                return permission;
            }            
        }

        internal static DbContextOptions GetDbContextOptions()
        {
            return DatabaseFactory.GetDbContextOptions();            
        }

        public static void InitilizeDatabase()
        {
            DatabaseFactory.InitilizeDatabase((SupportedDatabases)Enum.Parse(typeof(SupportedDatabases),SelectedDatabase), ConnectionString);
        }
        
        public static bool CreateDatabase(SupportedDatabases database, DatabaseInfo databaseInfo)
        {
            DatabaseFactory.CreateDatabase(database, databaseInfo);            
            return true;
        }

        public static async Task SaveBasicData(NccUser nccUser, NccDbContext nccDbConetxt, UserManager<NccUser> userManager, RoleManager<NccRole> roleManager, NccSignInManager<NccUser> signInManager, WebSiteInfo setupInfo/*, NccWebSiteWidgetService nccWebSiteWidgetService, NccWebSiteService nccWebSiteService*/)
        {
            string enDemoTitle = "";
            string enDemoSlug = "";
            string enDemoContent = "";
            string bnDemoTitle = "";
            string bnDemoSlug = "";
            string bnDemoContent = "";
            //var webSiteRepository = new NccWebSiteRepository(nccDbConetxt);
            //var webSiteInfoRepository = new NccWebSiteInfoRepository(nccDbConetxt);
            //var webSiteService = new NccWebSiteService(webSiteRepository, webSiteInfoRepository);

            #region Create a sample page
            var nccPageRepository = new NccPageRepository(nccDbConetxt);
            var nccPageDetailsRepository = new NccPageDetailsRepository(nccDbConetxt);
            var nccPageService = new NccPageService(nccPageRepository, nccPageDetailsRepository);

            try
            {
                enDemoTitle = "Sample Page ";
                enDemoSlug = "Sample-Page";
                enDemoContent = "This is a sample page.";
                bnDemoTitle = "নমুনা পৃষ্ঠা ";
                bnDemoSlug = "নমুনা-পৃষ্ঠা";
                bnDemoContent = "এটি একটি নমুনা পাতা।";

                NccPage page = new NccPage();
                page.Metadata = "DEMODATA";
                page.PublishDate = DateTime.Now;
                page.PageStatus = NccPageStatus.Published;
                page.PageType = NccPageType.Public;
                page.Layout = "SiteLayout";
                page.CreateBy = page.ModifyBy = 1;
                page.Name = enDemoTitle;

                foreach (var item in SupportedCultures.Cultures)
                {
                    NccPageDetails _nccPageDetails = new NccPageDetails();
                    _nccPageDetails.Language = item.TwoLetterISOLanguageName;
                    if (item.TwoLetterISOLanguageName == "en")
                    {
                        _nccPageDetails.Title = enDemoTitle;
                        _nccPageDetails.Slug = enDemoSlug;
                        _nccPageDetails.Name = enDemoSlug;
                        _nccPageDetails.Content = "<h1 style=\"text-align:center\">" + enDemoTitle + "</h1><p>" + enDemoContent + "</p>";
                        _nccPageDetails.MetaDescription = enDemoTitle + " " + enDemoContent;
                    }
                    else if (item.TwoLetterISOLanguageName == "bn")
                    {
                        _nccPageDetails.Title = bnDemoTitle;
                        _nccPageDetails.Slug = bnDemoSlug;
                        _nccPageDetails.Name = bnDemoSlug;
                        _nccPageDetails.Content = "<h1 style=\"text-align:center\">" + bnDemoTitle + "</h1><p>" + bnDemoContent + "</p>";
                        _nccPageDetails.MetaDescription = bnDemoTitle + " " + bnDemoContent;
                    }
                    page.PageDetails.Add(_nccPageDetails);
                }
                nccPageService.Save(page);
            }
            catch (Exception ex)
            {

            }
            #endregion

            #region Create a sample category
            var categoryRepository = new NccCategoryRepository(nccDbConetxt);
            var categoryService = new NccCategoryService(categoryRepository);

            try
            {
                enDemoTitle = "Sample Category ";
                enDemoSlug = "Sample-Category";
                bnDemoTitle = "নমুনা ক্যাটাগরি ";
                bnDemoSlug = "নমুনা-ক্যাটাগরি";
                NccCategory item = new NccCategory();
                item.Metadata = "DEMODATA";
                item.Name = enDemoTitle;
                item.CategoryImage = "/media/Images/2017/06/image-slider-0.jpg";
                foreach (var lang in SupportedCultures.Cultures)
                {
                    NccCategoryDetails _nccDetails = new NccCategoryDetails();
                    _nccDetails.Language = lang.TwoLetterISOLanguageName;
                    if (lang.TwoLetterISOLanguageName == "en")
                    {
                        _nccDetails.Title = enDemoTitle;
                        _nccDetails.Slug = enDemoSlug;
                        _nccDetails.Name = enDemoSlug;
                    }
                    else if (lang.TwoLetterISOLanguageName == "bn")
                    {
                        _nccDetails.Title = bnDemoTitle;
                        _nccDetails.Slug = bnDemoSlug;
                        _nccDetails.Name = bnDemoSlug;
                    }
                    item.CategoryDetails.Add(_nccDetails);
                }
                categoryService.Save(item);
            }
            catch (Exception ex) { }
            #endregion
            //var tagRepository = new NccTagRepository(nccDbConetxt);
            //var tagService = new NccTagService(tagRepository);

            #region Create a sample post
            var postRepository = new NccPostRepository(nccDbConetxt);
            var postDetailsRepository = new NccPostDetailsRepository(nccDbConetxt);
            var postService = new NccPostService(postRepository, postDetailsRepository);

            try
            {
                enDemoTitle = "Sample Post ";
                enDemoSlug = "Sample-Post";
                enDemoContent = "This is a sample post.";
                bnDemoTitle = "নমুনা পোস্ট ";
                bnDemoSlug = "নমুনা-পোস্ট";
                bnDemoContent = "এটি একটি নমুনা পোস্ট।";
                var categoryList = categoryService.LoadAll();

                NccPost post = new NccPost();
                post.Metadata = "DEMODATA";
                post.PublishDate = DateTime.Now;
                post.PostStatus = NccPostStatus.Published;
                post.PostType = NccPostType.Public;
                post.Layout = "SiteLayout";
                post.CreateBy = post.ModifyBy = 1;
                post.Name = enDemoTitle;
                post.AllowComment = true;
                post.ThumImage = "/media/Images/2017/06/image-slider-2.jpg";
                foreach (var postItem in SupportedCultures.Cultures)
                {
                    NccPostDetails _nccPostDetails = new NccPostDetails();
                    _nccPostDetails.Language = postItem.TwoLetterISOLanguageName;
                    if (postItem.TwoLetterISOLanguageName == "en")
                    {
                        _nccPostDetails.Title = enDemoTitle;
                        _nccPostDetails.Slug = enDemoSlug;
                        _nccPostDetails.Name = enDemoSlug;
                        _nccPostDetails.Content = "<h1 style=\"text-align:center\">" + enDemoTitle + "</h1><hr />" + enDemoContent;
                        _nccPostDetails.MetaDescription = enDemoTitle + " " + enDemoContent;
                    }
                    else if (postItem.TwoLetterISOLanguageName == "bn")
                    {
                        _nccPostDetails.Title = bnDemoTitle;
                        _nccPostDetails.Slug = bnDemoSlug;
                        _nccPostDetails.Name = bnDemoSlug;
                        _nccPostDetails.Content = "<h1 style=\"text-align:center\">" + bnDemoTitle + "</h1><hr />" + bnDemoContent;
                        _nccPostDetails.MetaDescription = bnDemoTitle + " " + bnDemoContent;
                    }
                    post.PostDetails.Add(_nccPostDetails);
                }
                postService.Save(post);

                try
                {
                    post = postService.Get(post.Id, true);
                    post.Categories = new List<NccPostCategory>();
                    var temp = categoryList.FirstOrDefault();
                    if (temp != null)
                    {
                        if (post.Categories.Where(x => x.CategoryId == temp.Id).Count() <= 0)
                        {
                            post.Categories.Add(new NccPostCategory() { Post = post, CategoryId = temp.Id });
                        }
                    }
                    postService.Update(post);
                }
                catch (Exception ex) { }
            }
            catch (Exception ex) { }
            #endregion

            #region Create a sample comment
            var commentsRepository = new NccCommentsRepository(nccDbConetxt);
            var commentsService = new NccCommentsService(commentsRepository);
            try
            {
                enDemoContent = "This is a sample comment.";
                bnDemoContent = "এটি একটি নমুনা মন্তব্য।";
                NccComment commentItem = new NccComment();
                commentItem.Metadata = "DEMODATA";
                commentItem.Name = "Sample Comments";
                commentItem.Content = enDemoContent;
                if (Language == "bn")
                {
                    commentItem.Content = bnDemoContent;
                }
                commentItem.CommentStatus = NccComment.NccCommentStatus.Approved;
                commentItem.Post = postService.LoadAll().FirstOrDefault();
                commentsService.Save(commentItem);
            }
            catch (Exception ex) { }
            #endregion

            #region Create sample menu
            var menuRepository = new NccMenuRepository(nccDbConetxt);
            var menuItemRepository = new NccMenuItemRepository(nccDbConetxt);
            var menuService = new NccMenuService(menuRepository, menuItemRepository);

            try
            {
                NccMenu nccMenu = new NccMenu()
                {
                    Name = "Main Menu",
                    Position = "Navigation",
                    MenuOrder = 1,
                    MenuLanguage = ""
                };
                if (Language == "bn")
                {
                    nccMenu.MenuItems.Add(new NccMenuItem()
                    {
                        Action = "",
                        Controller = "",
                        Data = "",
                        //Id = item.Id,
                        MenuActionType = NccMenuItem.ActionType.Url,
                        MenuOrder = 1,
                        Module = "",
                        Name = "হোম",
                        Target = "_self",
                        Url = "/"
                    });
                    nccMenu.MenuItems.Add(new NccMenuItem()
                    {
                        Action = "",
                        Controller = "",
                        Data = "",
                        //Id = item.Id,
                        MenuActionType = NccMenuItem.ActionType.Url,
                        MenuOrder = 1,
                        Module = "",
                        Name = "নমুনা পৃষ্ঠা",
                        Target = "_self",
                        Url = "/নমুনা-পৃষ্ঠা"
                    });
                    nccMenu.MenuItems.Add(new NccMenuItem()
                    {
                        Action = "",
                        Controller = "",
                        Data = "",
                        //Id = item.Id,
                        MenuActionType = NccMenuItem.ActionType.Url,
                        MenuOrder = 1,
                        Module = "",
                        Name = "ব্লগ পোস্ট",
                        Target = "_self",
                        Url = "/Post"
                    });
                    nccMenu.MenuItems.Add(new NccMenuItem()
                    {
                        Action = "",
                        Controller = "",
                        Data = "",
                        //Id = item.Id,
                        MenuActionType = NccMenuItem.ActionType.Url,
                        MenuOrder = 1,
                        Module = "",
                        Name = "ব্লগ বিভাগ",
                        Target = "_self",
                        Url = "/Category"
                    });
                }
                else
                {
                    nccMenu.MenuItems.Add(new NccMenuItem()
                    {
                        Action = "",
                        Controller = "",
                        Data = "",
                        //Id = item.Id,
                        MenuActionType = NccMenuItem.ActionType.Url,
                        MenuOrder = 1,
                        Module = "",
                        Name = "Home",
                        Target = "_self",
                        Url = "/"
                    });
                    nccMenu.MenuItems.Add(new NccMenuItem()
                    {
                        Action = "",
                        Controller = "",
                        Data = "",
                        //Id = item.Id,
                        MenuActionType = NccMenuItem.ActionType.Url,
                        MenuOrder = 1,
                        Module = "",
                        Name = "Sample Page",
                        Target = "_self",
                        Url = "/Sample-Page"
                    });
                    nccMenu.MenuItems.Add(new NccMenuItem()
                    {
                        Action = "",
                        Controller = "",
                        Data = "",
                        //Id = item.Id,
                        MenuActionType = NccMenuItem.ActionType.Url,
                        MenuOrder = 1,
                        Module = "",
                        Name = "Blog Posts",
                        Target = "_self",
                        Url = "/Post"
                    });
                    nccMenu.MenuItems.Add(new NccMenuItem()
                    {
                        Action = "",
                        Controller = "",
                        Data = "",
                        //Id = item.Id,
                        MenuActionType = NccMenuItem.ActionType.Url,
                        MenuOrder = 1,
                        Module = "",
                        Name = "Blog Categories",
                        Target = "_self",
                        Url = "/Category"
                    });
                }

                menuService.Save(nccMenu);
            }
            catch (Exception ex) { }
            #endregion

            #region Create sample widget
            //var currentWebsite = nccWebSiteService.LoadAll().FirstOrDefault();
            //var nccWebSiteWidget = new NccWebSiteWidget()
            //{
            //    WebSite = currentWebsite,
            //    WidgetConfigJson = "",
            //    ModuleId = "NetCoreCMS.Core.Modules.Cms",
            //    WidgetId = "NetCoreCms.Modules.Cms.CmsSearch",
            //    ThemeId = "com.NetCoreCMS.themes.NccSeventeen",
            //    LayoutName = "SiteLayout",
            //    Zone = "RightSidebar",
            //    WidgetOrder = 1,
            //    WidgetData = "",
            //};
            //nccWebSiteWidgetService.Save(nccWebSiteWidget);
            #endregion
        }

        //public static void CreateWebSite(NccDbContext dbContext, WebSiteInfo setupInfo)
        //{
        //    var webSiteRepository = new NccWebSiteRepository(dbContext);
        //    var webSiteInfoRepository = new NccWebSiteInfoRepository(dbContext);

        //    var webSiteService = new NccWebSiteService(webSiteRepository, webSiteInfoRepository);
        //    var webSite = new NccWebSite()
        //    {
        //        AllowRegistration = true,
        //        //Copyrights = "Copyright (c) " + DateTime.Now.Year + " " + setupInfo.SiteName,
        //        DateFormat = "dd/mm/yyyy",
        //        EmailAddress = setupInfo.Email,
        //        Name = setupInfo.SiteName,
        //        NewUserRole = "Reader",
        //        //SiteTitle = setupInfo.SiteName,
        //        //Tagline = setupInfo.Tagline,
        //        TimeFormat = "hh:mm:ss",
        //        TimeZone = "UTC_6"
        //    };
        //    webSiteService.Save(webSite);
        //}

        public static void CrateNccWebSite(NccDbContext dbContext, WebSiteInfo webSiteInfo)
        {
            var webSiteRepository = new NccWebSiteRepository(dbContext);
            var webSiteInfoRepository = new NccWebSiteInfoRepository(dbContext);
            var webSiteService = new NccWebSiteService(webSiteRepository, webSiteInfoRepository);
            var webSite = new NccWebSite()
            {
                Name = webSiteInfo.SiteName,
                AllowRegistration = true,
                DateFormat = "dd/MM/yyyy",
                TimeFormat = "hh:mm:ss",
                EmailAddress = webSiteInfo.Email,
                Language = webSiteInfo.Language,
                NewUserRole = "Subscriber",
                TimeZone = "UTC_6",                
                EnableCache = webSiteInfo.EnableCache
            };

            if (webSiteInfo.TablePrefix?.Trim() != "")
            {
                if (webSiteInfo.TablePrefix.EndsWith("_"))
                    webSite.TablePrefix = webSiteInfo.TablePrefix.Trim();
                else
                    webSite.TablePrefix = webSiteInfo.TablePrefix.Trim() + "_";
            }

            webSite.WebSiteInfos = new List<NccWebSiteInfo>();
            foreach (var item in SupportedCultures.Cultures)
            {
                webSite.WebSiteInfos.Add(new NccWebSiteInfo()
                {
                    Language = item.TwoLetterISOLanguageName,
                    Name = webSiteInfo.SiteName,
                    SiteTitle = webSiteInfo.SiteName,
                    Tagline = webSiteInfo.Tagline
                });
            }

            webSiteService.Save(webSite);
        }

        public static void RegisterAuthServices(SupportedDatabases dbe)
        {
            switch (dbe)
            {
                case SupportedDatabases.MSSQL:
                    GlobalContext.Services.AddDbContext<NccDbContext>(options =>
                        options.UseSqlServer(SetupHelper.ConnectionString, opts => opts.MigrationsAssembly("NetCoreCMS.Framework"))
                    );                    
                    break;
                case SupportedDatabases.MsSqlLocalStorage:
                    break;
                case SupportedDatabases.MySql:
                    GlobalContext.Services.AddDbContext<NccDbContext>(options =>
                        options.UseMySql(SetupHelper.ConnectionString, opts => opts.MigrationsAssembly("NetCoreCMS.Framework"))
                    );
                    
                    break;
                case SupportedDatabases.SqLite:
                    GlobalContext.Services.AddDbContext<NccDbContext>(options =>
                        options.UseSqlite(SetupHelper.ConnectionString, opts => opts.MigrationsAssembly("NetCoreCMS.Framework"))
                    );
                    
                    break;
                case SupportedDatabases.PgSql:
                    break;
            }
            
            GlobalContext.Services.AddCustomizedIdentity();
            
        }
    }
}
