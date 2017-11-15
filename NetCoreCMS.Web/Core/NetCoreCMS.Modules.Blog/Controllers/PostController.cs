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
using NetCoreCMS.Framework.Core.Events.Post;
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace NetCoreCMS.Core.Modules.Blog.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [SiteMenu(IconCls = "fa-newspaper-o", Name = "Blog", Order = 100)]
    [AdminMenu(IconCls = "fa-newspaper-o", Name = "Blog", Order = 3)]
    public class PostController : NccController
    {
        #region Initialization
        NccPostService _nccPostService;
        NccPostDetailsService _nccPostDetailsService;
        NccCategoryService _nccCategoryService;
        NccTagService _nccTagService;
        NccShortCodeProvider _nccShortCodeProvider;

        IMediator _mediator;
        NccUserService _nccUserService;
        ILoggerFactory _loggerFactory;

        public PostController(NccPostService nccPostService, NccPostDetailsService nccPostDetailsService, NccCategoryService nccCategoryService, NccTagService nccTagService, NccUserService nccUserService, NccShortCodeProvider nccShortCodeProvider, IMediator mediator, ILoggerFactory loggerFactory)
        {
            _nccPostService = nccPostService;
            _nccPostDetailsService = nccPostDetailsService;
            _nccCategoryService = nccCategoryService;
            _nccTagService = nccTagService;
            _nccShortCodeProvider = nccShortCodeProvider;
            _loggerFactory = loggerFactory;
            _nccUserService = nccUserService;
            _mediator = mediator;
            _logger = _loggerFactory.CreateLogger<BlogController>();
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "New post", Url = "/Post/CreateEdit", IconCls = "fa-pencil-square-o", Order = 3)]
        public ActionResult CreateEdit(long Id = 0)
        {
            NccPost post = new NccPost();
            post.PublishDate = DateTime.Now;
            post.PostStatus = NccPost.NccPostStatus.Draft;

            NccPostDetails nccPostDetails = new NccPostDetails();
            nccPostDetails.Language = GlobalContext.WebSite.Language;
            post.PostDetails.Add(nccPostDetails);

            if (Id > 0)
            {
                post = _nccPostService.Get(Id);
            }

            if (GlobalContext.WebSite.IsMultiLangual)
            {
                foreach (var item in SupportedCultures.Cultures)
                {
                    var count = post.PostDetails.Where(x => x.Language == item.TwoLetterISOLanguageName).Count();
                    if (count <= 0)
                    {
                        NccPostDetails _nccPostDetails = new NccPostDetails();
                        _nccPostDetails.Language = item.TwoLetterISOLanguageName;
                        post.PostDetails.Add(_nccPostDetails);
                    }
                }
            }

            SetPostViewData(post);
            return View(post);
        }

        [HttpPost]
        public ActionResult CreateEdit(NccPost model, long ParentId, long[] SelecetdCategories, string SelectedTags, string SubmitType)
        {
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                bool isSuccess = true;

                #region For default language
                var defaultPostDetails = model.PostDetails.Where(x => x.Language == GlobalContext.WebSite.Language).FirstOrDefault();
                if (defaultPostDetails == null)
                {
                    isSuccess = false;
                    ViewBag.Message = "Default language Post can't be null";
                }
                else
                {
                    //title empty validation
                    if (string.IsNullOrEmpty(defaultPostDetails.Title))
                    {
                        isSuccess = false;
                        ViewBag.Message = "Default language Title can't be null";
                    }
                    //slug empty validation
                    if (isSuccess && string.IsNullOrEmpty(defaultPostDetails.Slug))
                    {
                        isSuccess = false;
                        ViewBag.Message = "Default language Slug can't be null";
                    }
                    //slug duplicate validation
                    if (isSuccess)
                    {
                        defaultPostDetails.Title = defaultPostDetails.Title.Trim();
                        defaultPostDetails.Slug = defaultPostDetails.Slug.Trim();
                        model.Name = defaultPostDetails.Slug;

                        var temp = _nccPostDetailsService.Get(defaultPostDetails.Slug, defaultPostDetails.Language);
                        if (temp != null && temp.Id != defaultPostDetails.Id)
                        {
                            isSuccess = false;
                            ViewBag.Message = "Duplicate Slug found for language " + defaultPostDetails.Language;
                        }
                    }
                }
                #endregion

                #region Check validation for other languages 
                List<NccPostDetails> deletedList = new List<NccPostDetails>();
                foreach (var item in model.PostDetails.Where(x => x.Language != GlobalContext.WebSite.Language).ToList())
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

                            var temp = _nccPostDetailsService.Get(item.Slug, item.Language);
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
                        model.PostDetails.Remove(item);
                    }
                }
                #endregion

                #region Operation
                if (isSuccess)
                {
                    //Parent assign
                    try
                    {
                        var parrent = _nccPostService.Get(ParentId);
                        model.Parent = parrent;
                    }
                    catch (Exception) { }

                    //Categories Assign
                    try
                    {
                        if (SelecetdCategories.Count() > 0)
                        {
                            model.Categories = new List<NccPostCategory>();
                            foreach (var item in SelecetdCategories)
                            {
                                model.Categories.Add(new NccPostCategory() { Post = model, CategoryId = item });
                            }
                        }
                    }
                    catch (Exception) { }

                    //Tags Assign
                    try
                    {
                        if (!string.IsNullOrEmpty(SelectedTags))
                        {
                            model.Tags = new List<NccPostTag>();
                            string[] tagList = SelectedTags.Trim().Split(',');
                            foreach (var item in tagList)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    NccPostTag postTag = new NccPostTag();
                                    NccTag nccTag = _nccTagService.LoadAll(false, -1, item.Trim()).FirstOrDefault();
                                    if (nccTag == null)
                                    {
                                        nccTag = new NccTag();
                                        nccTag.Name = item.Trim();
                                        _nccTagService.Save(nccTag);
                                    }
                                    model.Tags.Add(new NccPostTag() { Post = model, Tag = nccTag });
                                }
                            }
                        }
                    }
                    catch (Exception) { }

                    if (model.Id > 0)
                    {
                        try
                        {
                            _nccPostService.Update(model);
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
                            _nccPostService.Save(model);
                            FireEvent("PostCreated",model);
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

            if (ViewBag.MessageType == "SuccessMessage")
            {
                return RedirectToAction("CreateEdit", new { Id = model.Id });
            }

            SetPostViewData(model);
            return View(model);
        }

        private void FireEvent(string actionType, NccPost model)
        {
            try
            {
                _mediator.Send(new OnPostCreated(model));
            }
            catch (Exception ex)
            {
                //Ignore exception
            }
        }

        [AdminMenuItem(Name = "Manage post", Url = "/Post/Manage", IconCls = "fa-th-list", Order = 1)]
        public ActionResult Manage()
        {
            var allPages = _nccPostService.LoadAll(false).OrderByDescending(p => p.PublishDate).ToList();
            return View(allPages);
        }

        public ActionResult Delete(long Id)
        {
            var page = _nccPostService.Get(Id);
            //page.
            return View(page);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            var post = _nccPostService.Get(Id);
            foreach (var item in post.PostDetails)
            {
                var temp = _nccPostDetailsService.Get(item.Id);
                temp.Status = EntityStatus.Deleted;
                _nccPostDetailsService.Update(temp);

            }
            post.Status = EntityStatus.Deleted;
            _nccPostService.Update(post);

            //_nccPostService.DeletePermanently(Id);

            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Post deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region Public View
        [AllowAnonymous]
        [SiteMenuItem(Name = "Blog Posts", Url = "/Post", Order = 1)]
        public ActionResult Index(string slug = "", int year = 0, int month = 0, int page = 0)
        {
            ViewBag.CurrentLanguage = CurrentLanguage;
            if (!string.IsNullOrEmpty(slug))
            {
                var post = _nccPostService.GetBySlug(slug);
                if (post != null)
                {
                    post = _mediator.Send(new OnPostShow(post)).Result;
                    SetShortCodeContent(post);
                    return View("Details", post);
                }
                TempData["Message"] = "Post not found";
            }
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            if (year > 0 && month > 0)
            {
                dateFrom = new DateTime(year, month, 1);
                dateTo = new DateTime(year, month + 1, 1).AddMinutes(-1);
            }
            else if (year > 0)
            {
                dateFrom = new DateTime(year, 1, 1);
                dateTo = new DateTime(year + 1, 1, 1).AddMinutes(-1);
            }
            var postPerPage = GlobalContext.WebSite.PerPagePostSize;
            var totalPost = _nccPostService.GetPublishedPostCount(dateFrom, dateTo);
            var allPost = _nccPostService.LoadPublished(page, postPerPage, true, true, dateFrom, dateTo);
            for (int i = 0; i < allPost.Count; i++)
            {
                allPost[i] = _mediator.Send(new OnPostShow(allPost[i])).Result;
            }
            ViewBag.CurrentPage = page;
            ViewBag.PostPerPage = postPerPage;
            ViewBag.TotalPost = totalPost;
            return View(allPost);
        }

        private void SetShortCodeContent(NccPost post)
        {
            foreach (var item in post.PostDetails)
            {
                item.Content = _nccShortCodeProvider.ReplaceShortContent(item.Content);
            }
        }
         
        [AllowAnonymous]
        public ActionResult Details(string slug)
        {
            ViewBag.CurrentLanguage = CurrentLanguage;
            var post = _nccPostService.GetBySlug(slug);
            if (post == null)
                return Redirect(Constants.NotFoundUrl);
            post = _mediator.Send(new OnPostShow(post)).Result;
            SetShortCodeContent(post);
            return View(post);
        }
        #endregion

        #region Helper
        private void SetPostViewData(NccPost post)
        {            
            ViewBag.Layouts = new SelectList(ThemeHelper.ActiveTheme.Layouts, "Name", "Name", post.Layout);

            ViewBag.CategoryList = _nccCategoryService.LoadAll();
            ViewBag.Categories = new SelectList(_nccCategoryService.LoadAll(), "Id", "Name", post.Categories);
            ViewBag.AllPosts = new SelectList(_nccPostService.LoadAll().Where(p => p.PostStatus == NccPost.NccPostStatus.Published && p.Id != post.Id), "Id", "Name", post.Parent != null ? post.Parent.Id : 0);

            var PostStatus = Enum.GetValues(typeof(NccPost.NccPostStatus)).Cast<NccPost.NccPostStatus>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            ViewBag.PostStatus = new SelectList(PostStatus, "Value", "Text", (int)post.PostStatus);

            var PostType = Enum.GetValues(typeof(NccPost.NccPostType)).Cast<NccPost.NccPostType>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            ViewBag.PostType = new SelectList(PostType, "Value", "Text", (int)post.PostType);

            ViewBag.DomainName = (Request.IsHttps == true ? "https://" : "http://") + Request.Host + "/Post/";
        }
        #endregion
    }
}
