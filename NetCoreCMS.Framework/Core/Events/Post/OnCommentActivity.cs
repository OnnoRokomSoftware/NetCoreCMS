using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Events.Post
{
    public class OnCommentActivity : IRequest<CommentActivity>
    {
        public CommentActivity Data { get; set; }
        public OnCommentActivity(CommentActivity data)
        {
            Data = Data;
        }
    }
}
