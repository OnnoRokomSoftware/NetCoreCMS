using Microsoft.AspNetCore.Authorization;
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
using System.Linq;
using System.Text;

namespace NetCoreCMS.MatGallery.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    //[AdminMenu(Name = "Gallery Management", IconCls = "", Order = 100)]
    public class MatGalleryController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private NccUserModuleService _nccUserModuleService;

        private Settings settings;

        public MatGalleryController(NccSettingsService nccSettingsService, ILoggerFactory factory, NccUserModuleService nccUserModuleService)
        {
            _logger = factory.CreateLogger<MatGalleryController>();
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
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel

        #endregion

        #region user Panel

        #endregion
    }
}