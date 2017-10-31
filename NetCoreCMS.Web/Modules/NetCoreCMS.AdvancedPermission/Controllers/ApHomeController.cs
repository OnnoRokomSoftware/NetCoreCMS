using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreCMS.AdvancedPermission.Controllers
{
    [AdminMenu(Name = "Advanced Security", IconCls = "fa fa-users", Order = 201)]
    [Authorize(Roles ="SuperAdmin,Administrator")]
    public class ApHomeController : NccController
    {
        private readonly UserManager<NccUser> _userManager;
        private readonly RoleManager<NccRole> _roleManager;        
        private readonly NccUserPermissionService _nccUserPermissionService;
        
        public ApHomeController(UserManager<NccUser> userManager, RoleManager<NccRole> roleManager, NccUserPermissionService nccUserPermissionService, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;            
            _nccUserPermissionService = nccUserPermissionService;
            _logger = loggerFactory.CreateLogger<ApHomeController>();
        }

        [AdminMenuItem(Name = "Permissions", Url = "/ApHome/Index", Order = 1 )]
        public ActionResult Index(long roleId = 0)
        {
            var roles = _roleManager.Roles.ToList();
            var roleUsers = new List<NccUser>();
            
            if(roleId > 0)
            {
                roleUsers = _userManager.Users.Include("Roles").Where(x => x.Roles.Where(y => y.Role.Id == roleId).ToList().Count > 0).ToList();
            }
            else
            {
                roleUsers = _userManager.Users.ToList();
            }

            var activeModules = GlobalContext.GetActiveModules();

            ViewBag.Roles = roles;
            ViewBag.Users = roleUsers;
            ViewBag.Modules = activeModules;

            return View();
        }

        [AdminMenuItem(Name = "Templates", Url = "/ApHome/Templates", Order = 1)]
        public ActionResult Templates()
        {
            return View();
        }
    }
}
