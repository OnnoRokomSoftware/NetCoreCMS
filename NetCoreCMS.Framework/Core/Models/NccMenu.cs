/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccMenu : BaseModel, IBaseModel<long>
    { 
        public NccMenu()
        {
            MenuItems = new List<NccMenuItem>();
        }

        public string Position { get; set; }
        public string MenuIconCls { get; set; }
        public int MenuOrder { get; set; }
        public string MenuLanguage { get; set; }

        public List<NccMenuItem> MenuItems { get; set; }        
        
    }   
}
