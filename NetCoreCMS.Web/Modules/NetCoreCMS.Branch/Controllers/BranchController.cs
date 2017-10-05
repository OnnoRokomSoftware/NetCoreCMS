/*
 * Author: OnnoRokom Software Ltd
 * Website: http://onnorokomsoftware.com
 * Copyright (c) onnorokomsoftware.com
 * License: Commercial
*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Branch.Models;
using NetCoreCMS.Branch.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.Branch.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    [AdminMenu(Name = "Branch", IconCls = "fa-cogs", Order = 100)]
    [SiteMenu(Name = "Branch", Order = 100)]
    public class BranchController : NccController
    {
        #region Initialization
        private NccBranchService _branchService;
        private NccBranch _nccBranch;
        private NccSettingsService _nccSettingsService;
        public BranchController(NccBranchService branchService, NccSettingsService nccSettingsService, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<BranchController>();
            _branchService = branchService;
            _nccSettingsService = nccSettingsService;
            var tempSettings = _nccSettingsService.GetByKey("Branches_Settings");
            if (tempSettings != null)
            {
                _nccBranch = JsonConvert.DeserializeObject<NccBranch>(tempSettings.Value);
            }
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Create", Url = "/Branch/CreateEdit", IconCls = "fa-arrow-right", Order = 1)]
        public ActionResult CreateEdit(long Id = 0)
        {
            NccBranch item = new NccBranch();
            if (Id > 0)
            {
                item = _branchService.Get(Id);
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(NccBranch model)
        {
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                //_nccSettingsService.SetByKey("UmsBdResult_Settings", JsonConvert.SerializeObject(model));
                //ViewBag.MessageType = "SuccessMessage";
                //ViewBag.Message = "Settings updated successfull.";
                if (model.Id > 0)
                {
                    _branchService.Update(model);
                    ViewBag.MessageType = "SuccessMessage";
                    ViewBag.Message = "Information updated successfull.";
                }
                else
                {
                    _branchService.Save(model);
                    ViewBag.MessageType = "SuccessMessage";
                    ViewBag.Message = "Information save successfull.";
                }
            }

            return View(model);
        }

        [AdminMenuItem(Name = "Manage", Url = "/Branch/Manage", IconCls = "fa-arrow-right", Order = 2)]
        public ActionResult Manage()
        {
            var itemList = _branchService.LoadAll().OrderBy(x => x.Order).ThenBy(x => x.Name).ToList();
            return View(itemList);
        }

        public ActionResult Delete(long Id)
        {
            _branchService.DeletePermanently(Id);
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Information deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region Public Site
        [AllowAnonymous]
        [SiteMenuItem(Name = "Branches", Url = "/Branch", Order = 1)]
        public ActionResult Index()
        {
            var itemList = _branchService.LoadAllActive().OrderBy(x => x.Order).ThenBy(x => x.Name).ToList();
            return View(itemList);
        }

        [AllowAnonymous]
        [SiteMenuItem(Name = "Branch Details", Url = "/Branch/Details", Order = 1)]
        public ActionResult Details(long Id = 0)
        {
            NccBranch item = new NccBranch();
            if (Id > 0)
            {
                item = _branchService.Get(Id);
            }
            return View(item);
        }
        #endregion
    }
}