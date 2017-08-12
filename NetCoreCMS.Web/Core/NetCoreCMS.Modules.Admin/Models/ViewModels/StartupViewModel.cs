using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Admin.Models.ViewModels
{
    public class StartupViewModel
    {
        public string Default { get; set; }
        public string StartupType { get; set; }
        public string RoleStartupType { get; set; }
        public string StartupFor { get; set; }
        public string PageSlug { get; set; }
        public SelectList Pages { get; set; }
        public string CategorySlug { get; set; }
        public SelectList Categories { get; set; }
        public string PostSlug { get; set; }
        public SelectList Posts { get; set; }
        public string ModuleSiteMenuUrl { get; set; }
        public SelectList ModuleSiteMenus { get; set; }
        public SelectList Roles { get; set; }
    }
}
