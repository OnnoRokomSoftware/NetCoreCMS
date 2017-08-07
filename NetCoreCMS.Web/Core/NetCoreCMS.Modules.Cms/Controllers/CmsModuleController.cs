using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreCMS.Modules.Cms.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    [AdminMenu(Name = "Module", IconCls = "fa-th-large", Order = 7)]
    public class CmsModuleController : NccController
    {
        #region Initialization
        private IHostingEnvironment _env;
        NccModuleService _moduleService;
        NccSettingsService _settingsService;
        ModuleManager moduleManager;
        List<IModule> _coreModules;
        List<IModule> _publicModules;
        IHostingEnvironment _hostingEnvironment;
        private readonly string _modulePath = "MatGallery\\Modules\\";

        public CmsModuleController(
            NccModuleService moduleService,
            NccSettingsService settingsService,
            IHostingEnvironment hostingEnvironment,
            ILoggerFactory factory
            )
        {
            _moduleService = moduleService;
            _settingsService = settingsService;
            _hostingEnvironment = hostingEnvironment;
            _logger = factory.CreateLogger<CmsModuleController>();
            moduleManager = new ModuleManager();
        }
        #endregion

        [AdminMenuItem(Name = "Manage", Url = "/CmsModule", IconCls = "fa-th-list", Order = 1)]
        public ActionResult Index()
        {
            var publicModuleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.ModuleFolder);
            var coreModuleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.CoreModuleFolder);
            ViewBag.Modules = _moduleService.LoadAll();
            _coreModules = moduleManager.LoadModules(coreModuleFolder);
            _publicModules = moduleManager.LoadModules(publicModuleFolder);

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
                var loadedModule = GlobalConfig.Modules.Where(x => x.ModuleId == module.ModuleId).FirstOrDefault();
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
                var module = GlobalConfig.Modules.Where(x => x.ModuleId == entity.ModuleId).FirstOrDefault();
                if (module != null)
                {
                    module.Inactivate();
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

            var entity = UpdateModuleStatus(id, NccModule.NccModuleStatus.Installed);
            if (entity != null)
            {
                var module = GlobalConfig.Modules.Where(x => x.ModuleId == entity.ModuleId).FirstOrDefault();
                module.Install(_settingsService, ExecuteQuery);
                module.ModuleStatus = (int)NccModule.NccModuleStatus.Installed;
                TempData["ModuleSuccessMessage"] = "Operation Successful. Restart Site";
            }
            else
            {
                TempData["ErrorMessage"] = "Error. Module is not found.";
            }

            return RedirectToAction("Index");
        }

        public ActionResult UninstallModule(string id)
        {
            var entity = UpdateModuleStatus(id, NccModule.NccModuleStatus.UnInstalled);
            var module = GlobalConfig.Modules.Where(x => x.ModuleId == entity.ModuleId).FirstOrDefault();
            if (module != null)
            {
                module.Uninstall(_settingsService, ExecuteQuery);
                module.ModuleStatus = (int)NccModule.NccModuleStatus.UnInstalled;
            }

            if (entity != null)
            {
                TempData["ModuleSuccessMessage"] = "Operation Successful. Restart Site";
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
            return RedirectToAction("Index");
        }


        public async Task<JsonResult> DownloadModule(string key = "", string moduleId = "", string moduleName = "")
        {
            ApiResponse resp = new ApiResponse();
            resp.IsSuccess = false;
            resp.Message = "Process Failed. Please try again.";
            if (moduleId.Trim() != "")
            {
                try
                {
                    string url = "http://localhost:60180/MatGalleryApi/DownloadModule?moduleId=" + moduleId;
                    #region Download in temp folder
                    using (var client = new HttpClient())
                    {
                        using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                        {
                            using (Stream contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync(), stream = new FileStream("Modules\\_temp\\" + moduleName + ".zip", FileMode.Create, FileAccess.Write))
                            {
                                await contentStream.CopyToAsync(stream);
                            }
                        }
                    }
                    #endregion

                    #region Check & Create module folder

                    #endregion

                    #region Unzip in folder location
                    //var finalFolderPath = Path.Combine(_env.WebRootPath, Path.Combine(_env.WebRootPath, _tempPath + finalFolderName));
                    //if (!Directory.Exists(unzipFolderPath))
                    //{
                    //    Directory.CreateDirectory(unzipFolderPath);
                    //    Directory.CreateDirectory(finalFolderPath);
                    //}
                    //else
                    //{
                    //    isSuccess = false;
                    //    responseMessage = "Redundant ZipFile found in server.";
                    //    //stop the process, because duplicate name situation occered.
                    //}
                    //if (isSuccess)
                    //{
                    //    ZipFile.ExtractToDirectory(tempFullFilepath, unzipFolderPath);
                    //}
                    #endregion

                    resp.IsSuccess = true;
                    resp.Message = "Module Downloaded complete.";
                }
                catch (Exception ex)
                {

                }

            }
            return Json(resp);
        }

        #region PrivetMethods
        private string ExecuteQuery(NccDbQueryText query)
        {
            return _moduleService.ExecuteQuery(query);
        }
        #endregion
    }
}