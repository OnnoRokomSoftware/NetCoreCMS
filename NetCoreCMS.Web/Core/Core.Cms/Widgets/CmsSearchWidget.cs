/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Core.Cms.Controllers;
using NetCoreCMS.Framework.Modules.Widgets;

namespace Core.Cms.Widgets
{
    public class CmsSearchWidget : Widget
    { 
        public CmsSearchWidget() : base(typeof(CmsHomeController), "Search Widget", "Search form show", "", "Widgets/CmsSearch")
        {
            
        }
    }
}
