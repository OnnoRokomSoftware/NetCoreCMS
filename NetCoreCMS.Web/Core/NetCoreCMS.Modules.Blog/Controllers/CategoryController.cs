/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
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
using NetCoreCMS.Framework.Core.Mvc.Attributes;

namespace NetCoreCMS.Core.Modules.Media.Controllers
{
    [SiteMenu(Name = "Blog", Order = 100)]
    [AdminMenu(Name = "Blog", Order = 1)]
    public class CategoryController : NccController
    {
        NccCategoryService _nccCategoryService;
        NccCategoryDetailsService _nccCategoryDetailsService;
        NccPostService _nccPostService;
        NccUserService _nccUserService;
        ILoggerFactory _loggerFactory;

        public CategoryController(NccCategoryService nccCategoryService, NccCategoryDetailsService nccCategoryDetailsService, NccPostService nccPostService, NccUserService nccUserService, ILoggerFactory loggerFactory)
        {
            _nccCategoryService = nccCategoryService;
            _nccCategoryDetailsService = nccCategoryDetailsService;
            _nccPostService = nccPostService;
            _loggerFactory = loggerFactory;
            _nccUserService = nccUserService;
            _logger = _loggerFactory.CreateLogger<CategoryController>();
        }

        #region Admin Panel
        [AdminMenuItem(Name = "Category", Url = "/Category/Manage", IconCls = "", SubActions = new string[] { "ManageAjax", "CreateEdit", "StatusUpdate", "Delete" }, Order = 5)]
        public ActionResult Manage(long Id = 0)
        {
            return View();
        }

