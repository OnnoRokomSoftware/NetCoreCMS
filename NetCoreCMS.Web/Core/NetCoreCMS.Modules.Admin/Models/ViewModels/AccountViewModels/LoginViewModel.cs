/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreCMS.Core.Modules.Admin.Models.AccountViewModels
{
    public class LoginViewModel
    {
        
        [Required(ErrorMessage = "The Email field is required")]
        [Display(Name = "Email")]
        //[EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password field is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]        
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
