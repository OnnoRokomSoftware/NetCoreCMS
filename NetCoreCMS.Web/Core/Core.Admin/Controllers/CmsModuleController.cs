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
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Events.Modules;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Utility;

using MediatR;
using Newtonsoft.Json;

using Core.Admin.Models.ViewModels;
using Core.Admin.Models.Dto;

namespace Core.Admin.Controllers
{
    [AdminMenu(Name = "Module", IconCls = "fa-th-large", Order = 5)]
    public class CmsModuleController : NccController
    {
        #region Initialization
        private IHostingEnvironment _env;
        NccModuleService _moduleService;
        INccSettingsService _settingsService;
        ModuleManager moduleManager;
        //List<IModule> _coreModules;
        //List<IModule> _publicModules;
        IHostingEnvironment _hostingEnvironment;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly string _modulePath = "Modules\\";

        public CmsModuleController(
            NccModuleService moduleService,
            INccSettingsService settingsService,
            IHostingEnvironment hostingEnvironment,
            IMediator mediator,
            ILoggerFactory factory,
            IHostingEnvironment env
            )
        {
            _moduleService = moduleService;
            _settingsService = settingsService;
            _hostingEnvironment = hostingEnvironment;
            _mediator = mediator;
            _logger = factory.CreateLogger<CmsModuleController>();
            _env = env;
            moduleManager = new ModuleManager();
        }
        #endregion

        #region ModuleManagement
        [AdminMenuItem(Name = "Manage", IconCls = "fa-th-list", Order = 1)]
        public ActionResult Index()
        {
            var publicModuleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.ModuleFolder);
            var coreModuleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.CoreModuleFolder);
            //_coreModules = moduleManager.LoadModules(coreModuleFolder).OrderBy(x => x.ModuleTitle).ToList();
            //_publicModules = moduleManager.LoadModules(publicModuleFolder).OrderBy(x => x.ModuleTitle).ToList();

            ViewBag.Modules = _moduleService.LoadAll().OrderBy(x => x.ModuleTitle).ToList();

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Install()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Install(string file)
        {
            return View();
        }

        public ActionResult ActivateModule(string id)
        {
            var entity = UpdateModuleStatus(id, NccModule.NccModuleStatus.Active);
            if (entity != null)
            {
                FireEvent(ModuleActivity.Type.Activated, GlobalContext.GetModuleByModuleName(entity.ModuleName));
                TempData["ModuleSuccessMessage"] = "Operation Successful. Restart Site";
            }
            else
            {
                TempData["ErrorMessage"] = "Error. Module is not found.";
            }

            return RedirectToAction("Index");
        }

        private NccModule UpdateModuleStatus(string id, NccModule.NccModuleStatus status)
        {
            var nId = long.Parse(id);
            var module = _moduleService.Get(nId);
            if (module != null)
            {
                module.ModuleStatus = status;
                _moduleService.Update(module);
                var loadedModule = GlobalContext.Modules.Where(x => x.ModuleName == module.ModuleName).FirstOrDefault();
                if (loadedModule != null)
                {
                    loadedModule.ModuleStatus = (int)status;
                }
            }
            return module;
        }

