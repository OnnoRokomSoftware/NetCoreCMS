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
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Services;
using System;
using System.Linq;

namespace NetCoreCMS.Core.Modules.Blog.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]    
    [AdminMenu(IconCls = "fa-newspaper-o", Name = "Blog", Order = 3)]
    public class CommentsController : NccController
    {
        #region Initialization
        NccCommentsService _nccCommentsService;
        
        NccUserService _nccUserService;
        ILoggerFactory _loggerFactory;

        public CommentsController(NccCommentsService nccCommentsService,NccUserService nccUserService, ILoggerFactory loggerFactory)
        {
            _nccCommentsService = nccCommentsService;
            _loggerFactory = loggerFactory;
            _nccUserService = nccUserService;
            _logger = _loggerFactory.CreateLogger<BlogController>();
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "New Comments", Url = "/Comments/CreateEdit", IconCls = "fa-pencil-square-o", Order = 3)]
        public ActionResult CreateEdit(long Id = 0)
        {
            NccComment item = new NccComment();            
            item.CommentStatus = NccComment.StatusEnum.Pending;

            if (Id > 0)
            {
                item = _nccCommentsService.Get(Id);
            }

            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(NccComment model, long PostId, long AuthorId, string SubmitType)
        {
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

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
                            _nccCommentsService.Save(model);
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

            return View(model);
        }

        [AdminMenuItem(Name = "Manage Comments", Url = "/Comments/Manage", IconCls = "fa-th-list", Order = 1)]
        public ActionResult Manage()
        {
            var allPages = _nccCommentsService.LoadAll(false).OrderByDescending(p => p.CreationDate).ToList();
            return View(allPages);
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
        
        #endregion

        #region Helper
        #endregion
    }
}
