using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    [AdminMenu(Name = "Module", IconCls = "fa-th-large", Order = 7)]
    public class CmsModuleController : NccController
    {
        #region Initialization
        NccModuleService _moduleService;
        NccSettingsService _settingsService;
        ModuleManager moduleManager;
        List<IModule> _coreModules;
        List<IModule> _publicModules;
        IHostingEnvironment _hostingEnvironment;

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
            }
            return module;
        }

        public ActionResult DeactivateModule(string id)
        {
            var entity = UpdateModuleStatus(id, NccModule.NccModuleStatus.Inactive);
            if (entity != null)
            {
                var module = GlobalConfig.Modules.Where(x => x.ModuleId == entity.ModuleId).FirstOrDefault();
                if(module != null)
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
            if(entity != null)
            {
                var module = GlobalConfig.Modules.Where(x => x.ModuleId == entity.ModuleId).FirstOrDefault();

                module.Install(_settingsService, ExecuteQuery);
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

        #region PrivetMethods
        private string ExecuteQuery(NccDbQueryText query)
        {
            return _moduleService.ExecuteQuery(query);
        }
        #endregion
    }
}
