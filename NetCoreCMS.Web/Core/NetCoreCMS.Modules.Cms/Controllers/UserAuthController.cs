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
        private readonly NccUserAuthorizationService _nccUserAuthorizationService;
        
        public UserAuthController(UserManager<NccUser> userManager, RoleManager<NccRole> roleManager, NccUserAuthorizationService nccUserAuthorizationService, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _nccUserAuthorizationService = nccUserAuthorizationService;
            _logger = loggerFactory.CreateLogger<UserAuthController>();
        }

        [AdminMenuItem(Name = "Permissions", Url = "/UserAuth/Index", Order = 1 )]
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

            ViewBag.Roles = new SelectList(roles,"Name","Name");
            ViewBag.Users = new SelectList(roleUsers, "Id", "Name");
            ViewBag.Modules = activeModules;

            return View();
        }

        [AdminMenuItem(Name = "Templates", Url = "/UserAuth/Templates", Order = 2)]
        public ActionResult Templates()
        {
            return View();
        }
    }
}
