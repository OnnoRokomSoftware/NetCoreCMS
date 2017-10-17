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
