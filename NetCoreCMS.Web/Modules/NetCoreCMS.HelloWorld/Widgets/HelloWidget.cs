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
    public class HelloWidget : Widget
    {
        public HelloWidget():base(typeof(HelloHomeController), "Hello", "Hello Widget", "", "Widgets/Hello")
        {
            
        }        
    }
}
