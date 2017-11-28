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
using NetCoreCMS.Framework.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NetCoreCMS.Core.Modules.Blog.Controllers
{
    [AdminMenu(IconCls = "fa-newspaper-o", Name = "Blog", Order = 1)]
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
        [AdminMenuItem(Name = "Manage Comments", Url = "/Comments/Manage", IconCls = "fa-comments", SubActions = new string[] { "ManageAjax","CreateEdit", "StatusUpdate", "Delete" }, Order = 9)]
        public ActionResult Manage()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ManageAjax(int draw, int start, int length)
        {
            var CommentStatus = Enum.GetValues(typeof(NccComment.NccCommentStatus)).Cast<NccComment.NccCommentStatus>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

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

                recordsTotal = _nccCommentsService.Count(false, 0, searchText);
                recordsFiltered = recordsTotal;
                List<NccComment> itemList = _nccCommentsService.Load(start, length, false, 0, searchText, orderBy, orderDir);
                string controllerName = "Comments";
                foreach (var item in itemList)
                {
                    var str = new List<string>();
                    str.Add(item.Post.Name);
                    str.Add(item.Content);
                    str.Add(item.AuthorName);

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

                    str.Add(item.CommentStatus.ToString());

                    string actionLink = "";
                    foreach (var commentsItem in CommentStatus)
                    {
                        if (item.CommentStatus.ToString() != commentsItem.Text)
                        {
                            actionLink += " <a href='" + Url.Action("StatusUpdate", controllerName, new { id = item.Id.ToString(), commentStatus = commentsItem.Value }) + "' class='btn btn-xs btn-info btn-outline'>" + commentsItem.Text + "</a> ";
                        }
                    }

                    actionLink += " <a href='" + Url.Action("Delete", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-danger'>Delete</a> ";
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


        public ActionResult StatusUpdate(long Id = 0, int CommentStatus = 0)
        {
            if (Id > 0)
            {
                var item = _nccCommentsService.Get(Id);
                item.CommentStatus = (NccComment.NccCommentStatus)CommentStatus;
                _nccCommentsService.Update(item);
                ShowMessage("Comment Status updated successfull.", Framework.Core.Mvc.Views.MessageType.Success, false, true);
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
            
            ShowMessage("Comment deleted successfully.", Framework.Core.Mvc.Views.MessageType.Success, false, true);
            return RedirectToAction("Manage");
        }
        #endregion

        #region Public View
        [AllowAnonymous]
        [SiteMenuItem(Name = "Post Comments", Url = "/Comments", SubActions = new string[] { "StatusUpdate" }, Order = 1)]
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
            ViewBag.AllPosts = new SelectList(_postService.LoadAll().Where(p => p.PostStatus == NccPost.NccPostStatus.Published), "Id", "Name", item.Post != null ? item.Post.Id : 0);

            var CommentStatus = Enum.GetValues(typeof(NccComment.NccCommentStatus)).Cast<NccComment.NccCommentStatus>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            ViewBag.CommentStatus = new SelectList(CommentStatus, "Value", "Text", (int)item.CommentStatus);
        }
        #endregion
    }
}
