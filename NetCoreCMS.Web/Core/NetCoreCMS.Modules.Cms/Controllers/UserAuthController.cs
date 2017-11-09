using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Cms.Models.ViewModels.UserAuthViewModels;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreCMS.AdvancedPermission.Controllers
{
    [AdminMenu(Name = "Advanced Security", IconCls = "fa fa-users", Order = 100)]
    [Authorize(Roles ="SuperAdmin,Administrator")]
    public class UserAuthController : NccController
    {
        private readonly UserManager<NccUser> _userManager;
        private readonly RoleManager<NccRole> _roleManager;
        private readonly NccPermissionService _nccPermissionService;
        private readonly NccPermissionDetailsService _nccPermissionDetailsService;        
        
        public UserAuthController(
            UserManager<NccUser> userManager, 
            RoleManager<NccRole> roleManager, 
            NccPermissionService nccPermissionService,
            NccPermissionDetailsService nccPermissionDetailsService,            
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _nccPermissionService = nccPermissionService;
            _nccPermissionDetailsService = nccPermissionDetailsService;            
            _logger = loggerFactory.CreateLogger<UserAuthController>();
        }

        [AdminMenuItem(Name = "Permission Templates", Url = "/UserAuth/PermissionTemplates", Order = 1)]
        public ActionResult PermissionTemplates()
        {
            var permissions = _nccPermissionService.LoadAll();
            var permissionViewModels = new List<PermissionViewModel>();

            foreach (var item in permissions)
            {
                var moduleCount = item.PermissionDetails.GroupBy(x => x.ModuleId).Count();
                var menuCount = item.PermissionDetails.GroupBy(x => x.Action).Count();
                var userCount = item.Users.Count;

                var pvm = new PermissionViewModel()
                {
                    Group = item.Group,
                    Name = item.Name,
                    ModuleCount = $"Modules ({moduleCount})",
                    MenuCount = $"Menus ({menuCount})",
                    UserCount = $"Users ({userCount})"
                };

                permissionViewModels.Add(pvm);
            }

            return View(permissionViewModels);
        }

        public ActionResult CreateEditPermission(long permissionId = 0)
        {           
            var activeModules = GlobalContext.GetActiveModules();
            ViewBag.Modules = activeModules;

            return View(new PermissionViewModel());
        }

        [HttpPost]
        public ActionResult CreateEditPermission(PermissionViewModel model)
        {
            var activeModules = GlobalContext.GetActiveModules();
            ViewBag.Modules = activeModules;

            return View(model);
        }

        [AdminMenuItem(Name = "User Permissions", Url = "/UserAuth/Index", Order = 1 )]
        public ActionResult UserPermission(long roleId = 0)
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

            ViewBag.Roles = new SelectList(roles,"Name","Name");
            ViewBag.Users = new SelectList(roleUsers, "Id", "Name");
            ViewBag.Modules = activeModules;

            return View();
        }

        public ActionResult ChangeUserPermission(long roleId = 0)
        {
            var roles = _roleManager.Roles.ToList();
            var roleUsers = new List<NccUser>();

            if (roleId > 0)
            {
                roleUsers = _userManager.Users.Include("Roles").Where(x => x.Roles.Where(y => y.Role.Id == roleId).ToList().Count > 0).ToList();
            }
            else
            {
                roleUsers = _userManager.Users.ToList();
            }

            var activeModules = GlobalContext.GetActiveModules();

            ViewBag.Roles = new SelectList(roles, "Name", "Name");
            ViewBag.Users = new SelectList(roleUsers, "Id", "Name");
            ViewBag.Modules = activeModules;

            return View();
        }
    }
}
