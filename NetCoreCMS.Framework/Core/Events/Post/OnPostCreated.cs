using MediatR;
using NetCoreCMS.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Events.Post
{
    public class OnPostCreated : IRequest<NccPost>
    {
        public NccPost Post { get; set; }
        public OnPostCreated(NccPost post)
        {
            Post = post;
        }
    }
}
