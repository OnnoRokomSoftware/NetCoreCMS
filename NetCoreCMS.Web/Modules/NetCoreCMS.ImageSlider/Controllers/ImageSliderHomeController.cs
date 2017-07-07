using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.ImageSlider.Models;
using NetCoreCMS.ImageSlider.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.ImageSlider.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "ImageSlider", IconCls = "", Order = 100)]
    public class ImageSliderController : NccController
    {
        private NccImageSliderService _nccImageSliderService;
        public ImageSliderController(NccImageSliderService nccImageSliderService)
        {
            _nccImageSliderService = nccImageSliderService;
        }
        
        [AdminMenuItem(Name = "Manage", Url = "/ImageSliderHome/Manage", IconCls = "", Order = 2)]
        public ActionResult Index()
        {
            var all = _nccImageSliderService.LoadAll();
            return View(all);
        }


        [AdminMenuItem(Name = "Create New", Url = "/ImageSliderHome/CreateEdit", IconCls = "", Order = 1)]
        public ActionResult CreateEdit(long Id = 0)
        {            
            return View();
        }

        [HttpPost]
        public ActionResult CreateEdit(NccImageSlider item)
        {
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                if (item.Id > 0)
                {
                    _nccImageSliderService.Update(item);
                    ViewBag.MessageType = "SuccessMessage";
                    ViewBag.Message = "Notice updated successfull.";
                }
                else
                {
                    _nccImageSliderService.Save(item);
                    ViewBag.MessageType = "SuccessMessage";
                    ViewBag.Message = "Notice save successfull.";
                }
                //TempData["SuccessMessage"] = "Notice save successfull.";
            }

            return View(item);
        }


        public ActionResult Delete(long Id)
        {
            NccImageSlider item = _nccImageSliderService.Get(Id);
            //page.
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
    }
}