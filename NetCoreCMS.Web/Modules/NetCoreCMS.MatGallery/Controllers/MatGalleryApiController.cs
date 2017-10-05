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
        private NccUserThemeService _nccUserThemeService;
        private readonly string _themePath = "MatGallery\\Themes\\";

        private Settings settings;

        public MatGalleryApiController(ILoggerFactory factory, IHostingEnvironment env, NccSettingsService nccSettingsService, NccUserModuleService nccUserModuleService, NccUserThemeService nccUserThemeService)
        {
            _logger = factory.CreateLogger<MatGalleryController>();
            _env = env;
            settings = new Settings();

            _nccSettingsService = nccSettingsService;
            _nccUserModuleService = nccUserModuleService;
            _nccUserThemeService = nccUserThemeService;
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
        #region User Module
        [AllowAnonymous]
        [HttpGet]
        public JsonResult Modules(string key = "")
        {
            var itemList = _nccUserModuleService.LoadAll(true).OrderByDescending(x => x.Id)
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
                var module = _nccUserModuleService.LoadAll(true, 0, moduleId.Trim()).FirstOrDefault();
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
        #region User Theme
        [AllowAnonymous]
        [HttpGet]
        public JsonResult Themes(string key = "")
        {
            var itemList = _nccUserThemeService.LoadAll(true).OrderByDescending(x => x.Id)
                .Select(x => new
                {
                    x.ThemeId,
                    x.ThemeName,
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
        public FileResult DownloadTheme(string key = "", string themeId = "")
        {
            if (themeId.Trim() != "")
            {
                var theme = _nccUserThemeService.LoadAll(true, 0, themeId.Trim()).FirstOrDefault();
                if (theme != null)
                {
                    var moduleFolderPath = Path.Combine(_env.WebRootPath, _modulePath + "\\" + theme.ThemeId);
                    var fileName = theme.ThemeId + "_v" + theme.Version + ".zip";
                    var filepath = moduleFolderPath + "\\" + fileName;
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
                    return File(fileBytes, "application/x-msdownload", fileName);
                }
            }
            return null;
        }
        #endregion
        #endregion
    }
}