/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using DefaultTheme.Controllers;
using NetCoreCMS.Framework.Modules.Widgets;

namespace DefaultTheme.Widgets
{
    public class DefaultThemeWidget : Widget
    { 
        public DefaultThemeWidget():base(typeof(DefaultThemeController), "Default Theme", "Default theme Hello Widget", "", "Widgets/Hello")
        {
            
        }
    }
}
