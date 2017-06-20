/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using NetCoreCMS.Framework.Utility;
using Newtonsoft.Json;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Services;

namespace NetCoreCMS.Framework.Modules
{
    public class ModuleManager
    {
        List<IModule> modules = new List<IModule>();
        List<IModule> instantiatedModuleList = new List<IModule>();
        public List<IModule> LoadModules(IDirectoryContents moduleRootFolder)
        {   
            foreach (var moduleFolder in moduleRootFolder.Where(x => x.IsDirectory))
            {
                try
                {
                    var binFolder = new DirectoryInfo(Path.Combine(moduleFolder.PhysicalPath, "bin"));
                    if (!binFolder.Exists)
                    {
                        continue;
                    }

                    foreach (var file in binFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
                    {
                        Assembly assembly;
                        try
                        {
                            assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                        }
                        catch (FileLoadException ex )
                        {
                            continue;
                        }
                        catch(BadImageFormatException ex)
                        {
                            continue;
                        }

                        if (assembly.FullName.Contains(moduleFolder.Name))
                        {
                            modules.Add(new Module{ ModuleName = moduleFolder.Name, Assembly = assembly, Path = moduleFolder.PhysicalPath });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not load module from " + moduleFolder);
                }
            }
            return modules;
        }
        
        public List<IModule> RegisterModules(IMvcBuilder mvcBuilder, IServiceCollection services, IServiceProvider serviceProvider)
        {  

            foreach (var module in modules)
            {
                try
                {
                    var moduleInitializerType = module.Assembly.GetTypes().Where(x => typeof(IModule).IsAssignableFrom(x)).FirstOrDefault();

                    if (moduleInitializerType != null && moduleInitializerType != typeof(IModule))
                    {
                        var initilizedModule = (IModule)Activator.CreateInstance(moduleInitializerType);
                        LoadModuleInfo(initilizedModule, module);

                        NccModule.NccModuleStatus moduleStatus = VerifyModuleInstallation(initilizedModule, serviceProvider);

                        if (moduleStatus == NccModule.NccModuleStatus.Active)
                        {
                            // Register controller from modules
                            mvcBuilder.AddApplicationPart(module.Assembly);
                            // Register dependency in modules                            
                            initilizedModule.Init(services);
                            RegisterWidgets(initilizedModule, services, serviceProvider);
                            instantiatedModuleList.Add(initilizedModule); 
                        }
                        else if (moduleStatus == NccModule.NccModuleStatus.Duplicate)
                        {
                            //TODO: Raise duplicate error message
                        }
                    }
                }
                catch (Exception ex)
                {
                    //RAISE GLOBAL ERROR
                }
            }
            
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ModuleViewLocationExpendar());
            });

            mvcBuilder.AddRazorOptions(o =>
            {
                foreach (var module in modules)
                {
                    o.AdditionalCompilationReferences.Add(MetadataReference.CreateFromFile(module.Assembly.Location));
                }
            });

            return instantiatedModuleList;
        }

        private bool IsCoreModule(IModule module)
        {
            var pathParts = module.Path.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var hasCore = pathParts.Where(x => x.Equals("Core", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            return !string.IsNullOrEmpty(hasCore);
        }

        private NccModule.NccModuleStatus VerifyModuleInstallation(IModule module, IServiceProvider serviceProvider)
        {
            var moduleService = serviceProvider.GetService<NccModuleService>();
            NccModule moduleEntity = moduleService.GetByModuleId(module.ModuleId);
            if(moduleEntity == null)
            {
                moduleEntity = CreateNccModuleEntity(module);
                moduleService.Save(moduleEntity);
            }
            else if(moduleEntity.Name != module.ModuleName)
            {
                return NccModule.NccModuleStatus.Duplicate;
            }
            
            return moduleEntity.ModuleStatus;
        }

        public List<IWidget> RegisterModuleWidgets(IMvcBuilder mvcBuilder, IServiceCollection services, IServiceProvider serviceProvider)
        {
            var widgetList = new List<IWidget>();
            foreach (var module in instantiatedModuleList)
            {
                module.Widgets = new List<IWidget>();
                var widgetTypeList = module.Assembly.GetTypes().Where(x => x.GetInterfaces()?.Where(y => y.Name == typeof(IWidget).Name).FirstOrDefault() != null).ToList();

                foreach (var widgetType in widgetTypeList)
                {
                    //var widgetInstance = (IWidget)Activator.CreateInstance(widgetType);                    
                    var widgetInstance = (IWidget)serviceProvider.GetService(widgetType);
                    widgetInstance.Init();
                    module.Widgets.Add(widgetInstance);
                    widgetList.Add(widgetInstance);
                }
            }
            return widgetList;
        }

        private NccModule CreateNccModuleEntity(IModule module)
        {
            var nccModule = new NccModule();
            nccModule.Name = module.ModuleName;
            nccModule.AntiForgery = module.AntiForgery;
            nccModule.ModuleId = module.ModuleId;
            nccModule.Dependencies = String.Join(",", module.Dependencies);
            nccModule.NetCoreCMSVersion = module.NetCoreCMSVersion;
            nccModule.Path = module.Path;            
            nccModule.Version = module.Version;
            nccModule.Description = module.Description;
            nccModule.Category = module.Category;
            nccModule.Author = module.Author;
            nccModule.WebSite = module.Website;
            nccModule.ModuleTitle = module.ModuleTitle;

            if(module.Category.Contains("Core"))
            {
                nccModule.ModuleStatus = NccModule.NccModuleStatus.Active;
            }
            else
            {
                nccModule.ModuleStatus = NccModule.NccModuleStatus.New;
            }

            return nccModule;
        }

        private void RegisterWidgets(IModule module, IServiceCollection services, IServiceProvider serviceProvider)
        {
            module.Widgets = new List<IWidget>();
            var widgetTypeList = module.Assembly.GetTypes().Where(x => x.GetInterfaces()?.Where(y => y.Name == typeof(IWidget).Name).FirstOrDefault() != null).ToList();
             
            foreach (var widgetType in widgetTypeList)
            {                
                services.AddTransient(widgetType);                
            }             
        }

        public void LoadModuleInfo(IModule module, IModule moduleInfo)
        {
            var moduleConfigFile = Path.Combine(moduleInfo.Path, Constants.ModuleConfigFileName);
            if (File.Exists(moduleConfigFile))
            {                
                var moduleInfoFileJson = File.ReadAllText(moduleConfigFile);
                var loadedModule = JsonConvert.DeserializeObject<Module>(moduleInfoFileJson);
                module.ModuleId = loadedModule.ModuleId;
                module.AntiForgery = loadedModule.AntiForgery;
                module.Author = loadedModule.Author;
                module.Category = loadedModule.Category;
                module.Dependencies = loadedModule.Dependencies;
                module.Description = loadedModule.Description;
                module.ModuleId = loadedModule.ModuleId;
                
                module.ModuleTitle = loadedModule.ModuleTitle;
                module.NetCoreCMSVersion = loadedModule.NetCoreCMSVersion;
                module.ModuleName = loadedModule.ModuleName;
                module.Version = loadedModule.Version;
                module.Website = loadedModule.Website;
                module.Assembly = moduleInfo.Assembly;
                module.Path = moduleInfo.Path;
            }
            else
            {
                //RAISE GLOBAL ERROR
            }
        }

    }
}
