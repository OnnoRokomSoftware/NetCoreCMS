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
