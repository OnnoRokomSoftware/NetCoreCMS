/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using MediatR;

namespace NetCoreCMS.Framework.Core.Events.Themes
{
    public class OnThemeActivity : IRequest<ThemeActivity>
    {
        public ThemeActivity Data { get; set; }
        public OnThemeActivity(ThemeActivity data)
        {
            Data = data;
        }
    }
}
