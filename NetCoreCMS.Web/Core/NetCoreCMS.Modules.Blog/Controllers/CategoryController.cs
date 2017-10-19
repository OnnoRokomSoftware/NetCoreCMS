/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Utility;
using System;
using NetCoreCMS.Framework.Core.Mvc.Extensions;
using System.Linq;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Themes;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Setup;
using System.Collections.Generic;

namespace NetCoreCMS.Core.Modules.Media.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [SiteMenu(Name = "Blog", Order = 100)]
    [AdminMenu(Name = "Blog", Order = 5)]
    public class CategoryController : NccController
    {
        NccCategoryService _nccCategoryService;
        NccCategoryDetailsService _nccCategoryDetailsService;
        NccUserService _nccUserService;
        ILoggerFactory _loggerFactory;

        public CategoryController(NccCategoryService nccCategoryService, NccCategoryDetailsService nccCategoryDetailsService, NccUserService nccUserService, ILoggerFactory loggerFactory)
        {
            _nccCategoryService = nccCategoryService;
            _nccCategoryDetailsService = nccCategoryDetailsService;
            _loggerFactory = loggerFactory;
            _nccUserService = nccUserService;
            _logger = _loggerFactory.CreateLogger<CategoryController>();
        }

        #region Admin Panel
        [AdminMenuItem(Name = "Category", Url = "/Category/Manage", IconCls = "", Order = 5)]
        public ActionResult Manage(long Id = 0)
        {
            //SetCategoryViewData();
            NccCategory item = new NccCategory();

            if (Id > 0)
                item = _nccCategoryService.Get(Id);
            ViewBag.Category = item;
            var itemList = _nccCategoryService.LoadAll(false).OrderBy(x => x.Name).ToList();
            return View(itemList);
        }

        public ActionResult CreateEdit(long Id = 0)
        {
            NccCategory category = new NccCategory();

            NccCategoryDetails nccCategoryDetails = new NccCategoryDetails();
            nccCategoryDetails.Language = GlobalConfig.WebSite.Language;
            category.CategoryDetails.Add(nccCategoryDetails);

            if (Id > 0)
            {
                category = _nccCategoryService.Get(Id);
            }

            if (GlobalConfig.WebSite.IsMultiLangual)
            {
                foreach (var item in SupportedCultures.Cultures)
                {
                    var count = category.CategoryDetails.Where(x => x.Language == item.TwoLetterISOLanguageName).Count();
                    if (count <= 0)
                    {
                        NccCategoryDetails _nccCategoryDetails = new NccCategoryDetails();
                        _nccCategoryDetails.Language = item.TwoLetterISOLanguageName;
                        category.CategoryDetails.Add(_nccCategoryDetails);
                    }
                }
            }

            SetCategoryViewData(category);
            return View(category);
        }

        [HttpPost]
        public ActionResult CreateEdit(NccCategory model, long ParentId, string save)
        {
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                bool isSuccess = true;

                #region For default language
                var defaultCategoryDetails = model.CategoryDetails.Where(x => x.Language == GlobalConfig.WebSite.Language).FirstOrDefault();
                if (defaultCategoryDetails == null)
                {
                    isSuccess = false;
                    ViewBag.Message = "Default language category can't be null";
                }
                else
                {
                    //title empty validation
                    if (string.IsNullOrEmpty(defaultCategoryDetails.Name))
                    {
                        isSuccess = false;
                        ViewBag.Message = "Default language Name can't be null";
                    }
                    //slug empty validation
                    if (isSuccess && string.IsNullOrEmpty(defaultCategoryDetails.Slug))
                    {
                        isSuccess = false;
                        ViewBag.Message = "Default language Slug can't be null";
                    }
                    //slug duplicate validation
                    if (isSuccess)
                    {
                        defaultCategoryDetails.Name = defaultCategoryDetails.Name.Trim();
                        defaultCategoryDetails.Slug = defaultCategoryDetails.Slug.Trim();
                        model.Name = defaultCategoryDetails.Name;

                        var temp = _nccCategoryDetailsService.Get(defaultCategoryDetails.Slug, defaultCategoryDetails.Language);
                        if (temp != null && temp.Id != defaultCategoryDetails.Id)
                        {
                            isSuccess = false;
                            ViewBag.Message = "Duplicate Slug found for language " + defaultCategoryDetails.Language;
                        }
                    }
                }
                #endregion

                #region Check validation for other languages 
                List<NccCategoryDetails> deletedList = new List<NccCategoryDetails>();
                foreach (var item in model.CategoryDetails.Where(x => x.Language != GlobalConfig.WebSite.Language).ToList())
                {
                    if (item.Id == 0 && string.IsNullOrEmpty(item.Title) && string.IsNullOrEmpty(item.Slug) && string.IsNullOrEmpty(item.Name))
                    {
                        deletedList.Add(item);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(item.Name))
                        {
                            isSuccess = false;
                            ViewBag.Message = "Name can't be null for language " + item.Language;
                        }
                        //slug empty validation
                        if (isSuccess && string.IsNullOrEmpty(item.Slug))
                        {
                            isSuccess = false;
                            ViewBag.Message = "Slug can't be null for language " + item.Language;
                        }
                        //slug duplicate validation
                        if (isSuccess)
                        {
                            item.Name = item.Name.Trim();
                            item.Slug = item.Slug.Trim();

                            var temp = _nccCategoryDetailsService.Get(item.Slug, item.Language);
                            if (temp != null && temp.Id != item.Id)
                            {
                                isSuccess = false;
                                ViewBag.Message = "Duplicate Slug found for language " + item.Language;
                            }
                        }
                    }
                }

                //Remove empty
                if (isSuccess)
                {
                    foreach (var item in deletedList)
                    {
                        model.CategoryDetails.Remove(item);
                    }
                }
                #endregion

                #region Operation
                if (isSuccess)
                {
                    try
                    {
                        var parrent = _nccCategoryService.Get(ParentId);
                        model.Parent = parrent;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }

                    if (model.Id > 0)
                    {
                        try
                        {
                            _nccCategoryService.Update(model);
                            ViewBag.MessageType = "SuccessMessage";
                            ViewBag.Message = "Data updated successfull.";
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Category update error.", ex.ToString());
                        }
                    }
                    else
                    {
                        try
                        {
                            _nccCategoryService.Save(model);
                            ViewBag.MessageType = "SuccessMessage";
                            ViewBag.Message = "Data saved successfull.";
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Category create error.", ex.ToString());
                        }
                    }
                }
                #endregion
            }
            else
            {
                ViewBag.Message = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            if (ViewBag.MessageType == "SuccessMessage" && save == "Save")
            {
                return RedirectToAction("Manage");
            }

            if (ViewBag.MessageType == "SuccessMessage" && save != "Save")
            {
                return RedirectToAction("CreateEdit");
            }

            SetCategoryViewData(model);
            return View(model);
        }

        public ActionResult StatusUpdate(long Id = 0)
        {
            if (Id > 0)
            {
                var item = _nccCategoryService.Get(Id);
                if (item.Status == EntityStatus.Active)
                {
                    item.Status = EntityStatus.Inactive;
                    ViewBag.Message = "In-activated successfull.";
                }
                else
                {
                    item.Status = EntityStatus.Active;
                    ViewBag.Message = "Activated successfull.";
                }

                _nccCategoryService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(long Id)
        {
            NccCategory item = _nccCategoryService.Get(Id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            var itemList = _nccCategoryService.LoadByParrentId(Id);
            if (itemList != null && itemList.Count > 0)
            {
                ViewBag.MessageType = "ErrorMessage";
                ViewBag.Message = "Item has child. You cann't delete this item.";
                var item = _nccCategoryService.Get(Id);
                SetCategoryViewData(item);
                return View(item);
            }
            else
            {
                var item = _nccCategoryService.Get(Id);
                item.Status = EntityStatus.Deleted;
                _nccCategoryService.Update(item);
                //_nccCategoryService.DeletePermanently(Id);
                ViewBag.MessageType = "SuccessMessage";
                ViewBag.Message = "Item deleted successful";
                return RedirectToAction("Manage");
            }
        }
        #endregion

        #region Public View
        [AllowAnonymous]
        [SiteMenuItem(Name = "Blog Categories", Url = "/Category", Order = 1)]
        public ActionResult Index(string slug = "")
        {
            ViewBag.CurrentLanguage = CurrentLanguage;
            if (!string.IsNullOrEmpty(slug))
            {
                NccCategory item = _nccCategoryService.GetWithPost(slug);
                if (item != null)
                {
                    return View("Details", item);
                }
            }
            var allPost = _nccCategoryService.LoadAll(true).OrderByDescending(x => x.CreationDate).ToList();
            return View(allPost);
        }
        #endregion

        #region Helper
        private void SetCategoryViewData(NccCategory category)
        {
            ViewBag.DomainName = (Request.IsHttps == true ? "https://" : "http://") + Request.Host + "/Category/";
            var parentList = _nccCategoryService.LoadAll().Where(x => x.Id != category.Id).ToList();
            ViewBag.AllCategories = new SelectList(parentList, "Id", "Name", category.Parent != null ? category.Parent.Id : 0);
            //var cultures = SupportedCultures.Cultures;
            //ViewBag.Languages = new SelectList(cultures.Select(x => new { Value = x.TwoLetterISOLanguageName.ToLower(), Text = x.DisplayName }).ToList(), "Value", "Text", SetupHelper.Language);
            //ViewBag.CurrentLanguage = CurrentLanguage;
        }
        #endregion
    }
}