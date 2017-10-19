using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Events.Page;
using NetCoreCMS.Framework.Core.Models;
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
using static NetCoreCMS.Framework.Core.Models.NccPage;

namespace NetCoreCMS.Core.Modules.Cms.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "Page", IconCls = "fa-file-text-o", Order = 3)]
    public class CmsPageController : NccController
    {
        #region Initialization
        NccPageService _pageService;
        NccPageDetailsService _pageDetailsService;
        NccShortCodeProvider _nccShortCodeProvider;
        IMediator _mediator;

        public CmsPageController(NccPageService pageService, NccPageDetailsService nccPageDetailsService, NccShortCodeProvider nccShortCodeProvider, IMediator mediator, ILoggerFactory factory)
        {
            _pageService = pageService;
            _pageDetailsService = nccPageDetailsService;
            _nccShortCodeProvider = nccShortCodeProvider;
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
                    page = _mediator.Send(new OnPageShow(page)).Result;
                    foreach (var item in page.PageDetails)
                    {
                        item.Content = _nccShortCodeProvider.ReplaceShortContent(item.Content);
                    }
                    return View(page);
                }
            }

            TempData["Message"] = "Page not found";
            return Redirect(NccUrlHelper.AddLanguageToUrl(CurrentLanguage,"/CmsHome/ResourceNotFound"));
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "New page", Url = "/CmsPage/CreateEdit", IconCls = "fa-pencil-square-o", Order = 3)]
        public ActionResult CreateEdit(long Id = 0)
        {
            NccPage page = new NccPage();
            page.PublishDate = DateTime.Now;
            page.PageStatus = NccPageStatus.Draft;

            NccPageDetails nccPageDetails = new NccPageDetails();
            nccPageDetails.Language = GlobalConfig.WebSite.Language;
            page.PageDetails.Add(nccPageDetails);

            if (Id > 0)
            {
                page = _pageService.Get(Id);
            }

            if (GlobalConfig.WebSite.IsMultiLangual)
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
                var defaultPageDetails = model.PageDetails.Where(x => x.Language == GlobalConfig.WebSite.Language).FirstOrDefault();
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
                foreach (var item in model.PageDetails.Where(x => x.Language != GlobalConfig.WebSite.Language).ToList())
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

        [AdminMenuItem(Name = "Manage", Url = "/CmsPage/Manage", IconCls = "fa-th-list", Order = 1)]
        public ActionResult Manage()
        {
            var allPages = _pageService.LoadAll(false).OrderByDescending(p => p.Id).ToList();
            return View(allPages);
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