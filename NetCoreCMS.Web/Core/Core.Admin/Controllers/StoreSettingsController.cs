/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/


using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Services;
using Core.Admin.Models.Dto;

namespace Core.Admin.Controllers
{
    [AdminMenu(Name = "Module", IconCls = "fa-tasks", Order = 6)]
    public class StoreSettingsController : NccController
    {
        INccSettingsService _nccSettingsService;
        private ILogger<StoreSettingsController> _logger;
        StoreSettings storeSettings;
        public StoreSettingsController(INccSettingsService nccSettingsService, ILoggerFactory factory)
        {
            _nccSettingsService = nccSettingsService;
            _logger = factory.CreateLogger<StoreSettingsController>();
            storeSettings = _nccSettingsService.GetByKey<StoreSettings>() ?? new StoreSettings();
        }

        [AdminMenuItem(Name = "Store Settings", IconCls = "fa-laptop", Order = 3)]
        public ActionResult Index()
        {
            return View(storeSettings);
        }

        [HttpPost]
        public ActionResult Index(StoreSettings model)
        {
            _nccSettingsService.SetByKey(model);
            ShowMessage("Settings Updated Successfully", MessageType.Success);
            return View(model);
        }
    }
}