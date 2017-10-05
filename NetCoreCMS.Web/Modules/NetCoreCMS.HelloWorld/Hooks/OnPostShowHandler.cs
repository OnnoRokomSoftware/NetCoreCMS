using MediatR;
using NetCoreCMS.Framework.Core.Events.Post;
using NetCoreCMS.Framework.Core.Models;
 

namespace NetCoreCMS.HelloWorld.Hooks
{
    public class OnPostShowHandler : IRequestHandler<OnPostShow, NccPost>
    {
        public NccPost Handle(OnPostShow message)
        {
            var post = message.Post;
            if(post.PostDetails.Count >= 2)
            {
                post.PostDetails[0].Content += " --- Modified from Hellow World Module by hooking.";
                post.PostDetails[1].Content += " --- এই কটেন্ট Hallo World মডিউল থেকে পরিবর্তন করা হয়েছে।";
            }
            else if(post.PostDetails.Count >= 1)
            {
                post.PostDetails[0].Content += " --- Modified from Hellow World Module by hooking.";
            }

            return post;
        }
    }
}
