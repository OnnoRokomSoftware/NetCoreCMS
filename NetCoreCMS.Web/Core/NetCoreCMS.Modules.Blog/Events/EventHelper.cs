using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Events.Post;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Modules.Blog.Events
{
    public static class EventHelper
    {
        public static NccPost FirePostEvent(this IMediator _mediator, NccPost post, PostEvent postEvent, NccPost newPost = null, ILogger logger = null)
        {
            try
            {
                var posts = new NccPost[] { };
                if (postEvent == PostEvent.Create)
                {
                    posts = _mediator.SendAll(new OnPostCreated(post)).Result;
                }
                else if (postEvent == PostEvent.Edit)
                {
                    posts = _mediator.SendAll(new OnPostEdited(post, newPost)).Result;
                }
                else if (postEvent == PostEvent.Show)
                {
                    posts = _mediator.SendAll(new OnPostShow(post)).Result;
                }

                return post = posts?.LastOrDefault();
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.Message, ex);
            }
            return null;
        }
    }
}
