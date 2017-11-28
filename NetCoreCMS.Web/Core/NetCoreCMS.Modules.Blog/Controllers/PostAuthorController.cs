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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Events.Post;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.ShotCodes;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Modules.Blog.Events;

namespace NetCoreCMS.Core.Modules.Blog.Controllers
{
    [AdminMenu(IconCls = "fa-newspaper-o", Name = "Blog", Order = 1)]
    public class PostAuthorController : NccController
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

        public PostAuthorController(NccPostService nccPostService, NccPostDetailsService nccPostDetailsService, NccCategoryService nccCategoryService, NccTagService nccTagService, NccUserService nccUserService, NccShortCodeProvider nccShortCodeProvider, IMediator mediator, ILoggerFactory loggerFactory)
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
        [AdminMenuItem(Name = "My Post", Url = "/PostAuthor/Manage", IconCls = "fa-th-list", SubActions = new string[] { "ManageAjax" }, Order = 3)]
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

                recordsTotal = _nccPostService.Count(false, false, false, true, null, null, 0, 0, GlobalContext.GetCurrentUserId(), searchText);
                recordsFiltered = recordsTotal;
                List<NccPost> itemList = _nccPostService.Load(start, length, false, false, false, true, null, null, 0, 0, GlobalContext.GetCurrentUserId(), searchText, orderBy, orderDir);
                string controllerName = "PostAuthor";
                foreach (var item in itemList)
                {
                    var str = new List<string>();
                    var temp = "";
                    #region Title
                    temp = "";
                    if (GlobalContext.WebSite.IsMultiLangual)
                    {
                        foreach (var details in item.PostDetails)
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
                        str.Add(item.Parent.PostDetails.FirstOrDefault().Title);
                    else
                        str.Add("-");

                    //if (item.CreateBy == item.ModifyBy)
                    //{
                    //    str.Add(_nccUserService.Get(item.CreateBy)?.UserName);
                    //}
                    //else
                    //{
                    //    str.Add("<b>Cr:</b> " + _nccUserService.Get(item.CreateBy)?.UserName + "<br /><b>Mo:</b> " + _nccUserService.Get(item.ModifyBy)?.UserName);
                    //}
                    #region Categories
                    temp = "";
                    foreach (var cat in item.Categories)
                    {
                        if (temp != "") temp += ", ";
                        temp += cat.Category.Name;
                    }
                    str.Add(temp);
                    #endregion
                    #region Tags
                    temp = "";
                    foreach (var tag in item.Tags)
                    {
                        if (temp != "") temp += ", ";
                        temp += tag.Tag.Name;
                    }
                    str.Add(temp);
                    #endregion

                    str.Add(item.PostStatus == NccPost.NccPostStatus.Published ? NccPost.NccPostStatus.Published.ToString() + ": " + item.PublishDate.ToString("yyyy-MM-dd hh:mm tt") : "Update: " + item.ModificationDate.ToString("yyyy-MM-dd hh:mm tt"));

                    str.Add(item.Layout);
                    str.Add(item.PostType.ToString());

                    string actionLink = " <a href='" + Url.Action("CreateEdit", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-primary btn-outline'>Edit</a> ";
                    //if (item.Status == EntityStatus.Active)
                    //    actionLink += " <a href='" + Url.Action("StatusUpdate", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-danger btn-outline'>Inactive</a> ";
                    //else
                    //    actionLink += " <a href='" + Url.Action("StatusUpdate", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-success btn-outline'>Active</a> ";
                    //actionLink += " <a href='" + Url.Action("Delete", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-danger'>Delete</a> ";
                    if (GlobalContext.WebSite.IsMultiLangual == true)
                    {
                        actionLink += "";
                        foreach (var Details in item.PostDetails)
                        {
                            actionLink += " <a href='/" + Details.Language + "/Post/" + Details.Slug + "' target='_blank' class='btn btn-outline btn-info btn-xs'><i class='fa fa-eye'></i> " + Details.Language + "</a> ";
                        }
                    }
                    else
                    {
                        actionLink += " <a href='/Post/" + item.PostDetails.Where(x => x.Language == GlobalContext.WebSite.Language).FirstOrDefault().Slug + "'  target='_blank' class='btn btn-outline btn-info btn-xs'><i class='fa fa-eye'></i> " + item.PostDetails.Where(x => x.Language == GlobalContext.WebSite.Language).FirstOrDefault().Language + "</a> ";
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

        [AdminMenuItem(Name = "Create Post", Url = "/PostAuthor/CreateEdit", IconCls = "fa-pencil-square-o", Order = 4)]
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
                var temp = _nccPostService.Get(Id);
                if (temp.CreateBy == GlobalContext.GetCurrentUserId())
                {
                    post = temp;
                }
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
            var oldModel = NccHelper.DeepClone(model);
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
                            _mediator.FirePostEvent(oldModel, PostEvent.Create, model, _logger);
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
                            _mediator.FirePostEvent(model, PostEvent.Create,null, _logger);
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

            if (ViewBag.MessageType == "SuccessMessage" && SubmitType.ToLower() == "saveandexit")
            {
                return RedirectToAction("Manage");
            }

            if (ViewBag.MessageType == "SuccessMessage")
            {
                return RedirectToAction("CreateEdit", new { Id = oldModel.Id });
            }

            SetPostViewData(model);
            return View(model);
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

            ViewBag.PostStatus = new SelectList(PostStatus.Where(x => x.Value != "2").ToList(), "Value", "Text", (int)post.PostStatus);

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