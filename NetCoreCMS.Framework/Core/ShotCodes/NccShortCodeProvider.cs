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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NetCoreCMS.Framework.Modules;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Utility;
using System.Net;

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

        public Hashtable RegisterShortCodes(List<IModule> modules)
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
                            if (string.IsNullOrEmpty(sc.ShortCodeName) == false)
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
        
        private string GetRenderedContent(Type type, object[] paramiters)
        {  
            var mi = type.GetMethod("Render");
            var obj = (IShortCode)_services.GetService(type);
            var ret = obj.Render(paramiters);
            return ret == null ? "": ret.ToString(); 
        }

        public string ReplaceShortContent(string content)
        {
            foreach (DictionaryEntry item in GlobalContext.ShortCodes)
            {
                var key = item.Key.ToString();
                if (content.Contains(key))
                {
                    var shortCode = GetShortCode(content, key);
                    if (shortCode != null)
                    {
                        var contentPrefix = content.Substring(0, shortCode.Start);
                        var contentSuffix = content.Substring(shortCode.End + shortCode.Name.Length + 1);
                        var renderedContent = GetRenderedContent((Type)item.Value, shortCode.Paramiters.ToArray());
                        content = contentPrefix + renderedContent + contentSuffix;
                    }
                }
            }

            return content;
        }

        private ShortCode GetShortCode(string content, string code)
        {
            ShortCode shortCode;
            var start = content.IndexOf("[" + code);
            var end = content.IndexOf(code + "]");

            if (start > 0 && end > 0)
            {
                shortCode = new ShortCode();

                shortCode.Start = start;
                shortCode.End = end;
                shortCode.Name = code;

                start = start + code.Length + 1;
                end = end - 1;

                var contentLength = end - start;
                if(contentLength > 0)
                {
                    var shortCodeContent = content.Substring(start, contentLength);
                    var paramsParts = shortCodeContent.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in paramsParts)
                    {
                        var dItem = WebUtility.HtmlDecode(item);
                        var keyValPart = dItem.Split("=".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (keyValPart.Length >= 2)
                        {
                            var p = keyValPart[1].Replace('"', ' ').Trim();
                            shortCode.Paramiters.Add(p);
                        }
                    }
                }  
                return shortCode;
            }

            return null;
        }
    }
}
