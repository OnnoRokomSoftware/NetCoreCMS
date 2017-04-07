using NetCoreCMS.Framework.Setup;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.Setup.Models.ViewModels
{
    public class AdminViewModel
    {
        [Required]
        public string SiteName { get; set; }
        [Required]
        public string Slogan { get; set; }
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

    }
}
