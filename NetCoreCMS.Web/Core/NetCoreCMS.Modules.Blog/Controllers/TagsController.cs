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
    public class TagsController : NccController
    {
        NccTagService _nccTagService;
        NccPostService _nccPostService;
        NccUserService _nccUserService;
        ILoggerFactory _loggerFactory;

        public TagsController(NccTagService nccTagService,NccPostService nccPostService, NccUserService nccUserService, ILoggerFactory loggerFactory)
        {
            _nccTagService = nccTagService;
            _nccPostService = nccPostService;
            _nccUserService = nccUserService;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<CategoryController>();
        }

        #region Admin Panel
        [AdminMenuItem(Name = "Tags", Url = "/Tags/Manage", IconCls = "fa-tags", SubActions = new string[] { "ManageAjax","StatusUpdate","Delete" }, Order = 7)]
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

                recordsTotal = _nccTagService.Count(false, searchText);
                recordsFiltered = recordsTotal;
                List<NccTag> itemList = _nccTagService.Load(start, length, false, searchText, orderBy, orderDir);
                string controllerName = "Tags";
                foreach (var item in itemList)
                {
                    var str = new List<string>();
                    str.Add(item.Name);

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

                    actionLink += " <a href='/Tag?name=" + item.Name + "' target='_blank' class='btn btn-outline btn-info btn-xs'><i class='fa fa-eye'></i> " + GlobalContext.WebSite.Language + "</a> ";

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

        public ActionResult StatusUpdate(long Id = 0)
        {
            if (Id > 0)
            {
                var item = _nccTagService.Get(Id);
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

                _nccTagService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(long Id)
        {
            NccTag item = _nccTagService.Get(Id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(long Id, int status)
        {
            if (Id > 0)
            {
                var item = _nccTagService.Get(Id);
                item.Status = EntityStatus.Deleted;
                ViewBag.Message = "Deleted successfull.";
                _nccTagService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }
        #endregion

        #region Public View
        [AllowAnonymous]
        [SiteMenuItem(Name = "Post Tags", Url = "/Tags", Order = 1)]
        public ActionResult Index(string name = "", int page = 0)
        {
            ViewBag.CurrentLanguage = CurrentLanguage;
            if (!string.IsNullOrEmpty(name))
            {
                var item = _nccTagService.Get(name);
                if (item != null)
                {
                    ViewBag.Name = name;
                    ViewBag.Tag = item;
                    var postPerPage = GlobalContext.WebSite.WebSitePageSize;
                    var totalPost = _nccPostService.Count(true, true, true, true, null, null, 0, item.Id);
                    var allPost = _nccPostService.Load(page, postPerPage, true, true, true, true, null, null, 0, item.Id);

                    ViewBag.CurrentPage = page;
                    ViewBag.PostPerPage = postPerPage;
                    ViewBag.TotalPost = totalPost;
                    return View("Details", allPost);
                }
            }
            var allPosts = _nccTagService.LoadAll(true).OrderByDescending(x => x.Posts.Count).ToList();
            return View(allPosts);
        }
        #endregion
    }
}