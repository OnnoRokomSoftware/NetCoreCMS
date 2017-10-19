/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using NetCoreCMS.Framework.Themes;

namespace NetCoreCMS.Framework.Core.Events.Themes
{
    public class ThemeActivity
    {
        public Theme Theme { get; set; }
        public Type ActivityType { get; set; }

        public enum Type
        {
            Download,
            Install,
            Activated,
            Deactivated,
            Uninstall            
        }
    }
}
