using MediatR;

namespace NetCoreCMS.Framework.Core.Events.App
{
    public class OnUserActivity : IRequest<UserActivity>
    {
        public UserActivity Data { get; set; }
        public OnUserActivity(UserActivity data)
        {
            Data = data;
        }
    }
}
