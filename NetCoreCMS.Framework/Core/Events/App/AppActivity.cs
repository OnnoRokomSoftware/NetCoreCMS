/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NetCoreCMS.Framework.Core.Events.App
{
    /// <summary>
    /// Event argument class for Web Application evetns. Our application will fire following types of events while running the application.
    /// </summary>
    public class AppActivity
    {
        public HttpContext Context { get; set; }  
        
        public IServiceCollection Services { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        public Type ActivityType { get; set; }
        public long ResponseTime { get; set; }

        public enum Type
        {
            Started,
            SessionStart,
            SessionEnd,
            RequestStart,
            RequestEnd,
            BeforeRestart
        }
    }
}
