using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
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
            moduleManager = new ModuleManager();
        }

        public ActionResult Index()
        {
            var publicModuleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.ModuleFolder);
            var coreModuleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.CoreModuleFolder);
            var modules = _moduleService.LoadAll();
            _coreModules = moduleManager.LoadModules(coreModuleFolder);
            _publicModules = moduleManager.LoadModules(publicModuleFolder);

            return View(modules);
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}
