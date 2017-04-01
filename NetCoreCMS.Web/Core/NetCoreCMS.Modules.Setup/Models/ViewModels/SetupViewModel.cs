using NetCoreCMS.Framework.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCoreCMS.Modules.Setup.Models.ViewModels
{
    public class SetupViewModel
    {
        [Required]
        public string SiteName { get; set; }
        public string DatabaseHost { get; set; }
        public string DatabasePort { get; set; }
        public string DatabaseUserName { get; set; }
        public string DatabasePassword { get; set; }
        public string DatabaseName { get; set; }
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
