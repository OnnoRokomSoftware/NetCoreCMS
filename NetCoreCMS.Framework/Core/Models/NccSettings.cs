/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Mvc.Models;
using System.Reflection;
using System;

namespace NetCoreCMS.Framework.Core.Models
{
    [Serializable]
    public class NccSettings : BaseModel<long>
    {   
        public NccSettings()
        {
            var assembly = Assembly.GetCallingAssembly();
            GroupId = assembly.FullName;
        }
        public string GroupId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; } 
    }
}
