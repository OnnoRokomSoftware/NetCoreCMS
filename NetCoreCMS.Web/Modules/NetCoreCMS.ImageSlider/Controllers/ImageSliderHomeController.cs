using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.ImageSlider.Models;
using NetCoreCMS.Notice.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.ImageSlider.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "Image Slider", IconCls = "", Order = 100)]
    public class ImageSliderHomeController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private NccImageSliderService _nccImageSliderService;

        private NccImageSliderSettings nccImageSliderSettings;
        private List<NccImageSliderItem> nccImageSliderItemList = new List<NccImageSliderItem>();

        public ImageSliderHomeController(NccSettingsService nccSettingsService, ILoggerFactory factory, NccImageSliderService nccImageSliderService)
        {
            _logger = factory.CreateLogger<ImageSliderHomeController>();
            nccImageSliderSettings = new NccImageSliderSettings();

            _nccSettingsService = nccSettingsService;
            _nccImageSliderService = nccImageSliderService;
            try
            {
                var tempSettings = _nccSettingsService.GetByKey("NccImageSlider_Settings");
                if (tempSettings != null)
                {
                    nccImageSliderSettings = JsonConvert.DeserializeObject<NccImageSliderSettings>(tempSettings.Value);
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Slider Manage", Url = "/ImageSliderHome", IconCls = "", Order = 1)]
        public ActionResult Manage()
        {
            var itemList = _nccSettingsService.LoadAll().OrderByDescending(n => n.Id).ToList(); ;
            return View(itemList);
        }


        public ActionResult Delete(long Id)
        {
            NccImageSlider item = _nccImageSliderService.Get(Id);            
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            _nccImageSliderService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Item deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        #endregion
    }
}