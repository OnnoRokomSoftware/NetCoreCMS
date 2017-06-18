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
        public ActionResult Index()
        {
            ViewBag.AllPages = _pageService.LoadAllByPageStatus(NccPage.NccPageStatus.Published);
            ViewBag.RecentPages = _pageService.LoadRecentPages(5);
            ViewBag.MenuList = _menuService.LoadAll();
            return View();
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
                else
                {
                    NccMenu menuModel = CreateMenuObject(menu);
                    CreateMenuItems(menuModel, menu);
                    _menuService.Save(menuModel);

                    r.IsSuccess = true;
                    r.Message = "Menu Save Successful.";
                }
            }

            GlobalConfig.Menus = _menuService.LoadAllSiteMenus();

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
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                    //TODO: log error
            }

            GlobalConfig.Menus = _menuService.LoadAllSiteMenus();

            TempData["ErrorMessage"] = "Delete Failed";
            return RedirectToAction("Index");
        }

        private List<NccMenuItem> CreateMenuItems(NccMenu menuModel, NccMenuViewModel menu)
        {
            foreach (var item in menu.Items)
            {
                NccMenuItem mi = MakeNccMenuItem(item);
                if(mi != null)
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
                        if(cMi != null)
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
                MenuFor = NccMenuItem.MenuItemFor.Site,
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
                MenuFor = NccMenu.NccMenuFor.Site,
                Name = menu.Name,
                Position = (NccMenu.MenuPosition)Enum.Parse(typeof(NccMenu.MenuPosition),menu.Position,true),
                MenuOrder = 1, //TODO:Load last order and incrase and set here
            };
        }
    }
}
