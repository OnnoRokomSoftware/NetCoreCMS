using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.MatGallery.Models;
using NetCoreCMS.MatGallery.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetCoreCMS.MatGallery.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    //[AdminMenu(Name = "Gallery Management", IconCls = "", Order = 100)]
    public class MatGalleryApiController : NccController
    {
        #region Initialization
        private IHostingEnvironment _env;
        private NccSettingsService _nccSettingsService;
        private NccUserModuleService _nccUserModuleService;
        private readonly string _modulePath = "MatGallery\\Modules\\";

        private Settings settings;

        public MatGalleryApiController(ILoggerFactory factory, IHostingEnvironment env, NccSettingsService nccSettingsService, NccUserModuleService nccUserModuleService)
        {
            _logger = factory.CreateLogger<MatGalleryController>();
            _env = env;
            settings = new Settings();

            _nccSettingsService = nccSettingsService;
            _nccUserModuleService = nccUserModuleService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("Ncc_MatGallery_Settings");
                if (tempSettings != null)
                {
                    settings = JsonConvert.DeserializeObject<Settings>(tempSettings.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel

        #endregion

        #region user Panel

        #endregion

        #region API Panel
        [AllowAnonymous]
        [HttpGet]
        public JsonResult Modules(string key = "")
        {
            var itemList = _nccUserModuleService.LoadAllActive().OrderByDescending(x => x.Id)
                .Select(x => new
                {
                    x.ModuleId,
                    x.ModuleName,
                    x.ModuleTitle,
                    x.Description,
                    x.Version,
                    x.Category,
                    x.Author,
                    x.Email,
                    x.Website,
                    LastUpdate = x.ModificationDate
                }).ToList(); ;
            return Json(itemList);
        }

        [AllowAnonymous]
        [HttpGet]
        public FileResult DownloadModule(string key = "", string moduleId = "")
        {
            if (moduleId.Trim() != "")
            {
                var module = _nccUserModuleService.LoadAllByName(moduleId.Trim()).FirstOrDefault();
                if (module != null)
                {
                    var moduleFolderPath = Path.Combine(_env.WebRootPath, _modulePath + "\\" + module.ModuleId);
                    var fileName = module.ModuleId + "_v" + module.Version + ".zip";
                    var filepath = moduleFolderPath + "\\" + fileName;
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
                    return File(fileBytes, "application/x-msdownload", fileName);
                }
            }
            return null;
        }
        #endregion
    }
}