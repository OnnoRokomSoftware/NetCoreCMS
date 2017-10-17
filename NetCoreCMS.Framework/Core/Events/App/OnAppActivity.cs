using MediatR;

namespace NetCoreCMS.Framework.Core.Events.App
{
    public class OnAppActivity : IRequest<AppActivity>
    {
        public AppActivity Data { get; set; }
        public OnAppActivity(AppActivity data)
        {
            Data = data;
        }
    }
}
