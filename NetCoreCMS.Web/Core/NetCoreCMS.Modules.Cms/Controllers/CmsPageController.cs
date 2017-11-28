/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Events.Page;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.ShotCodes;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static NetCoreCMS.Framework.Core.Models.NccPage;

namespace NetCoreCMS.Core.Modules.Cms.Controllers
{
    [AdminMenu(Name = "Page", IconCls = "fa-file-text-o", Order = 3)]
    public class CmsPageController : NccController
    {
        #region Initialization
        NccPageService _pageService;
        NccPageDetailsService _pageDetailsService;
        NccShortCodeProvider _nccShortCodeProvider;
        private NccUserService _nccUserService;
        IMediator _mediator;

        public CmsPageController(NccPageService pageService, NccPageDetailsService nccPageDetailsService, NccShortCodeProvider nccShortCodeProvider, IMediator mediator, NccUserService nccUserService, ILoggerFactory factory)
        {
            _pageService = pageService;
            _pageDetailsService = nccPageDetailsService;
            _nccShortCodeProvider = nccShortCodeProvider;
            _nccUserService = nccUserService;
            _mediator = mediator;
            _logger = factory.CreateLogger<CmsPageController>();
        }
        #endregion

        #region Public View
        [AllowAnonymous]
        public ActionResult Index(string slug)
        {
            ViewBag.CurrentLanguage = CurrentLanguage;

            if (!string.IsNullOrEmpty(slug))
            {
                NccPage page = _pageService.GetBySlug(slug);
                if (page != null)
                {
                    var rsp = _mediator.SendAll(new OnPageShow(page)).Result;
                    page = rsp.LastOrDefault();
                    foreach (var item in page.PageDetails)
                    {
                        item.Content = _nccShortCodeProvider.ReplaceShortContent(item.Content);
                    }
                    return View(page);
                }
            }

            TempData["Message"] = "Page not found";
            return Redirect(NccUrlHelper.AddLanguageToUrl(CurrentLanguage, "/CmsHome/ResourceNotFound"));
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Manage", Url = "/CmsPage/Manage", IconCls = "fa-th-list", SubActions = new string[] { "ManageAjax", "Delete" }, Order = 1)]
        public ActionResult Manage()
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

                recordsTotal = _pageService.Count(false, searchText);
                recordsFiltered = recordsTotal;
                List<NccPage> itemList = _pageService.Load(start, length, false, searchText, orderBy, orderDir);
                string controllerName = "CmsPage";
                foreach (var item in itemList)
                {
                    var str = new List<string>();
                    var temp = "";
                    #region Title
                    temp = "";
                    if (GlobalContext.WebSite.IsMultiLangual)
                    {
                        foreach (var details in item.PageDetails)
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
                        temp = item.Name;
                    }
                    str.Add(temp);
                    #endregion                    
                    if (item.Parent != null)
                        str.Add(item.Parent.PageDetails.FirstOrDefault().Title);
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

                    str.Add(item.PageStatus == NccPageStatus.Published ? NccPageStatus.Published.ToString() + ": " + item.PublishDate.ToString("yyyy-MM-dd hh:mm tt") : "Update: " + item.ModificationDate.ToString("yyyy-MM-dd hh:mm tt"));
                    str.Add(item.Layout);
                    str.Add(item.PageType.ToString());
                    str.Add("[Page Id=\"" + item.Id + "\" Page]");

                    string actionLink = " <a href='" + Url.Action("CreateEdit", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-primary btn-outline'>Edit</a> ";
                    //if (item.Status == EntityStatus.Active)
                    //    actionLink += " <a href='" + Url.Action("StatusUpdate", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-danger btn-outline'>Inactive</a> ";
                    //else
                    //    actionLink += " <a href='" + Url.Action("StatusUpdate", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-success btn-outline'>Active</a> ";
                    actionLink += " <a href='" + Url.Action("Delete", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-danger'>Delete</a> ";
                    if (GlobalContext.WebSite.IsMultiLangual == true)
                    {
                        actionLink += "";
                        foreach (var PageDetails in item.PageDetails)
                        {
                            actionLink += " <a href='/" + PageDetails.Language + "/" + PageDetails.Slug + "' target='_blank' class='btn btn-outline btn-info btn-xs'><i class='fa fa-eye'></i> " + PageDetails.Language + "</a> ";
                        }
                    }
                    else
                    {
                        actionLink += " <a href='/" + item.PageDetails.Where(x => x.Language == GlobalContext.WebSite.Language).FirstOrDefault().Slug + "'  target='_blank' class='btn btn-outline btn-info btn-xs'><i class='fa fa-eye'></i> " + item.PageDetails.Where(x => x.Language == GlobalContext.WebSite.Language).FirstOrDefault().Language + "</a> ";
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

        [AdminMenuItem(Name = "New page", Url = "/CmsPage/CreateEdit", IconCls = "fa-pencil-square-o", Order = 3)]
        public ActionResult CreateEdit(long Id = 0)
        {
            NccPage page = new NccPage();
            page.PublishDate = DateTime.Now;
            page.PageStatus = NccPageStatus.Draft;

            NccPageDetails nccPageDetails = new NccPageDetails();
            nccPageDetails.Language = GlobalContext.WebSite.Language;
            page.PageDetails.Add(nccPageDetails);

            if (Id > 0)
            {
                page = _pageService.Get(Id);
            }

            if (GlobalContext.WebSite.IsMultiLangual)
            {
                foreach (var item in SupportedCultures.Cultures)
                {
                    var count = page.PageDetails.Where(x => x.Language == item.TwoLetterISOLanguageName).Count();
                    if (count <= 0)
                    {
                        NccPageDetails _nccPageDetails = new NccPageDetails();
                        _nccPageDetails.Language = item.TwoLetterISOLanguageName;
                        page.PageDetails.Add(_nccPageDetails);
                    }
                }
            }

            SetPageViewData(page);
            return View(page);
        }

        [HttpPost]
        public ActionResult CreateEdit(NccPage model, long ParentId, string SubmitType)
        {
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                bool isSuccess = true;


                #region For default language
                var defaultPageDetails = model.PageDetails.Where(x => x.Language == GlobalContext.WebSite.Language).FirstOrDefault();
                if (defaultPageDetails == null)
                {
                    isSuccess = false;
                    ViewBag.Message = "Default language Page can't be null";
                }
                else
                {
                    //title empty validation
                    if (string.IsNullOrEmpty(defaultPageDetails.Title))
                    {
                        isSuccess = false;
                        ViewBag.Message = "Default language Title can't be null";
                    }
                    //slug empty validation
                    if (isSuccess && string.IsNullOrEmpty(defaultPageDetails.Slug))
                    {
                        isSuccess = false;
                        ViewBag.Message = "Default language Slug can't be null";
                    }
                    //slug duplicate validation
                    if (isSuccess)
                    {
                        defaultPageDetails.Title = defaultPageDetails.Title.Trim();
                        defaultPageDetails.Slug = defaultPageDetails.Slug.Trim();
                        if (defaultPageDetails.MetaDescription == null || defaultPageDetails.MetaDescription.Trim() == "")
                        {
                            defaultPageDetails.MetaDescription = Regex.Replace(defaultPageDetails.Content, "<.*?>", String.Empty);
                            if (defaultPageDetails.MetaDescription.Length > 161)
                                defaultPageDetails.MetaDescription = defaultPageDetails.MetaDescription.Substring(0, 160);
                        }
                        model.Name = defaultPageDetails.Slug;

                        var temp = _pageDetailsService.Get(defaultPageDetails.Slug, defaultPageDetails.Language);
                        if (temp != null && temp.Id != defaultPageDetails.Id)
                        {
                            isSuccess = false;
                            ViewBag.Message = "Duplicate Slug found for language " + defaultPageDetails.Language;
                        }
                    }
                }
                #endregion

                #region Check validation for other languages 
                List<NccPageDetails> deletedList = new List<NccPageDetails>();
                foreach (var item in model.PageDetails.Where(x => x.Language != GlobalContext.WebSite.Language).ToList())
                {
                    if (item.Id == 0 && string.IsNullOrEmpty(item.Title) && string.IsNullOrEmpty(item.Slug) && string.IsNullOrEmpty(item.Content) && string.IsNullOrEmpty(item.MetaKeyword) && string.IsNullOrEmpty(item.MetaDescription))
                    {
                        deletedList.Add(item);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(item.Title))
                        {
                            isSuccess = false;
                            ViewBag.Message = "Title can't be null for language " + item.Language;
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
                            item.Title = item.Title.Trim();
                            item.Slug = item.Slug.Trim();
                            if (item.MetaDescription == null || item.MetaDescription.Trim() == "")
                            {
                                item.MetaDescription = Regex.Replace(item.Content, "<.*?>", String.Empty);
                                if (item.MetaDescription.Length > 161)
                                    item.MetaDescription = item.MetaDescription.Substring(0, 160);
                            }

                            var temp = _pageDetailsService.Get(item.Slug, item.Language);
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
                        model.PageDetails.Remove(item);
                    }
                }
                #endregion

                #region Operation
                if (isSuccess)
                {
                    try
                    {
                        var parrent = _pageService.Get(ParentId);
                        model.Parent = parrent;
                    }
                    catch (Exception) { }
                    if (model.Id > 0)
                    {

                        try
                        {
                            _pageService.Update(model);
                            ViewBag.MessageType = "SuccessMessage";
                            ViewBag.Message = "Page updated successful";
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Page create error.", ex.ToString());
                        }
                    }
                    else
                    {
                        try
                        {
                            _pageService.Save(model);
                            ViewBag.MessageType = "SuccessMessage";
                            ViewBag.Message = "Page save successful";
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Page create error.", ex.ToString());
                        }
                    }
                }
                #endregion
            }
            else
            {
                ViewBag.Message = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            if (ViewBag.MessageType == "SuccessMessage" && SubmitType.ToLower() == "publish")
            {
                return RedirectToAction("Manage");
            }

            SetPageViewData(model);
            return View(model);
        }

        public ActionResult Delete(long Id)
        {
            NccPage page = _pageService.Get(Id);
            //page.
            return View(page);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            var page = _pageService.Get(Id);
            foreach (var item in page.PageDetails)
            {
                var temp = _pageDetailsService.Get(item.Id);
                temp.Status = EntityStatus.Deleted;
                _pageDetailsService.Update(temp);

            }
            page.Status = EntityStatus.Deleted;
            _pageService.Update(page);

            //_pageService.DeletePermanently(Id);

            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Page deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region Helper
        private void SetPageViewData(NccPage page)
        {
            ViewBag.Layouts = new SelectList(ThemeHelper.ActiveTheme.Layouts, "Name", "Name", page.Layout);
            ViewBag.AllPages = new SelectList(_pageService.LoadAll().Where(p => p.PageStatus == NccPageStatus.Published && p.Id != page.Id), "Id", "Name", page.Parent != null ? page.Parent.Id : 0);

            var PageStatus = Enum.GetValues(typeof(NccPageStatus)).Cast<NccPageStatus>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            ViewBag.PageStatus = new SelectList(PageStatus, "Value", "Text", (int)page.PageStatus);

            var PageType = Enum.GetValues(typeof(NccPageType)).Cast<NccPageType>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            ViewBag.PageType = new SelectList(PageType, "Value", "Text", (int)page.PageType);

            ViewBag.DomainName = (Request.IsHttps == true ? "https://" : "http://") + Request.Host + "/";
        }
        #endregion
    }
}