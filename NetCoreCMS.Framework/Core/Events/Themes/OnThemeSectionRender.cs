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
    public class OnThemeSectionRender : IRequest<ThemeSection>
    {
        public ThemeSection Data { get; set; }
        public OnThemeSectionRender(ThemeSection data)
        {
            Data = data;
        }
    }
}
