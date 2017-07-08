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
        public ActionResult ResetPassword(UserViewModel user)
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateRole(List<UserViewModel> user, string role)
        {

            return View();
        }

    }
}
