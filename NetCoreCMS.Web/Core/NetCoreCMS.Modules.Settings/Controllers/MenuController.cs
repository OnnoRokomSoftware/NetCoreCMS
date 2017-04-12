/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;

namespace NetCoreCMS.Modules.Settings.Controllers
{
    public class MenuController : NccController
    {
        NccMenuService _menuService;
        public MenuController(NccMenuService menuService)
        {
            _menuService = menuService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateMenuItem()
        {
            return View();
        }
    }
}
