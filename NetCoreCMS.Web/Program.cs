/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using NetCoreCMS.Framework.Core.App;
using System.Diagnostics;

namespace NetCoreCMS.Web
{
    public class Program
    {
        private static IWebHost nccWebHost;
        private static Thread starterThread = new Thread(StartApp);
        private static Stopwatch _stopwatch;
        
        public static void Main(string[] args)
        {
            _stopwatch = new Stopwatch();            
            NetCoreCmsHost.StartForerver(starterThread, new ParameterizedThreadStart(StartApp), Directory.GetCurrentDirectory(), args);
        }

        private static void StartApp(object argsObj)
        {
            BuildWebHost((string[])argsObj).Run();            
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            _stopwatch.Start();
            nccWebHost = WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseDefaultServiceProvider(options => options.ValidateScopes = false)
                .UseApplicationInsights()
                .Build();
            _stopwatch.Stop();
            Console.WriteLine("#\n\r#\n\r######### Elapsed Time: " + _stopwatch.Elapsed + " #############\n\r#\n\r#");
            _stopwatch.Reset();
            return nccWebHost;
        }

        public static async Task RestartAppAsync()
        {
            NetCoreCmsHost.StopAppAsync(nccWebHost);            
        }

        public static async Task ShutdownAppAsync()
        {
            NetCoreCmsHost.ShutdownAppAsync(nccWebHost);
        }
    }
}
