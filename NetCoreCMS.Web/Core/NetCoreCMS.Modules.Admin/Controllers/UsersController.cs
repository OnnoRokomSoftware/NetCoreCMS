/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Modules.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Services.Auth;

using NetCoreCMS.Framework.Core.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreCMS.Modules.Admin.Models.ViewModels.UserAuthViewModels;

namespace NetCoreCMS.Modules.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    [AdminMenu(Name = "Users", Order = 16, IconCls = "fa-users")]
    public class UsersController : NccController
    {
        UserManager<NccUser> _userManager;
        RoleManager<NccRole> _roleManager;
        SignInManager<NccUser> _signInManager;
        NccPermissionService _nccPermissionService;
        NccPermissionDetailsService _nccPermissionDetailsService;
        NccUserService _nccUserService;

        //IOptions<IdentityCookieOptions> _identityCookieOptions;
        IEmailSender _emailSender;
        ISmsSender _smsSender;
        NccStartupService _startupService;

        public UsersController(
            ILoggerFactory loggerFactory,
            UserManager<NccUser> userManager,
            RoleManager<NccRole> roleManager,
            SignInManager<NccUser> signInManager,
            NccPermissionService nccUserPermissionService,
            NccPermissionDetailsService  nccPermissionDetailsService,
            IEmailSender emailSender,
            ISmsSender smsSender,
            NccStartupService startupService,
            NccUserService nccUserService
            )
        {
            _logger = loggerFactory.CreateLogger<UsersController>();
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _nccPermissionService = nccUserPermissionService;
            _nccPermissionDetailsService = nccPermissionDetailsService;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _startupService = startupService;
            _nccUserService = nccUserService;
        }

        [AdminMenuItem(Name = "New User", Url = "/Users/CreateEdit", Order = 1, IconCls = "fa-user-plus")]
        public ActionResult CreateEdit(string userName = "")
        {
            var activeModules = GlobalContext.GetActiveModules();
            ViewBag.Modules = activeModules;
            var permissions = _nccPermissionService.LoadAll();
            ViewBag.Roles = new SelectList(permissions, "Id", "Name");

            var user = new UserViewModel();
            if (!string.IsNullOrEmpty(userName))
            {
                NccUser nccUser = _userManager.FindByNameAsync(user.UserName).Result;
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult CreateEditPost(UserViewModel user, string SendEmail)
        {
            if (user.Id > 0 && !string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.FullName) && !string.IsNullOrEmpty(user.Mobile))
            {
                var oldUser = _userManager.FindByIdAsync(user.Id.ToString()).Result;
                oldUser.FullName = user.FullName;
                oldUser.Email = user.Email;
                oldUser.Mobile = user.Mobile;
                var res = _userManager.UpdateAsync(oldUser).Result;
                if (res.Succeeded)
                    TempData["SuccessMessage"] = "User update successful.";
                else
                    TempData["ErrorMessage"] = "User update failed.";
                return RedirectToAction("Index");
            }
            else if (ModelState.IsValid)
            {

                if (user.Password == user.ConfirmPassword)
                {
                    var nccUser = new NccUser() { Email = user.Email, FullName = user.FullName, UserName = user.UserName, Mobile = user.Mobile, Status = EntityStatus.Active };
                    var result = _userManager.CreateAsync(nccUser, user.Password).Result;

                    var createdUser = _userManager.FindByNameAsync(user.UserName).Result;
                    if(createdUser != null)
                    {
                        foreach (var item in user.Roles)
                        {
                            var permission = _nccPermissionService.Get(item);
                            createdUser.Permissions.Add(new NccUserPermission() { Permission = permission, User = createdUser });                        
                        }

                        createdUser.ExtraPermissions = GetSelectedPermissionDetails(user.AllowModules,createdUser, true);                        
                        createdUser.ExtraDenies = GetSelectedPermissionDetails(user.DenyModules, createdUser, false);

                        var upResult = _userManager.UpdateAsync(createdUser).Result;
                        if (upResult.Succeeded == false)
                        {
                            TempData["ErrorMessage"] = "User role assign failed.";
                        }
                    }
                    
                    /*
                    var roleResult = _userManager.AddToRoleAsync(nccUser, user.Role).Result;

                    if (result.Succeeded && roleResult.Succeeded)
                    {
                        TempData["SuccessMessage"] = "User crate successful.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "User create failed.";
                    }
                    */
                }
                else
                {
                    TempData["ErrorMessage"] = "Password does not match.";
                }
            }
            return View("CreateEdit", user);
        }

        private List<NccPermissionDetails> GetSelectedPermissionDetails(List<ModuleViewModel> modules, NccUser user, bool isExtraAllowPermission)
        { 
            var permissionDetailsList = new List<NccPermissionDetails>();

            foreach (var module in modules)
            {
                foreach (var adminMenu in module.AdminMenus)
                {
                    foreach (var item in adminMenu.MenuItems)
                    {
                        if (item.IsChecked)
                        {
                            var pd = new NccPermissionDetails()
                            {
                                Action = item.Action,
                                Controller = item.Controller,
                                ModuleId = module.ModuleId,
                                Name = adminMenu.Name,                                
                                Order = item.Order,                                
                            };
                            if (isExtraAllowPermission)
                            {
                                pd.AllowUser = user;
                            }
                            else
                            {
                                pd.DenyUser = user;
                            }
                            permissionDetailsList.Add(pd);
                        }
                    }
                }

                foreach (var webSiteMenu in module.SiteMenus)
                {
                    foreach (var item in webSiteMenu.MenuItems)
                    {
                        if (item.IsChecked)
                        {

                            var pd = new NccPermissionDetails()
                            {
                                Action = item.Action,
                                Controller = item.Controller,
                                ModuleId = module.ModuleId,
                                Name = webSiteMenu.Name,
                                Order = item.Order 
                            };
                            if (isExtraAllowPermission)
                            {
                                pd.AllowUser = user;
                            }
                            else
                            {
                                pd.DenyUser = user;
                            }
                            permissionDetailsList.Add(pd);
                        }
                    }
                }
            }
            
            return permissionDetailsList;
        }

        [AdminMenuItem(Name = "Manage Users", Url ="/Users/Index", Order = 2, IconCls = "fa-user")]
        public ActionResult Index()
        {
            var users = GetUsersViewModelList("");
            var permissions = _nccPermissionService.LoadAll();
            ViewBag.RoleList = new SelectList(permissions, "Id", "Name");
            return View(users);
        }

        [HttpPost]
        public ActionResult Index(string searchKey)
        {
            if (string.IsNullOrEmpty(searchKey)){
                searchKey = "";
            }
            var users = GetUsersViewModelList(searchKey.Trim());
            ViewBag.SearchKey = searchKey;
            return View(users);
        }

        private List<UserViewModel> GetUsersViewModelList(string searchKey)
        {
            var users = _nccUserService.Search(searchKey);
            var list = new List<UserViewModel>();
            foreach (var user in users)
            {
                list.Add(ToUserViewModel(user));
            }
            return list;
        }

        private UserViewModel ToUserViewModel(NccUser user)
        {
            var uvm = new UserViewModel();
            uvm.Email = user.Email;
            uvm.FullName = user.FullName;
            uvm.Id = user.Id;
            uvm.Mobile = user.Mobile;
            uvm.RoleNames = string.Join(",", user.Permissions.Select(x => x.Permission.Name).ToList());
            uvm.UserName = user.UserName;
            return uvm;
        }
        
        [HttpPost]
        public ActionResult BulkOperation(List<long> userIds, string operation)
        {
            var apiResponses = new List<ApiResponse>();
            switch (operation)
            {
                case "Block":
                    apiResponses = BlockUnBlockUsers(userIds,true);
                    break;
                case "UnBlock":
                    apiResponses = BlockUnBlockUsers(userIds, false);
                    break;
                case "ResetPassword":
                    apiResponses = ResetPasswords(userIds);
                    break;
                default:
                    break;
            }
            return Json(apiResponses);
        }

        private List<ApiResponse> ResetPasswords(List<long> userIds)
        {
            var apiResponses = new List<ApiResponse>();
            
            foreach (var userId in userIds)
            {
                try
                {
                    var user = _userManager.FindByIdAsync(userId.ToString()).Result;
                    var code = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                    var newPassword = Guid.NewGuid().ToString().Substring(0, 6);
                    var result = _userManager.ResetPasswordAsync(user, code, newPassword).Result;

                    if (result.Succeeded)
                    {
                        var webSite = GlobalContext.WebSite.DomainName;
                        if (!webSite.StartsWith("http")) {
                            webSite = "http://" + webSite;
                        }
                        _emailSender.SendEmailAsync(user.Email, "Reset Password", $"Your password reset by Admin. Your new password is: " + newPassword + " <br/> For more info visit: " + webSite).Wait();
                        apiResponses.Add(new ApiResponse()
                        {
                            IsSuccess = true,
                            Message = "Password reset successful for user " + user.UserName + ".",
                        });
                    }
                }
                catch (Exception ex)
                {
                    apiResponses.Add(new ApiResponse()
                    {
                        IsSuccess = false,
                        Message = "Password reset failed for user ID: " + userId + ".",
                    });
                    _logger.LogError(ex.ToString());
                }
            }
            return apiResponses;
        }
         

        private List<ApiResponse> BlockUnBlockUsers(List<long> userIds, bool block)
        {
            var apiResponses = new List<ApiResponse>();
            
            foreach (var userId in userIds)
            {
                try
                {
                    var operation = "block";
                    var user = _userManager.FindByIdAsync(userId.ToString()).Result;

                    if (block == false)
                    {
                        operation = "unblock";
                        var rsp = _userManager.ResetAccessFailedCountAsync(user).Result;
                    }
                    var result = _userManager.SetLockoutEnabledAsync(user, block).Result;
                    
                    if (result.Succeeded)
                    {
                        apiResponses.Add(new ApiResponse()
                        {
                            IsSuccess = true,
                            Message = "Successfully "+operation+" user " + user.UserName + ".",
                        });
                    }
                }
                catch (Exception ex)
                {
                    apiResponses.Add(new ApiResponse()
                    {
                        IsSuccess = false,
                        Message = "Operation failed for user ID: " + userId + ".",
                    });
                    _logger.LogError(ex.ToString());
                }
            }
            return apiResponses;
        }

        [HttpPost]
        public ActionResult ChangeRole(List<long> userIds, long[] roles)
        {
            var apiResponses = new List<ApiResponse>();            
            try
            {
                var messages = _nccUserService.UpdateUsersPermission(userIds, roles);
                foreach (var item in messages)
                {
                    apiResponses.Add(new ApiResponse() { Message = item.Message, IsSuccess = item.IsSuccess});
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                apiResponses.Add(new ApiResponse() { Message = "Error Occoured", IsSuccess = false });
            }

            return Json(apiResponses);
        }
        
        public ActionResult Update(long userId)
        {
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;            
            return View("CreateEdit", ToUserViewModel(user));
        }
 
    }
}
