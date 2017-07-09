using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System;
using System.Linq;
using static NetCoreCMS.Framework.Core.Models.NccPage;

namespace NetCoreCMS.Modules.Cms.Controllers
{
    [Authorize(Roles ="SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "Page", IconCls = "fa-file-text-o", Order = 3)]
    public class CmsPageController : NccController
    {
        #region Initialization
        NccPageService _pageService;
        
        public CmsPageController(NccPageService pageService, ILoggerFactory factory)
        {
            _pageService = pageService;
            _logger = factory.CreateLogger<CmsPageController>();
        }
        #endregion

        #region Public View
        [AllowAnonymous]
        public ActionResult Index(string slug)
        {
            if (!string.IsNullOrEmpty(slug))
            {
                var page = _pageService.GetBySlugs(slug);
                if (page != null)
                {
                    return View(page);
                }
            }
            TempData["Message"] = "Page not found";
            return Redirect("/CmsHome/ResourceNotFound");
        }
        #endregion


        [AdminMenuItem(Name = "New page", Url = "/CmsPage/CreateEdit", IconCls = "fa-pencil-square-o", Order = 1)]
        public ActionResult CreateEdit(long Id = 0)
        {
            ViewBag.DomainName = (Request.IsHttps == true ? "https://" : "http://") + Request.Host + "/CmsPage/";

            NccPage page = new NccPage();
            page.Content = "";
            page.PublishDate = DateTime.Now;
            page.PageStatus = NccPage.NccPageStatus.Draft;

            if (Id > 0)
            {
                page = _pageService.Get(Id);
            }

            ViewBag.Layouts = new SelectList(GlobalConfig.ActiveTheme.Layouts, "Name", "Name", page.Layout);
            ViewBag.AllPages = new SelectList(_pageService.LoadAll().Where(p => p.PageStatus == NccPageStatus.Published && p.Id != Id), "Id", "Title", page.Parent != null ? page.Parent.Id : 0);

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

            return View(page);
        }


        [HttpPost]
        public ActionResult CreateEdit(NccPage model, string PageContent, long ParentId, string SubmitType)
        {
            ViewBag.MessageType = "ErrorMessage";
            ViewBag.Message = "Error occoured. Please fill up all field correctly.";

            model.Content = PageContent;
            if (ModelState.IsValid)
            {
                if (model.Title.Trim() == "")
                {
                    ViewBag.Message = "Please enter page title.";
                }
                else if (model.Slug.Trim() == "")
                {
                    ViewBag.Message = "Please do not delete slug. Slug is required.";
                }
                else
                {
                    var slugPage = _pageService.GetBySlugs(model.Slug);
                    if (model.Id > 0)
                    {
                        try
                        {
                            var parrent = _pageService.Get(ParentId);
                            model.Parent = parrent;
                        }
                        catch (Exception) { }
                        if (slugPage != null && slugPage.Id != model.Id)
                        {
                            ViewBag.Message = "This slug is already used in another page.";
                        }
                        else
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
                    }
                    else
                    {
                        if (slugPage != null)
                        {
                            ViewBag.Message = "Duplicate slug is found.";
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
                }
            }
            if (SubmitType.ToLower() == "publish")
            {
                return RedirectToAction("Manage");
            }
            ViewBag.Layouts = new SelectList(GlobalConfig.ActiveTheme.Layouts, "Name", "Name", model.Layout);
            ViewBag.AllPages = new SelectList(_pageService.LoadAll().Where(p => p.PageStatus == NccPageStatus.Published && p.Id != model.Id), "Id", "Title", model.Parent != null ? model.Parent.Id : 0);

            var PageStatus = Enum.GetValues(typeof(NccPageStatus)).Cast<NccPageStatus>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            ViewBag.PageStatus = new SelectList(PageStatus, "Value", "Text", (int)model.PageStatus);

            var PageType = Enum.GetValues(typeof(NccPageType)).Cast<NccPageType>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
            ViewBag.PageType = new SelectList(PageType, "Value", "Text", (int)model.PageType);
            return View(model);
        }        

        [AdminMenuItem(Name = "Manage", Url = "/CmsPage/Manage", IconCls = "fa-th-list", Order = 3)]
        public ActionResult Manage()
        {
            var allPages = _pageService.LoadAll().OrderByDescending(p => p.Id).ToList();
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
            ApiResponse rsp = new ApiResponse();
            _pageService.DeletePermanently(Id);
            //rsp.IsSuccess = true;
            //rsp.Message = "Page deleted successful";
            //rsp.Data = "";
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Page deleted successful";
            return RedirectToAction("Manage");
        }
    }
}
