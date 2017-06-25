/*
 * Author: TecRT
 * Website: http://tecrt.com
 * Copyright (c) tecrt.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.HelloWorld.Models;
using NetCoreCMS.HelloWorld.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.HelloWorld.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "Notice", Order = 1)]
    public class NoticeHomeController : NccController
    {
        private NccNoticeService _nccNoticeService;
        public NoticeHomeController(NccNoticeService nccNoticeService)
        {
            _nccNoticeService = nccNoticeService;
        }

        [AllowAnonymous]        
        public ActionResult Index()
        {
            return View();
        }

        [AdminMenuItem(Name = "New Notice", Url = "/NoticeHome/Create", Order = 1)]
        public ActionResult Create()
        {
            return View(new NccNotice());
        }

        [HttpPost]
        public ActionResult Create(NccNotice notice)
        {
            if (ModelState.IsValid)
            {
                _nccNoticeService.Save(notice);
                TempData["SuccessMessage"] = "Notice save successfull.";
            }

            return View(notice);
        }

        [AdminMenuItem(Name = "Manage", Url = "/NoticeHome/Manage", Order = 2)]
        public ActionResult Manage()
        {
            return View();
        }
    }
}
