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

            return View();
        }

        [HttpPost]
        public JsonResult CreateMenu(string model)
        {
            var query = Request.Query;
            var menu = JsonConvert.DeserializeObject<NccMenuViewModel>(model);

            if(menu != null)
            {
                NccMenu menuModel = CreateMenuObject(menu);
                menuModel.MenuItems = CreateMenuItems(menuModel, menu);
                _menuService.Save(menuModel);

                var r = new ApiResponse();
                r.IsSuccess = true;
                r.Message = "Menu Save Successful.";
                return Json(r);
            }

            ApiResponse rsp = new ApiResponse();
            rsp.IsSuccess = false;
            rsp.Message = "Error occoured. Please fill up all field correctly.";
            return Json(rsp);
        }

        private List<NccMenuItem> CreateMenuItems(NccMenu menuModel, NccMenuViewModel menu)
        {
            foreach (var item in menu.Items)
            {
                List<NccMenuItem> items = ConstructMenuItems(item);
                if(items != null && items.Count > 0)
                {
                    menuModel.MenuItems.AddRange(items);
                }
            }
            
            return new List<NccMenuItem>();
        }

        private List<NccMenuItem> ConstructMenuItems(NccMenuItemViewModel item)
        {
            //TODO: Write code
            throw new NotImplementedException();
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
