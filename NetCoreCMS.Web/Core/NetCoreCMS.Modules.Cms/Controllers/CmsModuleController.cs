using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Controllers
{
    public class CmsModuleController : NccController
    {
        NccModuleService _moduleService;
        ModuleManager moduleManager;
        List<IModule> _coreModules;
        List<IModule> _publicModules;
        IHostingEnvironment _hostingEnvironment;

        public CmsModuleController(NccModuleService moduleService, IHostingEnvironment hostingEnvironment)
        {
            _moduleService = moduleService;
            _hostingEnvironment = hostingEnvironment;
            moduleManager = new ModuleManager();
        }

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
                var module = GlobalConfig.Modules.Where(x => x.ModuleId == entity.ModuleId).FirstOrDefault();
                module.Activate();
                TempData["SuccessMessage"] = "Operation Successful. Restart Site";
            }
            else
            {
                TempData["ErrorMessage"] = "Error. Module is not found.";
            }

            return RedirectToAction("Index");
        }

        private NccModule UpdateModuleStatus(string id, Framework.Core.Models.NccModule.NccModuleStatus status)
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

        public ActionResult InactivateModule(string id)
        {
            var entity = UpdateModuleStatus(id, NccModule.NccModuleStatus.Inactive);
            if (entity != null)
            {
                var module = GlobalConfig.Modules.Where(x => x.ModuleId == entity.ModuleId).FirstOrDefault();
                module.Inactivate();
                TempData["SuccessMessage"] = "Operation Successful. Restart Site";
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
                module.Install();
                TempData["SuccessMessage"] = "Operation Successful. Restart Site";
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
            if(entity != null)
            {
                var module = GlobalConfig.Modules.Where(x => x.ModuleId == entity.ModuleId).FirstOrDefault();
                module.Uninstall();
                TempData["SuccessMessage"] = "Operation Successful. Restart Site";
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
            _moduleService.Remove(nId);
            TempData["SuccessMessage"] = "Operation Successful. Restart Site";
            return RedirectToAction("Index"); 
        }
    }
}
