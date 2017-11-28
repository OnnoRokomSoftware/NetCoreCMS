/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Setup;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Core.Modules.Setup.Models.ViewModels
{
    public class AdminViewModel
    {
        [Required]
        public string SiteName { get; set; }
        [Required]
        public string Tagline { get; set; }
        [Required]
        public SetupTemplate SetupTemplate { get; set; }
        [Required]
        public string AdminUserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string AdminPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Language { get; set; }
        public string TablePrefix { get; set; }

    }
}
