using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.ImageSlider.Models;
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

        private NccImageSlider nccImageSlider;
        private List<NccImageSliderItem> nccImageSliderItemList = new List<NccImageSliderItem>();

        public ImageSliderHomeController(NccSettingsService nccSettingsService, ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<ImageSliderHomeController>();
            _nccSettingsService = nccSettingsService;
            var tempSettings = _nccSettingsService.GetByKey("NccImageSlider_Settings");
            if (tempSettings != null)
            {
                nccImageSlider = JsonConvert.DeserializeObject<NccImageSlider>(tempSettings.Value);
            }

            tempSettings = _nccSettingsService.GetByKey("NccImageSlider_Items");
            if (tempSettings != null)
            {
                nccImageSliderItemList = JsonConvert.DeserializeObject<List<NccImageSliderItem>>(tempSettings.Value);
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Slider Settings", Url = "/ImageSliderHome", IconCls = "", Order = 1)]
        public ActionResult Index()
        {
            if (nccImageSlider == null)
                nccImageSlider = new NccImageSlider();

            //nccImageSliderItemList = new List<NccImageSliderItem>();
            for (int i = nccImageSliderItemList.Count; i < nccImageSlider.TotalSlide; i++)
            {
                nccImageSliderItemList.Add(new NccImageSliderItem
                {
                    Path = "",
                    Description = "",
                    Order = i
                });
            }
            ViewBag.NccImageSliderItemList = nccImageSliderItemList;

            return View(nccImageSlider);
        }

        [HttpPost]
        public ActionResult Index(NccImageSlider model, string[] itemPath, string[] description)
        {
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";
            if (ModelState.IsValid)
            {
                if (model.TotalSlide > 1)
                {
                    nccImageSliderItemList = new List<NccImageSliderItem>();
                    int loopCount = 0;
                    for (int i = 0; i < itemPath.Count() && i < model.TotalSlide; i++)
                    {
                        nccImageSliderItemList.Add(new NccImageSliderItem
                        {
                            Path = itemPath[i],
                            Description = description[i],
                            Order = i
                        });
                        loopCount++;
                    }
                    nccImageSlider = model;
                    _nccSettingsService.SetByKey("NccImageSlider_Settings", JsonConvert.SerializeObject(model));

                    _nccSettingsService.SetByKey("NccImageSlider_Items", JsonConvert.SerializeObject(nccImageSliderItemList));
                    ViewBag.MessageType = "SuccessMessage";
                    ViewBag.Message = "Slider updated successfull.";
                }
                else
                {
                    ViewBag.Message = "Total slide must be greater then 1.";
                }
            }
            if (nccImageSlider == null)
                nccImageSlider = new NccImageSlider();


            for (int i = nccImageSliderItemList.Count; i < nccImageSlider.TotalSlide; i++)
            {
                nccImageSliderItemList.Add(new NccImageSliderItem
                {
                    Path = "",
                    Description = "",
                    Order = i
                });
            }

            ViewBag.NccImageSliderItemList = nccImageSliderItemList;
            return View(nccImageSlider);
        }
        #endregion

        #region User Panel
        #endregion
    }
}