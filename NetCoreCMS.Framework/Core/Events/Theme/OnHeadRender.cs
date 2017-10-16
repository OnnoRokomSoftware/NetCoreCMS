using MediatR;
namespace NetCoreCMS.Framework.Core.Events.Theme
{
    public class OnHeadRender : IRequest<ThemeSection>
    {
        public ThemeSection Data { get; set; }
        public OnHeadRender(ThemeSection data)
        {
            Data = data;
        }
    }
}
