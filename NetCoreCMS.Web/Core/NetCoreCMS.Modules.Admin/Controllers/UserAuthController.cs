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
using NetCoreCMS.Modules.Admin.Models.ViewModels.UserAuthViewModels;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreCMS.Modules.Admin.Controllers
{
    [AdminMenu(Name = "Users", IconCls = "fa fa-users", Order = 6)]    
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

        [AdminMenuItem(Name = "Manage User Roles", Url = "/UserAuth/ManageUserRoles", SubActions = new string[] { "ExtraPermissions", "CreateEditRoles", "UserPermissions" }, Order = 4)]
        public ActionResult ManageUserRoles()
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
                    Id = item.Id,
                    Group = item.Group,
                    Name = item.Name,
                    Description = item.Description,
                    ModuleCount = moduleCount,
                    MenuCount = menuCount,
                    UserCount = userCount
                };

                permissionViewModels.Add(pvm);
            }
            
            return View(permissionViewModels);
        }
        
        public ActionResult CreateEditRoles(long roleId = 0)
        {
            var model = new PermissionViewModel();
            var activeModules = GlobalContext.GetActiveModules();
            ViewBag.Modules = activeModules;

            if (roleId > 0) {
                var permission = _nccPermissionService.Get(roleId);
                if (permission != null)
                {
                    model = GetPermissionViewModel(permission);
                }
                else
                {
                    ViewBag.InfoMessage = "Role not found.";
                }
            }
            return View(model);
        }
        
        [HttpPost]
        public ActionResult CreateEditRoles(PermissionViewModel model)
        {
            var activeModules = GlobalContext.GetActiveModules();
            ViewBag.Modules = activeModules;
            var permission = new NccPermission();
            permission.Description = model.Description;
            permission.Rank = model.Rank;
            permission.Group = model.Group;            
            permission.Name = model.Name;
            permission.Id = model.Id;

            var removePermissionDetailsIdList = new List<long>();

            foreach (var item in model.Modules)
            {
                foreach (var am in item.AdminMenus)
                {
                    foreach (var mi in am.MenuItems)
                    {
                        if (mi.IsChecked)
                        {
                            var permissionDetails = new NccPermissionDetails()
                            {
                                Id = mi.Id,
                                ModuleId = item.ModuleId,
                                Name = am.Name,
                                Action = mi.Action,
                                Controller = mi.Controller
                            };
                            permission.PermissionDetails.Add(permissionDetails);
                        }
                        else
                        {
                            if (mi.Id > 0)
                            {
                                removePermissionDetailsIdList.Add(mi.Id);
                            }
                        }
                    }
                }

                foreach (var sm in item.SiteMenus)
                {
                    foreach (var mi in sm.MenuItems)
                    {
                        if (mi.IsChecked)
                        {
                            var permissionDetails = new NccPermissionDetails()
                            {
                                Id = mi.Id,
                                ModuleId = item.ModuleId,
                                Name = sm.Name,
                                Action = mi.Action,
                                Controller = mi.Controller
                            };
                            permission.PermissionDetails.Add(permissionDetails);
                        }
                        else
                        {
                            if (mi.Id > 0)
                            {
                                removePermissionDetailsIdList.Add(mi.Id);
                            }
                        }
                    }
                }
            }

            var (res,message) = _nccPermissionService.SaveOrUpdate(permission, removePermissionDetailsIdList);

            if (res)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("CreateEditRoles");
            }
            else
            {
                ViewBag.ErrorMessage = message;
            }
            
            return View(model);
        }

        //[AdminMenuItem(Name = "Extra Permissions", Url = "/UserAuth/ExtraPermissions", Order = 5 )]        
        public ActionResult ExtraPermissions(long roleId = 0)
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

        public ActionResult UserPermissions(long roleId = 0)
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

        private PermissionViewModel GetPermissionViewModel(NccPermission permission)
        {
            var pvm = new PermissionViewModel(permission);
            return pvm;
        }

        private void UpdateModelData(PermissionViewModel model, NccPermission permission)
        {
            model.Id = permission.Id;
            model.Rank = permission.Rank;
            model.Description = permission.Description;
            model.Group = permission.Group;
            model.MenuCount = permission.PermissionDetails.GroupBy(x => x.Controller).Count();
            model.ModuleCount = permission.PermissionDetails.GroupBy(x => x.ModuleId).Count();
            model.UserCount = permission.Users.Count;

            foreach (var module in model.Modules)
            {
                foreach (var menu in module.AdminMenus)
                {
                    foreach (var item in menu.MenuItems.Where(x => x.IsChecked).ToList())
                    {
                        var menuItem = permission.PermissionDetails.Where(
                            x => x.ModuleId == module.ModuleId
                            && x.Action == item.Action
                            && x.Controller == item.Controller
                        ).FirstOrDefault();

                        if (menuItem != null)
                        {
                            item.Id = menuItem.Id;
                        }
                    }
                }
            }
        }
    }
}
