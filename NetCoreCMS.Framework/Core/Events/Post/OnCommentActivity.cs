/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
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
