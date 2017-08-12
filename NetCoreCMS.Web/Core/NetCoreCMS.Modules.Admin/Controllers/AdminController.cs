using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace NetCoreCMS.Core.Modules.Admin.Controllers
{
    [Authorize(Roles ="SuperAdmin,Administrator")]
    [AdminMenu(Name ="Settings", Order = 20)]
    public class AdminController : NccController
    {
        NccWebSiteService _webSiteService;
        NccPageService _pageService;
        NccPostService _postService;
        NccCategoryService _categoryService;
        NccSettingsService _settingsService;
        RoleManager<NccRole> _roleManager;
        NccStartupService _startupService;

        public AdminController(
            NccWebSiteService nccWebSiteService, 
            NccPageService pageService, 
            NccPostService postService, 
            NccCategoryService categoryService,
            NccSettingsService settingsService,
            RoleManager<NccRole> roleManager,
            NccStartupService startupService,
            ILoggerFactory loggarFactory)
        {
            _webSiteService = nccWebSiteService;
            _pageService = pageService;
            _postService = postService;
            _categoryService = categoryService;
            _settingsService = settingsService;
            _roleManager = roleManager;
            _startupService = startupService;
            _logger = loggarFactory.CreateLogger<AdminController>();
        }
        
        [Authorize]
        //[AdminMenuItem(Name = "Dashboard", Url = "/Admin", IconCls = "fa-dashboard", Order = 1)]
        public ActionResult Index()
        {
            var webSite = new NccWebSite();
            var webSites = _webSiteService.LoadAll();

            if (webSites != null && webSites.Count > 0)
            {
                webSite = webSites.FirstOrDefault();
            }
            return View(webSite);
        }

        #region Settings

        [AdminMenuItem(Name = "General", Url = "/Admin/Settings", IconCls = "fa-gear", Order = 2)]
        public ActionResult Settings()
        {
            var webSite = new NccWebSite();
            var webSites = _webSiteService.LoadAll();

            if (webSites != null && webSites.Count > 0)
            {
                webSite = webSites.FirstOrDefault();
            }
            return View(webSite);
        }

        [HttpPost]
        public ActionResult Settings(NccWebSite website)
        {
            if (ModelState.IsValid)
            {
                _webSiteService.Update(website);
            }
            else
            {
                ModelState.AddModelError("Name", "Please check all values and submit again.");
            }
            GlobalConfig.WebSite = website;
            return View(website);
        }

        [AdminMenuItem(Name = "Startup", Url = "/Admin/Startup", IconCls = "fa-random", Order = 3)]
        public ActionResult Startup()
        {
            var model = PrepareStartupViewData();            
            return View(model);
        }

        [AdminMenuItem(Name = "Email", Url = "/Admin/EmailSettings", IconCls = "fa-envelope", Order = 4)]
        public ActionResult EmailSettings()
        {
            var model = _settingsService.GetByKey<SmtpSettings>(Constants.SMTPSettingsKey);
            if(model == null)
                model = new SmtpSettings();
            return View(model);
        }

        [HttpPost]
        public ActionResult EmailSettings(SmtpSettings model)
        {
            if (ModelState.IsValid)
            {
                var settings = _settingsService.SetByKey<SmtpSettings>(Constants.SMTPSettingsKey, model);
                TempData["SuccessMessage"] = "Settings save successful.";
            }
            return View(model);
        }

        [AdminMenuItem(Name = "Logging", Url = "/Admin/Logging", IconCls = "fa-file-text-o", Order = 5)]
        public ActionResult Logging()
        {
            PrepareLogViewData();
            return View();
        }

        private void PrepareLogViewData()
        {
            var config = SetupHelper.LoadSetup();
            var levels = new Dictionary<string, int>();

            levels.Add("Trace", (int)LogLevel.Trace);
            levels.Add("Debug", (int)LogLevel.Debug);
            levels.Add("Information", (int)LogLevel.Information);
            levels.Add("Warning", (int)LogLevel.Warning);
            levels.Add("Error", (int)LogLevel.Error);
            levels.Add("Critical", (int)LogLevel.Critical);
            levels.Add("None", (int)LogLevel.None);

            ViewBag.LogLevels = levels;
            ViewBag.LogLevel = config.LoggingLevel;

            ViewBag.LogFiles = ListLogFiles();

        }

        [HttpPost]
        public ActionResult Logging(int logLevelValue, string logFileName, string operation)
        {   
            if(operation == "SetLog")
            {
                SetupHelper.LoadSetup();
                SetupHelper.LoggingLevel = logLevelValue;
                SetupHelper.SaveSetup();
                TempData["SuccessMessage"] = "Log Levels save successful. <a href='/Home/RestartHost'> Restart Site</a> for change effect.";
            }
            else
            {
                if (!string.IsNullOrEmpty(logFileName))
                { 
                    try
                    {
                        var logFilePath = GlobalConfig.ContentRootPath + "\\" + NccInfo.LogFolder + "\\" + logFileName;
                        var originalFileStream = System.IO.File.Open(logFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                        MemoryStream zipStream = new MemoryStream();
                        using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                        {
                            var zipEntry = zip.CreateEntry(logFileName);
                            using (var writer = new StreamWriter(zipEntry.Open()))
                            {
                                originalFileStream.Seek(0, SeekOrigin.Begin);
                                originalFileStream.CopyTo(writer.BaseStream);
                            }
                        }
                        zipStream.Seek(0, SeekOrigin.Begin);
                        return File(zipStream, "application/zip", logFileName + ".zip");
                         
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = ex.Message;
                    }
                    finally
                    {

                    }
                }
            }
            PrepareLogViewData();
            return View();
        }

        private Dictionary<string,string> ListLogFiles()
        {
            var dict = new Dictionary<string, string>();
            var logFolderPath = GlobalConfig.ContentRootPath + "\\" + NccInfo.LogFolder;
            var files = Directory.GetFiles(logFolderPath);
            foreach (var item in files)
            {
                var file = new FileInfo(item);
                dict.Add(file.Name, file.Name);
            }
            return dict;
        }

        public FileResult DownloadAllLogs()
        {
            var dict = new Dictionary<string, string>();
            var logFolderPath = GlobalConfig.ContentRootPath + "\\" + NccInfo.LogFolder;
            
            MemoryStream zipStream = new MemoryStream();
            using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                var files = Directory.GetFiles(logFolderPath);
                foreach (var item in files)
                {
                    var fi = new FileInfo(item);
                    var originalFileStream = System.IO.File.Open(item, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    var zipEntry = zip.CreateEntry(fi.Name);
                    using (var writer = new StreamWriter(zipEntry.Open()))
                    {
                        originalFileStream.Seek(0, SeekOrigin.Begin);
                        originalFileStream.CopyTo(writer.BaseStream);
                    }
                }
            }

            zipStream.Seek(0, SeekOrigin.Begin);
            return File(zipStream, "application/zip", "AllLogFiles.zip");
            
        }

        [HttpPost]
        public ActionResult Startup(StartupViewModel vmodel)
        {
            var setupConfig = SetupHelper.LoadSetup();
            setupConfig.StartupType = vmodel.StartupType;
            
            if (vmodel.StartupType == StartupTypes.Default)
            {
                setupConfig.StartupUrl =  vmodel.Default;
            }
            else if (vmodel.StartupType == StartupTypes.Page)
            {
                setupConfig.StartupUrl = "/" + vmodel.PageSlug;
            }
            else if (vmodel.StartupType == StartupTypes.Post)
            {
                setupConfig.StartupUrl = "/Post/" + vmodel.PostSlug;
            }
            else if (vmodel.StartupType == StartupTypes.Category)
            {
                setupConfig.StartupUrl = "/Category/" + vmodel.PageSlug;
            }
            else if (vmodel.StartupType == StartupTypes.Module)
            {
                setupConfig.StartupUrl = vmodel.ModuleSiteMenuUrl;
            }
            else
            {
                setupConfig.StartupUrl = "/CmsHome";
            }

            if (setupConfig.StartupUrl.Trim('/') == "" || setupConfig.StartupUrl.Trim().Trim('/').ToLower() == "home")
            {
                ViewBag.Message = "Incorrect value";
                return View(vmodel);
            }

            SetupHelper.UpdateSetup(setupConfig);
            var model = PrepareStartupViewData();

            return View(model);
        }
         
        public StartupViewModel PrepareStartupViewData()
        {
            var setupConfig = SetupHelper.LoadSetup();
            var model = new StartupViewModel();
            var moduleSiteMenuList = new List<SiteMenuItem>();
            var roleList = _roleManager.Roles.Select(x=> new { Name = x.Name, Value = x.Id }).ToList();

            model.Default = setupConfig.StartupUrl;
            model.StartupType = setupConfig.StartupType;

            model.Pages = new SelectList(_pageService.LoadAllActive(), "Slug", "Title", GetSlug(setupConfig.StartupUrl));
            model.Posts = new SelectList(_postService.LoadAllByPostStatusAndDate(NccPost.NccPostStatus.Published, DateTime.Now), "Slug", "Title", GetSlug(setupConfig.StartupUrl));
            model.Categories = new SelectList(_categoryService.LoadAllActive(), "Slug", "Title", GetSlug(setupConfig.StartupUrl));
            AdminMenuHelper.ModulesSiteMenus().Select(x => x.Value).ToList().ForEach(x => moduleSiteMenuList.AddRange(x));
            model.ModuleSiteMenus = new SelectList(moduleSiteMenuList, "Url", "Url", setupConfig.StartupUrl);
            model.Roles = new SelectList(roleList,"Value", "Name");

            ViewBag.RoleStartups = _startupService.LoadAll();

            ViewBag.DefaultChecked = "";
            ViewBag.PageChecked = "";
            ViewBag.CategoryChecked = "";
            ViewBag.PostChecked = "";
            ViewBag.ModuleChecked = "";

            if (setupConfig.StartupType == StartupTypes.Page)
            {
                ViewBag.PageChecked = "checked";
            }
            else if (setupConfig.StartupType == StartupTypes.Post)
            {
                ViewBag.PostChecked = "checked";
            }
            else if (setupConfig.StartupType == StartupTypes.Category)
            {
                ViewBag.CategoryChecked = "checked";
            }
            else if (setupConfig.StartupType == StartupTypes.Module)
            {
                ViewBag.ModuleChecked = "checked";
            }
            else
            {
                ViewBag.DefaultChecked = "checked";
            }

            return model;
        }
        
        public ActionResult RoleStartup()
        {
            var model = PrepareStartupViewData();
            return View(model);
        }

        [HttpPost]
        public ActionResult RoleStartup(StartupViewModel vmodel, long[] Roles)
        {
            foreach (var item in Roles)
            {
                var startup = new NccStartup();
                var startupType = (StartupType)Enum.Parse(typeof(StartupType), vmodel.RoleStartupType);
                var existingStartup = _startupService.Get(item, StartupFor.Role);
                var role = _roleManager.FindByIdAsync(item.ToString()).Result;

                if (existingStartup == null)
                {
                    startup.Role = role;
                    startup.StartupFor = StartupFor.Role;
                    startup.StartupType = startupType;
                    startup.StartupUrl = GetSelectedUrl(vmodel);
                    _startupService.Save(startup);
                }
                else
                {
                    existingStartup.Role = role;
                    existingStartup.StartupFor = StartupFor.Role;
                    existingStartup.StartupType = startupType;
                    existingStartup.StartupUrl = GetSelectedUrl(vmodel);
                    _startupService.Update(existingStartup);
                }                
            }
            var model = PrepareStartupViewData();
            return View(model);
        }

        private string GetSelectedUrl(StartupViewModel vmodel)
        {
            string url;
            if (vmodel.StartupType == StartupTypes.Default)
            {
                url = vmodel.Default;
            }
            else if (vmodel.StartupType == StartupTypes.Page)
            {
                url = "/" + vmodel.PageSlug;
            }
            else if (vmodel.StartupType == StartupTypes.Post)
            {
                url = "/Post/" + vmodel.PostSlug;
            }
            else if (vmodel.StartupType == StartupTypes.Category)
            {
                url = "/Category/" + vmodel.PageSlug;
            }
            else if (vmodel.StartupType == StartupTypes.Module)
            {
                url = vmodel.ModuleSiteMenuUrl;
            }
            else
            {
                url = "/CmsHome";
            }
            return url;
        }

        public ActionResult ManageUsers()
        {

            return View();
        }

        [AdminMenuItem(Name = "Maintenance Mode",Url = "/Admin/MaintenanceMode", Order = 10)]
        public IActionResult MaintenanceMode()
        {
            ViewBag.SetupConfig = SetupHelper.LoadSetup();
            return View();
        }

        [HttpPost]
        public IActionResult MaintenanceMode(string isMaintenanceMode, int maintenanceDuration, string maintenanceMessage)
        {
            var setupConfig = SetupHelper.LoadSetup();
            setupConfig.IsMaintenanceMode = !string.IsNullOrEmpty(isMaintenanceMode);
            setupConfig.MaintenanceDownTime = maintenanceDuration;
            setupConfig.MaintenanceMessage = maintenanceMessage;

            SetupHelper.UpdateSetup(setupConfig);
            ViewBag.Message = "Save successful";

            ViewBag.SetupConfig = setupConfig;
            return View();
        }

        #endregion

        #region Privet Methods

        public string GetSlug(string url)
        {
            var slug = "";            
            if (!string.IsNullOrEmpty(url))
            {
                var parts = url.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1)
                {
                    slug = parts[parts.Length-1];
                }
            }
            return slug;
        }

        #endregion

    }
}
