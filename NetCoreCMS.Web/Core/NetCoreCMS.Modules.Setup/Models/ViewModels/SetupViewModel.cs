using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCoreCMS.Modules.Setup.Models.ViewModels
{
    public class SetupViewModel : DatabaseInfo
    {
        [Required]
        public string SiteName { get; set; }
        [Required]
        public string Slogan { get; set; }
        [Required]
        public SetupTemplate SetupTemplate { get; set; }
        [Required]
        public DatabaseEngine Database { get; set; }
        [Required]
        public string AdminUserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string AdminPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
