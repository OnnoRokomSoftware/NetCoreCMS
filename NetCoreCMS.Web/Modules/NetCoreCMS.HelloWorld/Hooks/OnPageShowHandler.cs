/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using MediatR;
using NetCoreCMS.Framework.Core.Events.Page;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.HelloWorld.Hooks
{
    public class OnPageShowHandler : IRequestHandler<OnPageShow, NccPage>
    {
        public NccPage Handle(OnPageShow message)
        {
            var post = message.Page;
            if(post.PageDetails.Count >= 2)
            {
                post.PageDetails[0].Content += " --- Modified from Hellow World Module by hooking.";
                post.PageDetails[1].Content += " --- এই কটেন্ট Hallo World মডিউল থেকে পরিবর্তন করা হয়েছে।";
            }
            else if(post.PageDetails.Count >= 1)
            {
                post.PageDetails[0].Content += " --- Modified from Hellow World Module by hooking.";
            }

            return post;
        }
    }
}
