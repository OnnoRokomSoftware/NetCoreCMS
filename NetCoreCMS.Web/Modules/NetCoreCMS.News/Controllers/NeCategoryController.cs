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
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.News.Models;
using NetCoreCMS.Modules.News.Models.Entity;
using NetCoreCMS.Modules.News.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Modules.News.Controllers
{
    [AdminMenu(Name = "News", IconCls = "fa-link", Order = 100)]
    public class NeCategoryController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private NeCategoryService _neCategoryService;
        private NccUserService _nccUserService;

        private NewsSettings nccNewsSettings;

        public NeCategoryController(NccSettingsService nccSettingsService, ILoggerFactory factory, NeCategoryService neCategoryService, NccUserService nccUserService)
        {
            _logger = factory.CreateLogger<NeCategoryController>();
            nccNewsSettings = new NewsSettings();

            _nccSettingsService = nccSettingsService;
            _neCategoryService = neCategoryService;
            _nccUserService = nccUserService;
            nccNewsSettings = _nccSettingsService.GetByKey<NewsSettings>() ?? new NewsSettings();
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Manage Category", Url = "/NeCategory/Manage", IconCls = "", SubActions = new string[] { "ManageAjax", "StatusUpdate", "Delete" }, Order = 4)]
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

                recordsTotal = _neCategoryService.Count(false, searchText);
                recordsFiltered = recordsTotal;
                List<NeCategory> itemList = _neCategoryService.Load(start, length, false, searchText, orderBy, orderDir);
                string controllerName = "NeCategory";
                foreach (var item in itemList)
                {
                    var str = new List<string>();
                    var name = "";
                    if (GlobalContext.WebSite.IsMultiLangual)
                    {
                        foreach (var details in item.Details)
                        {
                            if (!string.IsNullOrEmpty(name))
                            {
                                name += "<br />";
                            }
                            name += "<b>" + details.Language + ":</b> " + details.Name;
                        }
                    }
                    else
                    {
                        name = item.Name;
                    }
                    str.Add(name);
                    str.Add(item.NewsList.Count.ToString());

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
                        str.Add(item.CreationDate.ToString("yyyy-MM-dd HH:mm"));
                    }
                    else
                    {
                        str.Add("<b>Cr:</b> " + item.CreationDate.ToString("yyyy-MM-dd HH:mm") + "<br /><b>Mo:</b> " + item.ModificationDate.ToString("yyyy-MM-dd HH:mm"));
                    }
                    str.Add(item.Status.ToString());

                    string actionLink = " <a href='" + Url.Action("CreateEdit", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-primary btn-outline'>Edit</a> ";
                    if (item.Status == EntityStatus.Active)
                        actionLink += " <a href='" + Url.Action("StatusUpdate", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-danger btn-outline'>Inactive</a> ";
                    else
                        actionLink += " <a href='" + Url.Action("StatusUpdate", controllerName, new { id = item.Id.ToString() }) + "' class='btn btn-xs btn-success btn-outline'>Active</a> ";
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

        [AdminMenuItem(Name = "New Category", Url = "/NeCategory/CreateEdit", IconCls = "fa-plus", Order = 5)]
        public ActionResult CreateEdit(long Id = 0)
        {
            NeCategory item = new NeCategory();

            if (Id > 0)
            {
                item = _neCategoryService.Get(Id);
            }
            else
            {
                NeCategoryDetails _itemDetails = new NeCategoryDetails();
                _itemDetails.Language = GlobalContext.WebSite.Language;
                item.Details.Add(_itemDetails);
            }

            if (GlobalContext.WebSite.IsMultiLangual)
            {
                foreach (var lang in SupportedCultures.Cultures)
                {
                    var count = item.Details.Where(x => x.Language == lang.TwoLetterISOLanguageName).Count();
                    if (count <= 0)
                    {
                        NeCategoryDetails _itemDetails = new NeCategoryDetails();
                        _itemDetails.Language = lang.TwoLetterISOLanguageName;
                        item.Details.Add(_itemDetails);
                    }
                }
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(NeCategory model, string save)
        {
            bool isSuccess = false;
            string returnMessage = "Error occoured. Please fill up all field correctly.";

            if (ModelState.IsValid)
            {
                isSuccess = true;

                #region For default language
                var defaultDetails = model.Details.Where(x => x.Language == GlobalContext.WebSite.Language).FirstOrDefault();
                if (defaultDetails == null)
                {
                    isSuccess = false;
                    returnMessage = "Default language data can't be null";
                }
                else
                {
                    //title empty validation
                    if (string.IsNullOrEmpty(defaultDetails.Name))
                    {
                        isSuccess = false;
                        returnMessage = "Default language Name can't be null";
                    }
                    else
                    {
                        model.Name = defaultDetails.Name;
                    }
                }
                #endregion

                #region Check validation for other languages 
                List<NeCategoryDetails> deletedList = new List<NeCategoryDetails>();
                foreach (var item in model.Details.Where(x => x.Language != GlobalContext.WebSite.Language).ToList())
                {
                    if (item.Id == 0 && string.IsNullOrEmpty(item.Name))
                    {
                        deletedList.Add(item);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(item.Name))
                        {
                            isSuccess = false;
                            returnMessage = "Name can't be null for language " + item.Language;
                        }
                    }
                }

                //Remove empty
                if (isSuccess)
                {
                    foreach (var item in deletedList)
                    {
                        model.Details.Remove(item);
                    }
                }
                #endregion

                #region Operation
                if (isSuccess)
                {
                    //unique name check
                    model.Name = model.Name.Trim();
                    var itemCount = _neCategoryService.LoadAll(true, 0, model.Name).Where(x => x.Id != model.Id).ToList().Count();
                    if (itemCount > 0)
                    {
                        isSuccess = false;
                        returnMessage = "Duplicate name found.";
                    }
                    else
                    {
                        if (model.Id > 0)
                        {
                            _neCategoryService.Update(model);
                            isSuccess = true;
                            returnMessage = "Data updated successfull.";
                        }
                        else
                        {
                            _neCategoryService.Save(model);
                            isSuccess = true;
                            returnMessage = "Data saved successfull.";
                        }
                    }
                }
                #endregion
            }
            else
            {
                returnMessage = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            if (isSuccess)
                ShowMessage(returnMessage, Framework.Core.Mvc.Views.MessageType.Success);
            else
                ShowMessage(returnMessage, Framework.Core.Mvc.Views.MessageType.Error, false, true);

            if (isSuccess == true && save == "Save")
            {
                return RedirectToAction("Manage");
            }
            else if (isSuccess == true)
            {
                return RedirectToAction("CreateEdit");
            }

            return View(model);
        }
        
        public ActionResult StatusUpdate(long Id = 0)
        {
            if (Id > 0)
            {
                var item = _neCategoryService.Get(Id);
                if (item.Status == EntityStatus.Active)
                {
                    item.Status = EntityStatus.Inactive;
                    ShowMessage("In-activated successfull.", Framework.Core.Mvc.Views.MessageType.Success, false, true);
                }
                else
                {
                    item.Status = EntityStatus.Active;
                    ShowMessage("Activated successfull.", Framework.Core.Mvc.Views.MessageType.Success, false, true);
                }

                _neCategoryService.Update(item);
            }
            return RedirectToAction("Manage");
        }
                
        public ActionResult Delete(long Id)
        {
            NeCategory item = _neCategoryService.Get(Id);
            return View(item);
        }

        [HttpPost]        
        public ActionResult Delete(long Id, int status)
        {
            _neCategoryService.DeletePermanently(Id);
            ShowMessage("Item deleted successful", Framework.Core.Mvc.Views.MessageType.Success, false, true);
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        #endregion
    }
}