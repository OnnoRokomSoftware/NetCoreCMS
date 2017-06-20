using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Cms.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NetCoreCMS.Modules.Cms.Controllers
{
    [Authorize]
    public class CmsMenuController : NccController
    {
        NccMenuService _menuService;
        NccPageService _pageService;

        public CmsMenuController(NccMenuService menuService, NccPageService pageService)
        {
            _pageService = pageService;
            _menuService = menuService;
        }
        public ActionResult Index(bool isManage = false, long menuId = 0)
        {
            ViewBag.AllPages = _pageService.LoadAllByPageStatus(NccPage.NccPageStatus.Published);
            ViewBag.RecentPages = _pageService.LoadRecentPages(5);
            ViewBag.MenuList = _menuService.LoadAll();
            ViewBag.IsManage = false;
            if (isManage)
                ViewBag.IsManage = true;
            ViewBag.CurrentMenu = new NccMenu();
            ViewBag.CurrentMenuItems = "";
            if (menuId > 0)
            {
                //NccMenu nccMenu = _menuService.Get(menuId);
                NccMenu nccMenu = GlobalConfig.Menus.Where(x => x.Id == menuId).FirstOrDefault();
                if (nccMenu != null)
                {
                    ViewBag.CurrentMenu = nccMenu;
                    string finalMenuList = "";
                    foreach (var menuItem in nccMenu.MenuItems)
                    {
                        finalMenuList += menuItemToString(menuItem, 1);
                    }
                    ViewBag.CurrentMenuItems = finalMenuList;
                }
            }
            return View();
        }

        private string menuItemToString(NccMenuItem menuItem, int level)
        {
            string menuStart = @"<li class='list-group-item no-boarder' ncc-menu-item-id='" + menuItem.Id + @"' ncc-menu-item-action-type='Url' ncc-menu-item-controller='" + menuItem.Controller + @"' ncc-menu-item-action='" + menuItem.Action + @"' ncc-menu-item-action-data='' ncc-menu-item-url='" + menuItem.Url + @"' ncc-menu-item-target='" + menuItem.Target + @"' ncc-menu-item-order='" + menuItem.MenuOrder + @"' ncc-menu-item-title='" + menuItem.Name + @"'>
                            <div class='menu-item-content'>
                                <div class='pull-left' style='padding: 5px 5px;'>
                                    <i class='glyphicon glyphicon-move margin-right-10'></i>
                                    <span class='ncc-menu-title'>" + menuItem.Name + @"</span>
                                </div>
                                <input type='button' class='closeMenuItem pull-right' value='X' onclick='RemoveMenuItem(this)' />
                            </div>";
            string menuCenter = "";
            if (menuItem.Childrens.Count > 0)
            {
                foreach (var menuItemChild in menuItem.Childrens)
                {
                    menuCenter = "<ul>" + menuItemToString(menuItemChild, level + 1) + "</ul>";
                }
            }
            string menuEnd = @"</li>";
            return menuStart + menuCenter + menuEnd;
        }

        [HttpPost]
        public JsonResult CreateMenu(string model)
        {
            var query = Request.Query;
            var menu = JsonConvert.DeserializeObject<NccMenuViewModel>(model);

            var r = new ApiResponse();
            if (menu != null)
            {
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
                        if (_menuService.LoadAllByName(menu.Name).Count > 0 && _menuService.LoadAllByName(menu.Name).FirstOrDefault().Id != menu.Id)
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
                        if (_menuService.LoadAllByName(menu.Name).Count > 0)
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

            GlobalConfig.Menus = _menuService.LoadAllSiteMenus();
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

            GlobalConfig.Menus = _menuService.LoadAllSiteMenus();

            return RedirectToAction("Index", new { isManage = true });
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
                Id = item.Id,
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
            };
        }
    }
}