        [HttpPost]
        public JsonResult ManageAjax(int draw, int start, int length)
        {
            var data = new List<object>();
            long recordsTotal = 0;
            long recordsFiltered = 0;
            try
            {
                string searchText = HttpContext.Request.Form["search[value]"];
                searchText = searchText.Trim();
                #region OrderBy and Direction
                string orderBy = HttpContext.Request.Form["order[0][column]"];
                string orderDir = HttpContext.Request.Form["order[0][dir]"];
                if (!string.IsNullOrEmpty(orderDir))
                    orderDir = orderDir.ToUpper();
                if (!string.IsNullOrEmpty(orderBy))
                {
                    switch (orderBy)
                    {
                        case "0":
                            orderBy = "name";
                            break;
                        default:
                            orderBy = "";
                            break;
                    }
                }
                #endregion

                recordsTotal = _nccCategoryService.Count(false, searchText);
                recordsFiltered = recordsTotal;
                List<NccCategory> itemList = _nccCategoryService.Load(start, length, false, searchText, orderBy, orderDir);
                string controllerName = "Category";
                foreach (var item in itemList)
                {
                    var str = new List<string>();
                    var temp = "";
                    #region Title
                    temp = "";
                    if (GlobalContext.WebSite.IsMultiLangual)
                    {
                        foreach (var details in item.CategoryDetails)
                        {
                            if (!string.IsNullOrEmpty(temp))
                            {
                                temp += "<br />";
                            }
                            temp += "<b>" + details.Language + ":</b> " + details.Title;
                        }
                    }
                    else
                    {
                            temp = item.CategoryDetails.Where(x => x.Language == GlobalContext.WebSite.Language).FirstOrDefault().Title;
                    }
                    str.Add(temp);
                    #endregion
                    str.Add("<img src=" + item.CategoryImage + " style='max-width:300px;max-height:100px;'>");
                    if (item.Parent != null)
                        str.Add(item.Parent?.CategoryDetails?.FirstOrDefault()?.Title);
                    else
                        str.Add("-");

                    if (item.Posts.Count > 0)
                        str.Add(item.Posts.Count.ToString());
                    else
                        str.Add("-");

                    if (item.CreateBy == item.ModifyBy)
                    {
                        str.Add(_nccUserService.Get(item.CreateBy)?.UserName);
                    }
                    else
                    {
                        str.Add("<b>Cr:</b> " + _nccUserService.Get(item.CreateBy)?.UserName + "<br /><b>Mo:</b> " + _nccUserService.Get(item.ModifyBy)?.UserName);
                    }

                    if (item.CreationDate == item.ModificationDate)
                    {
                        str.Add(item.CreationDate.ToString("yyyy-MM-dd hh:mm tt"));
                    }
                    else
                    {
                        str.Add("<b>Cr:</b> " + item.CreationDate.ToString("yyyy-MM-dd hh:mm tt") + "<br /><b>Mo:</b> " + item.ModificationDate.ToString("yyyy-MM-dd hh:mm tt"));
                    }
                    
                    str.Add(item.Status.ToString());

                    string actionLink = " <a href='" + Url.Action("CreateEdit", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-primary btn-outline'>Edit</a> ";
                    if (item.Status == EntityStatus.Active)
                        actionLink += " <a href='" + Url.Action("StatusUpdate", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-danger btn-outline'>Inactive</a> ";
                    else
                        actionLink += " <a href='" + Url.Action("StatusUpdate", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-success btn-outline'>Active</a> ";
                    actionLink += " <a href='" + Url.Action("Delete", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-danger'>Delete</a> ";
                    if (GlobalContext.WebSite.IsMultiLangual == true)
                    {
                        actionLink += "";
                        foreach (var Details in item.CategoryDetails)
                        {
                            actionLink += " <a href='/" + Details.Language + "/Category/" + Details.Slug + "' target='_blank' class='btn btn-outline btn-info btn-xs'><i class='fa fa-eye'></i> " + Details.Language + "</a> ";
                        }
                    }
                    else
                    {
                        actionLink += " <a href='/Category/" + item.CategoryDetails.Where(x => x.Language == GlobalContext.WebSite.Language).FirstOrDefault().Slug + "'  target='_blank' class='btn btn-outline btn-info btn-xs'><i class='fa fa-eye'></i> " + GlobalContext.WebSite.Language + "</a> ";
                    }
                    str.Add(actionLink);
                    data.Add(str);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Json(new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                start = start,
                length = length,
                data = data
            });
        }

        public ActionResult CreateEdit(long Id = 0)
        {
            NccCategory category = new NccCategory();

            NccCategoryDetails nccCategoryDetails = new NccCategoryDetails();
            nccCategoryDetails.Language = GlobalContext.WebSite.Language;
            category.CategoryDetails.Add(nccCategoryDetails);

            if (Id > 0)
            {
                category = _nccCategoryService.Get(Id);
            }

            if (GlobalContext.WebSite.IsMultiLangual)
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
                var defaultCategoryDetails = model.CategoryDetails.Where(x => x.Language == GlobalContext.WebSite.Language).FirstOrDefault();
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
                foreach (var item in model.CategoryDetails.Where(x => x.Language != GlobalContext.WebSite.Language).ToList())
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
        [SiteMenuItem(Name = "Blog Categories", Url = "/Category/Index", Order = 1)]
        public ActionResult Index(string slug = "", int page = 0)
        {
            ViewBag.CurrentLanguage = CurrentLanguage;
            if (!string.IsNullOrEmpty(slug))
            {
                NccCategory item = _nccCategoryService.GetBySlug(slug);
                if (item != null)
                {
                    ViewBag.Slug = slug;
                    ViewBag.Category = item;
                    var postPerPage = GlobalContext.WebSite.WebSitePageSize;
                    var totalPost = _nccPostService.Count(true, true, true, true, null, null, item.Id, 0);
                    var allPost = _nccPostService.Load(page, postPerPage, true, true, true, true, null, null, item.Id, 0);

                    ViewBag.CurrentPage = page;
                    ViewBag.PostPerPage = postPerPage;
                    ViewBag.TotalPost = totalPost;

                    return View("Details", allPost);
                }
            }
            var ItemList = _nccCategoryService.LoadAll(true).OrderByDescending(x => x.CreationDate).ToList();
            return View(ItemList);
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