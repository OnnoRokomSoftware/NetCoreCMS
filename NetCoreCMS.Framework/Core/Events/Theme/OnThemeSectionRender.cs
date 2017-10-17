using MediatR;
namespace NetCoreCMS.Framework.Core.Events.Theme
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
