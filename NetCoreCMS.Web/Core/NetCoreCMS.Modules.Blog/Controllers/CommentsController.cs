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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static NetCoreCMS.Framework.Core.Models.NccComment;
using static NetCoreCMS.Framework.Core.Models.NccPage;
using static NetCoreCMS.Framework.Core.Models.NccPost;


namespace NetCoreCMS.Core.Modules.Blog.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]    
    [AdminMenu(IconCls = "fa-newspaper-o", Name = "Blog", Order = 3)]
    public class CommentsController : NccController
    {
        #region Initialization
        NccCommentsService _nccCommentsService;
        NccPostService _postService;

        NccUserService _nccUserService;
        ILoggerFactory _loggerFactory;

        UserManager<NccUser> _userManager;

        public CommentsController(NccCommentsService nccCommentsService, NccPostService postService, NccUserService nccUserService, ILoggerFactory loggerFactory, UserManager<NccUser> UserManager)
        {
            _nccCommentsService = nccCommentsService;
            _postService = postService;
            _loggerFactory = loggerFactory;
            _nccUserService = nccUserService;
            _userManager = UserManager;
            _logger = _loggerFactory.CreateLogger<BlogController>();
        }
        #endregion

        #region Admin Panel
        public ActionResult CreateEdit(long Id = 0)
        {
            NccComment item = new NccComment();            
            item.CommentStatus = NccComment.NccCommentStatus.Pending;

            if (Id > 0)
            {
                item = _nccCommentsService.Get(Id);
            }
            else
            {
                return RedirectToAction("Manage");
            }

            SetPageViewData(item);
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(NccComment model, long PostId, long AuthorId, string SubmitType)
        {
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            if (PostId > 0)
                model.Post = _postService.Get(PostId);
            if (AuthorId > 0)
                model.Author = _userManager.FindByIdAsync(AuthorId.ToString()).Result;
            var res = model.ValidationResults();
            if (ModelState.IsValid)
            {
                bool isSuccess = true;

                #region Operation
                if (isSuccess)
                {
                    //Parent assign
                    //try
                    //{
                    //    var parrent = _nccCommentsService.Get(ParentId);
                    //    model.Parent = parrent;
                    //}
                    //catch (Exception) { }

                    if (model.Id > 0)
                    {
                        try
                        {
                            _nccCommentsService.Update(model);
                            ViewBag.MessageType = "SuccessMessage";
                            ViewBag.Message = "Information updated successful";
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Create error.", ex.ToString());
                        }
                    }
                    //else
                    //{
                    //    try
                    //    {
                    //        _nccCommentsService.Save(model);
                    //        ViewBag.MessageType = "SuccessMessage";
                    //        ViewBag.Message = "Page save successful";
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        _logger.LogError("Page create error.", ex.ToString());
                    //    }
                    //}
                }
                #endregion
            }
            else
            {
                ViewBag.Message = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
             }

            if (ViewBag.MessageType == "SuccessMessage")
            {
                return RedirectToAction("Manage");
            }

            SetPageViewData(model);
            return View(model);
        }

        [AdminMenuItem(Name = "Manage Comments", Url = "/Comments/Manage", IconCls = "fa-th-list", Order = 9)]
        public ActionResult Manage()
        {
            var allPages = _nccCommentsService.LoadAll(false).OrderByDescending(p => p.CreationDate).ToList();
            var CommentStatus = Enum.GetValues(typeof(NccCommentStatus)).Cast<NccCommentStatus>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            ViewBag.CommentStatus = new SelectList(CommentStatus, "Value", "Text");

            return View(allPages);
        }
        public ActionResult StatusUpdate(long Id = 0, int CommentStatus = 0)
        {
            if (Id > 0)
            {
                var item = _nccCommentsService.Get(Id);
                item.CommentStatus = (NccCommentStatus)CommentStatus;
                ViewBag.Message = "Comment Status updated successfull.";
                //if (item.Status == EntityStatus.Active)
                //{
                //    item.Status = EntityStatus.Inactive;
                //    ViewBag.Message = "In-activated successfull.";
                //}
                //else
                //{
                //    item.Status = EntityStatus.Active;
                //    ViewBag.Message = "Activated successfull.";
                //}

                _nccCommentsService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(long Id)
        {
            var item = _nccCommentsService.Get(Id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            var item = _nccCommentsService.Get(Id);
            item.Status = EntityStatus.Deleted;
            _nccCommentsService.Update(item);

            //_nccCommentsService.DeletePermanently(Id);

            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Comments deleted successful";
            return RedirectToAction("Manage");
        }
        #endregion

        #region Public View
        [AllowAnonymous]
        [SiteMenuItem(Name = "Post Comments", Url = "/Comments", Order = 1)]
        public ActionResult Index(int page = 0)
        {
            ViewBag.CurrentLanguage = CurrentLanguage;
            var allItem = _nccCommentsService.LoadApproved(0, page);
            return View(allItem);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult GetComments(long postId, bool isAll = false)
        {
            ApiResponse ret = new ApiResponse();
            ret.IsSuccess = true;
            ret.Message = "Load Successful.";
            
            if (isAll)
                ret.Data = _nccCommentsService.Load(postId, -1);
            else
                ret.Data = _nccCommentsService.Load(postId).ToList();

            
            return Json(ret, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
        
        [AllowAnonymous]        
        [HttpPost]
        public JsonResult PostComments(long postId, string comments, long authorId = 0, string authorName = "", string email = "", string website = "")
        {
            ApiResponse ret = new ApiResponse();
            ret.IsSuccess= true;
            ret.Message = "Save Successful.";
            var model = new NccComment();
            model.Post = _postService.Get(postId);
            model.Content = comments;
            model.AuthorName = authorName;
            model.Email = email;
            model.WebSite = website;
            
            _nccCommentsService.Save(model);
            return Json(ret);
        }
        #endregion

        #region Helper
        private void SetPageViewData(NccComment item)
        {
            ViewBag.AllPosts = new SelectList(_postService.LoadAll().Where(p => p.PostStatus == NccPostStatus.Published), "Id", "Name", item.Post != null ? item.Post.Id : 0);

            var CommentStatus = Enum.GetValues(typeof(NccCommentStatus)).Cast<NccCommentStatus>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            ViewBag.CommentStatus = new SelectList(CommentStatus, "Value", "Text", (int)item.CommentStatus);

        }
        #endregion
    }
}
