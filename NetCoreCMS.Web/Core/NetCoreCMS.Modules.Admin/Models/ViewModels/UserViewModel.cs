/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreCMS.Framework.Core.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCoreCMS.Modules.Admin.Models.ViewModels
{
    public class UserViewModel
    {
        public long Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required(ErrorMessage = "The Email field is required")]
        [EmailAddress]
        public string Email { get; set; }

        public string Mobile { get; set; }

        public bool SendEmailNotification { get; set; }

        [Required]
        public string Role { get; set; }

        public static SelectList GetRolesDropdown()
        {
            var list = new Dictionary<string, string>();
            list.Add(NccCmsRoles.Administrator, NccCmsRoles.Administrator);
            list.Add(NccCmsRoles.Author, NccCmsRoles.Author);
            list.Add(NccCmsRoles.Contributor, NccCmsRoles.Contributor);
            list.Add(NccCmsRoles.Editor, NccCmsRoles.Editor);
            list.Add(NccCmsRoles.Subscriber, NccCmsRoles.Subscriber);
            list.Add(NccCmsRoles.SuperAdmin, NccCmsRoles.SuperAdmin);

            return  new SelectList(list, "Value","Key");
        }
    }
}
