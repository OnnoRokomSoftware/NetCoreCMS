using MediatR;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Framework.Core.Events.Post
{
    public class OnPostShow : IRequest<NccPost>
    {
        public NccPost Post { get; set; }
        public OnPostShow(NccPost post)
        {
            Post = post;
        }
    }
}
