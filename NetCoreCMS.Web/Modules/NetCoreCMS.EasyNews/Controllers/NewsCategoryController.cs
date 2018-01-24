/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Cache;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.EasyNews.Models.Entities;
using NetCoreCMS.EasyNews.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreCMS.EasyNews.Controllers
{
    [AdminMenu(Name = "News", IconCls = "fa-link", Order = 100)]
    public class NewsCategoryController : NccController
    {
        #region Initialization
        private CategoryService _categoryService;
        public NewsCategoryController(ILoggerFactory factory, CategoryService categoryService)
        {
            _logger = factory.CreateLogger<NewsCategoryController>();
            _categoryService = categoryService;
        }
        #endregion

        #region Admin Panel
        [AdminMenuItem(Name = "Manage Category", IconCls = "", SubActions = new string[] { "ManageAjax", "StatusUpdate", "Delete" }, Order = 4)]
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

                recordsTotal = _categoryService.Count(false, searchText);
                recordsFiltered = recordsTotal;
                List<Category> itemList = _categoryService.Load(start, length, false, searchText, orderBy, orderDir);
                string controllerName = "NewsCategory";
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
                        str.Add(Cache.GetNccUser(item.CreateBy)?.UserName);
                    }
                    else
                    {
                        str.Add("<b>Cr:</b> " + UserService.Get(item.CreateBy)?.UserName + "<br /><b>Mo:</b> " + UserService.Get(item.ModifyBy)?.UserName);
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

        [AdminMenuItem(Name = "New Category", IconCls = "fa-plus", Order = 5)]
        public ActionResult CreateEdit(long Id = 0)
        {
            Category item = new Category();

            if (Id > 0)
            {
                item = _categoryService.Get(Id);
            }
            else
            {
                CategoryDetails _itemDetails = new CategoryDetails
                {
                    Language = GlobalContext.WebSite.Language
                };
                item.Details.Add(_itemDetails);
            }

            if (GlobalContext.WebSite.IsMultiLangual)
            {
                foreach (var lang in SupportedCultures.Cultures)
                {
                    var count = item.Details.Where(x => x.Language == lang.TwoLetterISOLanguageName).Count();
                    if (count <= 0)
                    {
                        CategoryDetails _itemDetails = new CategoryDetails
                        {
                            Language = lang.TwoLetterISOLanguageName
                        };
                        item.Details.Add(_itemDetails);
                    }
                }
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult CreateEdit(Category model, string save)
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
                List<CategoryDetails> deletedList = new List<CategoryDetails>();
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
                    var itemCount = _categoryService.LoadAll(true, 0, model.Name).Where(x => x.Id != model.Id).ToList().Count();
                    if (itemCount > 0)
                    {
                        isSuccess = false;
                        returnMessage = "Duplicate name found.";
                    }
                    else
                    {
                        if (model.Id > 0)
                        {
                            _categoryService.Update(model);
                            isSuccess = true;
                            returnMessage = "Data updated successfull.";
                        }
                        else
                        {
                            _categoryService.Save(model);
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
                var item = _categoryService.Get(Id);
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

                _categoryService.Update(item);
            }
            return RedirectToAction("Manage");
        }
                
        public ActionResult Delete(long Id)
        {
            Category item = _categoryService.Get(Id);
            return View(item);
        }

        [HttpPost]        
        public ActionResult Delete(long Id, int status)
        {
            _categoryService.DeletePermanently(Id);
            ShowMessage("Item deleted successful", Framework.Core.Mvc.Views.MessageType.Success, false, true);
            return RedirectToAction("Manage");
        }
        #endregion

        #region User Panel
        #endregion
    }
}