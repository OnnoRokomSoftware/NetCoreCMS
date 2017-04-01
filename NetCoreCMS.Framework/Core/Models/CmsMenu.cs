using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Models
{
    public class CmsMenu : BaseModel
    {
        public CmsMenuType CmsMenuType { get; set; }
        public List<Menu> Menus { get; set; }
        public MenuFor MenuFor { get; set; }
    }

    public enum CmsMenuType
    {
        Horizontal,
        Vertical
    }
    public enum MenuFor
    {
        Site,
        Admin,
        Settings
    }
}
