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
using System.Text;

namespace Core.Cms.Models.ViewModels
{
    class NccModuleMenuViewModel
    {
        public string ModuleName { get; set; }
        public string Area { get; set; }
        public string MenuName { get; set; }
        public string MenuItemName { get; set; }        
        public string MenuUrl { get; set; }
    }
}
