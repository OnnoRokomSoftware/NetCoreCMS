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

namespace NetCoreCMS.Framework.Setup
{
    public class SetupTemplateModules
    {
        public List<string> Basic { get; } = new List<string>()
        {
            "NetCoreCMS.Modules.Cms"
        };

        public List<string> Blog { get; } = new List<string>()
        {
            "NetCoreCMS.Modules.Cms"
        };

        public List<string> CompanyWebiste { get; } = new List<string>()
        {
            "NetCoreCMS.Modules.Cms"
        };

        public List<string> PersonalProfile { get; } = new List<string>()
        {
            "NetCoreCMS.Modules.Cms"
        };

        public List<string> CRM { get; } = new List<string>()
        {
            "NetCoreCMS.Modules.Cms",
            "NetCoreCMS.Modules.CRM",
        };

        public List<string> ShoppingCart { get; } = new List<string>()
        {
            "NetCoreCMS.Modules.Cms",
            "NetCoreCMS.Modules.ShoppingCart",
        };

        public List<string> Accounting { get; } = new List<string>()
        {
            "NetCoreCMS.Modules.Cms",
            "NetCoreCMS.Modules.Accounting",
        };
        
        public List<string> Inventory { get; } = new List<string>()
        {
            "NetCoreCMS.Modules.Cms",
            "NetCoreCMS.Modules.Inventory",
        };

        public List<string> SchoolManagement { get; } = new List<string>()
        {
            "NetCoreCMS.Modules.Cms",
            "NetCoreCMS.Modules.SchoolManagement",
        };
    }
}