        public ActionResult DeactivateModule(string id)
        {
            var entity = UpdateModuleStatus(id, NccModule.NccModuleStatus.Inactive);
            if (entity != null)
            {
                var module = GlobalContext.Modules.Where(x => x.ModuleName == entity.ModuleName).FirstOrDefault();
                if (module != null)
                {
                    module.Inactivate();
                    FireEvent(ModuleActivity.Type.Deactivated, GlobalContext.GetModuleByModuleName(entity.ModuleName));
                    TempData["ModuleSuccessMessage"] = "Operation Successful. Restart Site";
                }
                else
                {
                    TempData["ErrorMessage"] = "Module did not loaded.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Error. Module is not found.";
            }

            return RedirectToAction("Index");
        }

        public ActionResult InstallModule(string id)
        {

            var entity = _moduleService.Get(long.Parse(id));
            //var entity = UpdateModuleStatus(id, NccModule.NccModuleStatus.Active);
            if (entity != null)
            {
                var module = GlobalContext.Modules.Where(x => x.ModuleName == entity.ModuleName).FirstOrDefault();
                var retVal = module.Install(_settingsService, ExecuteQuery, CreateOrUpdateTable);
                if (retVal)
                {
                    module.ModuleStatus = (int)NccModule.NccModuleStatus.Installed;
                    FireEvent(ModuleActivity.Type.Installed, module);
                    TempData["ModuleSuccessMessage"] = "Operation Successful. Restart Site";
                    UpdateModuleStatus(id, NccModule.NccModuleStatus.Installed);
                }
                else
                {
                    TempData["ErrorMessage"] = "Error. Module install failed.";
                }

            }
            else
            {
                TempData["ErrorMessage"] = "Error. Module is not found.";
            }

            return RedirectToAction("Index");
        }

        public ActionResult UninstallModule(string id)
        {
            var entity = _moduleService.Get(long.Parse(id));
            var module = GlobalContext.Modules.Where(x => x.ModuleName == entity.ModuleName).FirstOrDefault();
            if (module != null)
            {
                module.RemoveTables(_settingsService, ExecuteQuery, DeleteTable);
                module.ModuleStatus = (int)NccModule.NccModuleStatus.UnInstalled;
            }

            if (entity != null)
            {
                _moduleService.DeletePermanently(entity.Id);
                FireEvent(ModuleActivity.Type.Uninstalled, module);
                TempData["ModuleSuccessMessage"] = "Operation Successful. Restart Site";
                UpdateModuleStatus(id, NccModule.NccModuleStatus.UnInstalled);
            }
            else
            {
                TempData["ErrorMessage"] = "Error. Module is not found.";
            }
            return RedirectToAction("Index");
        }

        public ActionResult RemoveModule(string id)
        {
            var nId = long.Parse(id);
            _moduleService.DeletePermanently(nId);
            TempData["ModuleSuccessMessage"] = "Operation Successful. Restart Site";
            FireEvent(ModuleActivity.Type.Removed, GlobalContext.GetModuleByModuleName(id));
            return RedirectToAction("Index");
        }

        #endregion

        #region Online Gallery
        public async Task<JsonResult> LoadNccStoreModules()
        {
            ApiResponse resp = new ApiResponse();
            resp.IsSuccess = false;
            resp.Message = "";
            try
            {
                try
                {
                    var settings = Settings.GetByKey<StoreSettings>();
                    resp.Data = NccRestClient.Get<List<NccModuleViewModel>>(settings.StoreBaseUrl, "NccStore/Api/Modules",new List<KeyValuePair<string, string>>() {
                        new KeyValuePair<string, string>("nccVersion",NccInfo.Version.ToString(4)),
                        new KeyValuePair<string, string>("key",settings.SecretKey),
                        new KeyValuePair<string, string>("domain", GlobalContext.WebSite.DomainName)
                    });
                    resp.IsSuccess = true;
                    resp.Message = "Success";
                }
                catch (HttpRequestException e)
                {
                    _logger.LogError($"Request exception: {e.Message}", e);
                }
            }
            catch (Exception) { }
            return Json(resp);
        }

        public async Task<JsonResult> DownloadModule(string moduleName, string version)
        {
            ApiResponse resp = new ApiResponse();
            resp.IsSuccess = true;
            resp.Message = "";
            if (moduleName.Trim() != "")
            {
                try
                {
                    var settings = Settings.GetByKey<StoreSettings>();
                    var data = NccRestClient.Download(settings.StoreBaseUrl, "NccStore/Api/DownloadModule", new List<KeyValuePair<string, string>>() {
                        new KeyValuePair<string, string>("moduleName", moduleName),
                        new KeyValuePair<string, string>("version", version),
                        new KeyValuePair<string, string>("key",settings.SecretKey),
                        new KeyValuePair<string, string>("domain", GlobalContext.WebSite.DomainName)
                    });

                    #region Download in temp folder
                    var tempFullFilepath = Path.Combine(_env.ContentRootPath, _modulePath + "\\_temp");
                    try
                    {
                        if (resp.IsSuccess == true)
                        {
                            if (!Directory.Exists(tempFullFilepath))
                            {
                                Directory.CreateDirectory(tempFullFilepath);
                            }

                            using (var stream = new FileStream(_modulePath + "\\_temp\\" + moduleName + ".zip", FileMode.Create, FileAccess.Write))
                            {
                                stream.Write(data, 0, data.Length);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        resp.IsSuccess = false;
                        resp.Message += "Module Download Failed.";
                    }
                    #endregion

                    #region Check & Create module folder
                    var finalFolderPath = Path.Combine(_env.ContentRootPath, _modulePath + "\\" + moduleName);
                    try
                    {
                        if (resp.IsSuccess == true)
                        {
                            if (!Directory.Exists(finalFolderPath))
                            {
                                Directory.CreateDirectory(finalFolderPath);
                            }
                            else
                            {
                                //Delete is not working perfectly. Bin folder dll file is in use cannot delete.
                                Thread.Sleep(2000);
                                try
                                {
                                    string strCmdText;
                                    strCmdText = "rd /s /q \"" + finalFolderPath + "\"";
                                    System.Diagnostics.Process.Start("CMD.exe", strCmdText);
                                    //Directory.Delete(finalFolderPath, true);
                                    //DeleteDirectory(finalFolderPath);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex.ToString());
                                    resp.IsSuccess = false;
                                    resp.Message += "Previous module folder delete failed.";
                                }
                                Directory.CreateDirectory(finalFolderPath);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        resp.IsSuccess = false;
                        resp.Message += "Module folder creation failed.";
                    }
                    #endregion

                    #region Unzip in folder location     
                    try
                    {
                        if (resp.IsSuccess == true)
                        {
                            ZipFile.ExtractToDirectory(tempFullFilepath + "\\" + moduleName + ".zip", finalFolderPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        resp.IsSuccess = false;
                        resp.Message += "Module Unzip Failed.";
                    }

                    try
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (System.IO.File.Exists(tempFullFilepath + "\\" + moduleName + ".zip") == true)
                            {
                                Thread.Sleep(2000);
                                System.IO.File.Delete(tempFullFilepath + "\\" + moduleName + ".zip");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        resp.IsSuccess = false;
                        resp.Message += "Downloaded temporary file remove failed.";
                    }
                    #endregion                       
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    resp.IsSuccess = false;
                    resp.Message = ex.Message;
                }
            }
            if (resp.IsSuccess == true)
            {
                resp.Message = "Module downloaded and restored successfully";
            }
            return Json(resp);
        }
        #endregion

        #region PrivetMethods
        private ModuleActivity FireEvent(ModuleActivity.Type type, IModule module)
        {
            try
            {
                var rsp = _mediator.SendAll(new OnModuleActivity(new ModuleActivity()
                {
                    ActivityType = type,
                    Module = module
                })).Result;
                return rsp.LastOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return null;
        }
        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                try
                {
                    System.IO.File.SetAttributes(file, FileAttributes.Normal);
                    System.IO.File.Delete(file);
                }
                catch (Exception) { }
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }
            try
            {
                Directory.Delete(target_dir, false);
            }
            catch (Exception) { }
        }

        private string ExecuteQuery(NccDbQueryText query)
        {
            return _moduleService.ExecuteQuery(query);
        }

        private int DeleteTable(Type modelType)
        {
            return _moduleService.DeleteTable(modelType);
        }

        private int CreateOrUpdateTable(Type modelType, bool DeleteUnusedColumns = true)
        {
            return _moduleService.CreateOrUpdateTable(modelType, DeleteUnusedColumns);
        }

        #endregion
    }
}