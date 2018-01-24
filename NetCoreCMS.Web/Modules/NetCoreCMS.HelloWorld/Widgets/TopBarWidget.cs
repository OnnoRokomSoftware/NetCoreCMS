/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.HelloWorld.Controllers;

namespace NetCoreCMS.HelloWorld.Widgets
{
    public class TopBarWidget : Widget
    {
        public TopBarWidget():base(typeof(HelloHomeController), "TopBar", "TopBar Widget", "", "Widgets/TopBar")
        {
            
        }        
    }
}
