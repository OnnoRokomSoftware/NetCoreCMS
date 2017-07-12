using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Modules.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NetCoreCMS.Framework.Core.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Modules.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    [AdminMenu(Name = "Users", Order = 20, IconCls = "fa-users")]
    public class UsersController : NccController
    {
        UserManager<NccUser> _userManager;
        RoleManager<NccRole> _roleManager;
        SignInManager<NccUser> _signInManager;
        IOptions<IdentityCookieOptions> _identityCookieOptions;
        IEmailSender _emailSender;
        ISmsSender _smsSender;

        public UsersController(
            ILoggerFactory loggerFactory,
            UserManager<NccUser> userManager,
            RoleManager<NccRole> roleManager,
            SignInManager<NccUser> signInManager,
            IOptions<IdentityCookieOptions> identityCookieOptions,
            IEmailSender emailSender,
            ISmsSender smsSender
            )
        {
            _logger = loggerFactory.CreateLogger<UsersController>();
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _identityCookieOptions = identityCookieOptions;
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        [AdminMenuItem(Name = "Manage Users", Url ="/Users/Index", Order = 2, IconCls = "fa-user")]
        public ActionResult Index()
        {
            var users = GetUsersViewModelList();
            
            return View(users);
        }

        private List<UserViewModel> GetUsersViewModelList()
        {
            var users = _userManager.Users.ToList();
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
            uvm.Role = string.Join(",", user.Roles.Select(x => x.Role.Name).ToList());
            uvm.UserName = user.UserName;
            return uvm;
        }

        [AdminMenuItem(Name = "New User", Url = "/Users/CreateEdit", Order = 1, IconCls = "fa-user-plus")]
        public ActionResult CreateEdit(string userName = "")
        {
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
            if (ModelState.IsValid)
            {
                if(user.Password == user.ConfirmPassword)
                {
                    var nccUser = new NccUser() { Email = user.Email, FullName = user.FullName, UserName = user.UserName, Mobile = user.Mobile, Status = EntityStatus.Active };
                    var result =  _userManager.CreateAsync(nccUser,user.Password).Result;
                    var roleResult = _userManager.AddToRoleAsync(nccUser, user.Role).Result;
                    
                    if (result.Succeeded && roleResult.Succeeded)
                    {
                        TempData["SuccessMessage"] = "User crate successful.";
                        RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "User create failed.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Password does not match.";
                }
            }
            return View("CreateEdit",user);
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
                        var webSite = GlobalConfig.WebSite.DomainName;
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
        public ActionResult ChangeRole(List<long> userIds, string role)
        {
            var apiResponses = new List<ApiResponse>();

            foreach (var userId in userIds)
            {
                try
                {
                    var user = _userManager.FindByIdAsync(userId.ToString()).Result;
                    var existingRoles = _userManager.GetRolesAsync(user).Result;
                    var rRemoveResult = _userManager.RemoveFromRolesAsync(user, existingRoles).Result;

                    if (rRemoveResult.Succeeded)
                    {
                        var result = _userManager.AddToRoleAsync(user, role).Result;
                        if (result.Succeeded == false)
                        {
                            foreach (var err in result.Errors)
                            {
                                apiResponses.Add(new ApiResponse() { Message = err.Description, IsSuccess = false });
                            }
                        }
                        else
                        {
                            apiResponses.Add(new ApiResponse() { Message = "Role '" + role + "' has been successfully assigned into user "+user.UserName+".", IsSuccess = true });
                        }
                    }
                    else
                    {
                        apiResponses.Add(new ApiResponse() { Message = "Removing previous role failed for user " + user.UserName, IsSuccess = false });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    apiResponses.Add(new ApiResponse() { Message = "Unknown Error Occoured for userId " + userId, IsSuccess = false });
                }
            }
            
            return Json(apiResponses);
        }
        
        public ActionResult Update(long userId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update(UserViewModel user)
        {
            return View();
        }
    }
}
