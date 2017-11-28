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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Cms.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NetCoreCMS.Core.Modules.Cms.Controllers
{
    [AdminMenu(Name = "Appearance", IconCls = "fa-tasks", Order = 40)]
    public class CmsMenuController : NccController
    {
        #region Initialization
        NccMenuService _menuService;
        NccPageService _pageService;
        NccPageDetailsService _pageDetailsService;
        NccPostDetailsService _nccPostDetailsService;
        NccCategoryDetailsService _nccCategoryDetailsService;
        NccTagService _nccTagService;

        public CmsMenuController(
            NccMenuService menuService,
            NccPageService pageService,
            NccPageDetailsService pageDetailsService,
            NccPostDetailsService nccPostDetailsService,
            NccCategoryDetailsService nccCategoryDetailsService,
            NccTagService nccTagService,
            ILoggerFactory factory)
        {
            _pageService = pageService;
            _menuService = menuService;
            _pageDetailsService = pageDetailsService;
            _nccPostDetailsService = nccPostDetailsService;
            _nccCategoryDetailsService = nccCategoryDetailsService;
            _nccTagService = nccTagService;
            _logger = factory.CreateLogger<CmsMenuController>();
        }
        #endregion

        #region Operations
        [AdminMenuItem(Name = "Menu", Url = "/CmsMenu", IconCls = "fa-list", SubActions = new string[] { "CreateEditMenu", "DeleteMenu", "LoadPages", "LoadPosts", "LoadCategories", "LoadModules", "LoadTags" }, Order = 1)]
        public ActionResult Index(bool isManage = false, long menuId = 0)
        {
            AddCmsMenuViewData();

            ViewBag.IsManage = false;
            if (isManage)
                ViewBag.IsManage = true;

            var langList = SupportedCultures.Cultures.Select(x => new { Value = x.TwoLetterISOLanguageName, Text = x.NativeName }).ToList();
            langList.Add(new { Value = "", Text = "All" });
            var menuLanguages = new SelectList(langList, "Value", "Text", "All");

            ViewBag.SelectedLanguage = "All";

            if (menuId > 0)
            {
                NccMenu nccMenu = GlobalContext.Menus.Where(x => x.Id == menuId).FirstOrDefault();
                if (nccMenu != null)
                {
                    ViewBag.SelectedLanguage = nccMenu.MenuLanguage;
                    menuLanguages = new SelectList(langList, "Value", "Text", nccMenu.MenuLanguage);
                    ViewBag.CurrentMenu = nccMenu;

                    string finalMenuList = "";
                    foreach (var menuItem in nccMenu.MenuItems.OrderBy(m => m.MenuOrder))
                    {
                        finalMenuList += menuItemToString(menuItem, 1);
                    }
                    ViewBag.CurrentMenuItems = finalMenuList;
                }
            }

            ViewBag.MenuLanguages = menuLanguages.OrderBy(x => x.Text);
            return View();
        }
        
        [HttpPost]
        public JsonResult CreateEditMenu(string model)
        {
            var query = Request.Query;
            var menu = JsonConvert.DeserializeObject<NccMenuViewModel>(model);

            var r = new ApiResponse();
            if (menu != null)
            {
                if (string.IsNullOrEmpty(menu.MenuLanguage))
                {
                    menu.MenuLanguage = "";
                }

                if (menu.Name.Trim() == "")
                {
                    r.IsSuccess = false;
                    r.Message = "Please enter a menu name.";
                }
                else if (menu.Position.Trim() == "")
                {
                    r.IsSuccess = false;
                    r.Message = "Please select a menu position.";
                }
                else if (menu.Items.Count == 0)
                {
                    r.IsSuccess = false;
                    r.Message = "You cannot save an empty menu.";
                }
                else
                {
                    if (menu.Id > 0)
                    {
                        if (_menuService.LoadAll(false, -1, menu.Name).Count > 0 && _menuService.LoadAll(false, -1, menu.Name).FirstOrDefault().Id != menu.Id)
                        {
                            r.IsSuccess = false;
                            r.Message = "This menu name already used.";
                        }
                        else
                        {
                            NccMenu menuModel = CreateMenuObject(menu);
                            CreateMenuItems(menuModel, menu);
                            _menuService.Update(menuModel);

                            r.IsSuccess = true;
                            r.Message = "Menu updated successfully.";
                        }
                    }
                    else
                    {
                        if (_menuService.LoadAll(false, -1, menu.Name).Count > 0)
                        {
                            r.IsSuccess = false;
                            r.Message = "This menu name already exists.";
                        }
                        else
                        {
                            NccMenu menuModel = CreateMenuObject(menu);
                            CreateMenuItems(menuModel, menu);
                            _menuService.Save(menuModel);

                            r.IsSuccess = true;
                            r.Message = "Menu added successfully.";
                        }
                    }
                }
            }

            GlobalContext.Menus = _menuService.LoadAllSiteMenus();
            ViewBag.MenuList = _menuService.LoadAll();
            //ApiResponse rsp = new ApiResponse();
            //rsp.IsSuccess = false;
            //rsp.Message = "Error occoured. Please fill up all field correctly.";
            //return Json(rsp);
            return Json(r);
        }

        [HttpGet]
        public ActionResult DeleteMenu(long menuId)
        {
            ViewBag.AllPages = _pageService.LoadAllByPageStatus(NccPage.NccPageStatus.Published);
            ViewBag.RecentPages = _pageService.LoadRecentPages(5);
            ViewBag.MenuList = _menuService.LoadAll();

            try
            {
                _menuService.DeletePermanently(menuId);
                TempData["SuccessMessage"] = "Delete successful";
                //return RedirectToAction("Index", new { isManage = true });
            }
            catch (Exception ex)
            {
                //TODO: log error
                TempData["ErrorMessage"] = "Delete Failed";
            }

            GlobalContext.Menus = _menuService.LoadAllSiteMenus();

            return RedirectToAction("Index", new { isManage = true });
        }
        #endregion

        #region Helper Ajax
        [HttpPost]
        public JsonResult LoadPages(string name, string lang)
        {
            var response = new ApiResponse();

            response.IsSuccess = false;
            response.Message = "No data found";
            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower();
                List<NccPageDetails> pageDetails = _pageDetailsService.Search(name, lang);

                if (pageDetails != null)
                {
                    if (pageDetails.Count > 0)
                    {
                        response.Data = pageDetails.Select(x => new { name = x.Name, title = x.Title, slug = x.Slug, id = x.Id });
                        response.IsSuccess = true;
                        response.Message = "successfull";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Empty name";
                    }
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Empty name";
            }

            return Json(response);
        }
        [HttpPost]
        public JsonResult LoadPosts(string name, string lang)
        {
            var response = new ApiResponse();

            response.IsSuccess = false;
            response.Message = "No data found";
            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower();
                List<NccPostDetails> details = _nccPostDetailsService.Search(name, lang);

                if (details != null)
                {
                    if (details.Count > 0)
                    {
                        response.Data = details.Select(x => new { name = x.Name, title = x.Title, slug = x.Slug, id = x.Id });
                        response.IsSuccess = true;
                        response.Message = "successfull";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Empty name";
                    }
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Empty name";
            }

            return Json(response);
        }
        [HttpPost]
        public JsonResult LoadCategories(string name, string lang)
        {
            var response = new ApiResponse();

            response.IsSuccess = false;
            response.Message = "No data found";
            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower();
                List<NccCategoryDetails> details = _nccCategoryDetailsService.Search(name, lang);

                if (details != null)
                {
                    if (details.Count > 0)
                    {
                        response.Data = details.Select(x => new { name = x.Name, title = x.Title, slug = x.Slug, id = x.Id });
                        response.IsSuccess = true;
                        response.Message = "successfull";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Empty name";
                    }
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Empty name";
            }

            return Json(response);
        }
        [HttpPost]
        public JsonResult LoadModules(string name)
        {
            List<NccModuleMenuViewModel> moduleMenuList = new List<NccModuleMenuViewModel>();
            var response = new ApiResponse();

            response.IsSuccess = false;
            response.Message = "No data found";
            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower();
                foreach (KeyValuePair<SiteMenu, List<SiteMenuItem>> item in NccMenuHelper.GetModulesSiteMenus())
                {
                    foreach (SiteMenuItem subItem in item.Value)
                    {
                        moduleMenuList.Add(new NccModuleMenuViewModel() { ModuleName = item.Key.Name, MenuName = subItem.Name, MenuUrl = subItem.Url });
                    }
                }
                var details = moduleMenuList.Where(x => x.ModuleName.ToLower().Contains(name) || x.MenuName.ToLower().Contains(name)).OrderBy(x => x.ModuleName).ThenBy(x => x.MenuName).ToList();
                if (details != null)
                {
                    if (details.Count > 0)
                    {
                        response.Data = details.Select(x => new { moduleName = x.ModuleName, menuName = x.MenuName, menuUrl = x.MenuUrl });
                        response.IsSuccess = true;
                        response.Message = "successfull";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Empty name";
                    }
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Empty name";
            }

            return Json(response);
        }
        [HttpPost]
        public JsonResult LoadTags(string name)
        {
            var response = new ApiResponse();

            response.IsSuccess = false;
            response.Message = "No data found";
            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower();
                List<NccTag> details = _nccTagService.Search(name);

                if (details != null)
                {
                    if (details.Count > 0)
                    {
                        response.Data = details.Select(x => new { name = x.Name, title = "", slug = "", id = x.Id });
                        response.IsSuccess = true;
                        response.Message = "successfull";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Empty name";
                    }
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Empty name";
            }

            return Json(response);
        }
        #endregion

        #region Helper
        private void AddCmsMenuViewData()
        {
            string lang = GlobalContext.WebSite.IsMultiLangual ? "" : GlobalContext.WebSite.Language;
            ViewBag.RecentPages = _pageDetailsService.LoadRecentPageDetails(10, lang);            
            ViewBag.RecentPostDetails = _nccPostDetailsService.LoadRecentPostDetails(10,lang);            
            ViewBag.RecentCategoryDetails = _nccCategoryDetailsService.LoadRecentCategoryDetails(10, lang);            
            ViewBag.RecentTags = _nccTagService.LoadRecentTag(10);

            ViewBag.ModuleSiteMenus = NccMenuHelper.GetModulesSiteMenus();

            ViewBag.MenuList = _menuService.LoadAll();
            ViewBag.MenuLocations = ThemeHelper.ActiveTheme.MenuLocations.ToList();

            ViewBag.CurrentMenu = new NccMenu();
            ViewBag.CurrentMenuItems = "";
        }

        private string menuItemToString(NccMenuItem menuItem, int level)
        {
            var src = Guid.NewGuid().ToString();
            var dest = Guid.NewGuid().ToString();

            string menuStart = @"<li class='list-group-item no-boarder' ncc-menu-item-id='" + menuItem.Id + @"' ncc-menu-item-action-type='Url' ncc-menu-item-controller='" + menuItem.Controller + @"' ncc-menu-item-action='" + menuItem.Action + @"' ncc-menu-item-action-data='' ncc-menu-item-url='" + menuItem.Url + @"' ncc-menu-item-target='" + menuItem.Target + @"' ncc-menu-item-order='" + menuItem.MenuOrder + @"' ncc-menu-item-title='" + menuItem.Name + @"'>
                            <div class='menu-item-content'>
                                <div class='pull-left' style='padding: 5px 5px;'>
                                    <i class='glyphicon glyphicon-move margin-right-10'></i>
                                    <span id='" + src + "' class='ncc-menu-title'>" + menuItem.Name + @"</span>
                                    <input id='" + dest + "' class='ncc-menu-title-editor' type='text' style='display:none' />"
                                    + "&nbsp;&nbsp;<i class='fa fa-edit' onclick = 'ShowEditor(\"" + src + "\", \"" + dest + "\")'></i>"
                                + @"</div>
                                <input type='button' class='closeMenuItem pull-right' value='X' onclick='RemoveMenuItem(this)' />
                            </div>";
            string menuCenter = "";
            if (menuItem.Childrens.Count > 0)
            {
                foreach (var menuItemChild in menuItem.Childrens)
                {
                    menuCenter += "<ul>" + menuItemToString(menuItemChild, level + 1) + "</ul>";
                }
            }
            string menuEnd = @"</li>";
            return menuStart + menuCenter + menuEnd;
        }

        private List<NccMenuItem> CreateMenuItems(NccMenu menuModel, NccMenuViewModel menu)
        {
            foreach (var item in menu.Items)
            {
                NccMenuItem mi = MakeNccMenuItem(item);
                if (mi != null)
                {
                    menuModel.MenuItems.Add(mi);
                }
            }

            return new List<NccMenuItem>();
        }

        private NccMenuItem MakeNccMenuItem(NccMenuItemViewModel miViewModel)
        {
            NccMenuItem parentMenuItem = null;
            if (miViewModel != null)
            {
                parentMenuItem = CreateNccMenuItemObject(miViewModel);
                if (miViewModel.Childrens != null)
                {
                    foreach (NccMenuItemViewModel menuItem in miViewModel.Childrens)
                    {
                        var cMi = MakeNccMenuItem(menuItem);
                        if (cMi != null)
                        {
                            parentMenuItem.Childrens.Add(cMi);
                        }
                    }
                }
            }
            return parentMenuItem;
        }

        private static NccMenuItem CreateNccMenuItemObject(NccMenuItemViewModel item)
        {
            return new NccMenuItem()
            {
                Action = item.Action,
                Controller = item.Controller,
                Data = item.Data,
                //Id = item.Id,
                MenuActionType = TypeConverter.TryParseActionTypeEnum(item.Type),
                MenuOrder = int.Parse(item.Order),
                Module = "",
                Name = item.Title,
                Target = item.Target,
                Url = item.url
            };
        }

        private NccMenu CreateMenuObject(NccMenuViewModel menu)
        {
            return new NccMenu()
            {
                Id = menu.Id,
                Name = menu.Name,
                Position = menu.Position,
                MenuOrder = 1, //TODO:Load last order and incrase and set here
                MenuLanguage = menu.MenuLanguage
            };
        }
        #endregion
    }
}