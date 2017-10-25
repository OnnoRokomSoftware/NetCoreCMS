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
using NetCoreCMS.Framework.Core.Events.Themes;

namespace NetCoreCMS.Framework.Core.Hooks
{
    public class OnSectionRenderHandler : IRequestHandler<OnThemeSectionRender, ThemeSection>
    {    
        public ThemeSection Handle(OnThemeSectionRender message)
        { 
            return message.Data;
        }     
    }
}
