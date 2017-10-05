using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NetCoreCMS.Framework.Modules;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace NetCoreCMS.Framework.Core.ShotCodes
{
    public class NccShortCodeProvider
    {
        private Hashtable _shortCodes;
        private IServiceProvider _services;
        private ILogger _logger;

        public NccShortCodeProvider(IServiceProvider services, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NccShortCodeProvider>();
            _services = services;
            _shortCodes = new Hashtable();
        }
        private (bool IsSuccess, string Message) Register(string code, Type type)
        {
            if (_shortCodes.ContainsKey(code))
            {
                return (false, "Duplicate ShortCode.");
            }
            else
            {
                _shortCodes.Add(code, type);
            }
            return (true, "Registration successful.");
        }

        public Hashtable ScanAndRegisterShortCodes(List<IModule> modules)
        {
            foreach (var module in modules.Where(x=>x.ModuleStatus == (int) NccModule.NccModuleStatus.Active).ToList())
            {
                try
                {
                    var shortCodeTypeList = module.Assembly.GetTypes().Where(x => typeof(IShortCode).IsAssignableFrom(x)).ToList();

                    foreach (var item in shortCodeTypeList)
                    {
                        try
                        {
                            //var sc = (IShortCode)Activator.CreateInstance(item);
                            var sc = (IShortCode)_services.GetService(item);
                            if (string.IsNullOrEmpty(sc.ShortCodeName))
                            {
                                var rsp = Register(sc.ShortCodeName, item);
                                if (rsp.IsSuccess == false)
                                {
                                    _logger.LogError(rsp.Message);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "ShortCode register error for module: " + module.ModuleTitle + ", ShortCode: " + item.Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ShortCode register error for module: " + module.ModuleTitle);
                }
            }
            return _shortCodes;
        }
        
        public static string GetRenderedContent(Type type, object[] paramiters)
        {  
            var mi = type.GetMethod("Render");
            var obj = Activator.CreateInstance(type);
            var ret = mi.Invoke(obj, paramiters);                
            return ret == null ? "": ret.ToString(); 
        }
    }
}
