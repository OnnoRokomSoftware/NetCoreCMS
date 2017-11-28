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
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using NetCoreCMS.Framework.Core.Events.App;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace NetCoreCMS.Framework.Core.App
{
    public class NetCoreCmsHost
    {
        public static bool IsRestartRequired { get; set; }
        public static string AppRootDirectory { get; set; }
        public static IMediator Mediator { get; set; }
        public static ILogger Logger{ get; set; }
        public static HttpContext HttpContext { get; set; }
        public static IServiceCollection Services { get; set; }
        public static IServiceProvider ServiceProvider { get; set; }

        private static bool _isShutdown = false;
        private static int _heartBit = 2000;        
        private static Thread _starterThread;
        
        public static void StartForerver(Thread starterThread, ParameterizedThreadStart webHostStarter, string currentDirectory, string[] args)
        { 
            AppRootDirectory = currentDirectory;            
            _starterThread = starterThread;
            
            try
            {
                if (_starterThread.ThreadState == ThreadState.Unstarted)
                {
                    _starterThread.Start(args);
                    Thread.Sleep(5000);
                    FireEvent(AppActivity.Type.Started);
                }

                while (_isShutdown == false)
                {
                    try
                    {
                        if (_starterThread == null || _starterThread.ThreadState == ThreadState.Stopped)
                        {
                            StartApp(webHostStarter,args);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    Thread.Sleep(_heartBit);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void StartApp(ParameterizedThreadStart webHostStarter, string[] args)
        {
            _starterThread = new Thread(webHostStarter);
            _starterThread.Start(args);
            Thread.Sleep(5000);
            FireEvent(AppActivity.Type.Started);
        }

        private static void FireEvent(AppActivity.Type started)
        {
            try
            {
                Mediator?.SendAll(
                    new OnAppActivity(
                        new AppActivity() {
                            ActivityType = started,
                            Context = HttpContext,
                            Services = Services,
                            ServiceProvider = ServiceProvider
                        })
                    );
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex.Message);
            }
        }

        public static async Task StopAppAsync(IWebHost webHost)
        {
            FireEvent(AppActivity.Type.BeforeRestart);
            new Task(() => {
                Thread.Sleep(1000);
                webHost.StopAsync(new TimeSpan(0, 0, 3));
                _starterThread.Join(3000);
                _starterThread = null;
                GC.Collect();
            }).Start();
        }

        public static async Task ShutdownAppAsync(IWebHost webHost)
        {
            _isShutdown = true;
            StopAppAsync(webHost);
        }
    }
}
