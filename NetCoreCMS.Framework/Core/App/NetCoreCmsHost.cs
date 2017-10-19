/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace NetCoreCMS.Framework.Core.App
{
    public class NetCoreCmsHost
    {
        public static bool IsRestartRequired { get; set; }
        public static string AppRootDirectory { get; set; }

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
        }

        public static async Task StopAppAsync(IWebHost webHost)
        {
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
