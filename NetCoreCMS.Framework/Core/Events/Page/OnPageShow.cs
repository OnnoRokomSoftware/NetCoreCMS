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
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Framework.Core.Events.Page
{
    public class OnPageShow : IRequest<NccPage>
    {
        public NccPage Page { get; set; }
        public OnPageShow(NccPage page)
        {
            Page = page;
        }
    }
}
