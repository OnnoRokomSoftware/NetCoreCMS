using MediatR;
using NetCoreCMS.Framework.Core.Events.Theme;

namespace NetCoreCMS.Framework.Core.Hooks.Default
{
    class OnHeadRenderHandler : IRequestHandler<OnHeadRender, ThemeSection>
    {    
        public ThemeSection Handle(OnHeadRender message)
        {
            return message.Data;
        }     
    }
}
