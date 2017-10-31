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
using NetCoreCMS.Framework.Core.ShotCodes;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.IoC;
using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreCMS.Framework.Core.Mvc.FIlters;
using Microsoft.Extensions.Logging;

namespace NetCoreCMS.Framework.Modules
{
    public class ModuleManager
    {
        List<IModule> modules;
        List<IModule> instantiatedModuleList;

        public ModuleManager()
        {
            modules = new List<IModule>();
            instantiatedModuleList = new List<IModule>();
        }

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
                            modules.Add(new Module{ Folder = moduleFolder.Name, Assembly = assembly, Path = moduleFolder.PhysicalPath });
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
        
        public List<IModule> AddModulesAsApplicationPart(IMvcBuilder mvcBuilder, IServiceCollection services, IServiceProvider serviceProvider)
        {  

            foreach (var module in modules)
            {
                try
                {
                    var moduleInitializerType = module.Assembly.GetTypes().Where(x => typeof(IModule).IsAssignableFrom(x)).FirstOrDefault();

                    if (moduleInitializerType != null && moduleInitializerType != typeof(IModule))
                    {
                        var moduleInstance = (IModule)Activator.CreateInstance(moduleInitializerType);
                        LoadModuleInfo(moduleInstance, module);

                        NccModule.NccModuleStatus moduleStatus = VerifyModuleInstallation(moduleInstance, serviceProvider);
                        moduleInstance.ModuleStatus = (int)moduleStatus;

                        if (moduleStatus == NccModule.NccModuleStatus.Active)
                        {                            
                            // Register controller from modules
                            mvcBuilder.AddApplicationPart(module.Assembly);                            
                        }
                        else if (moduleStatus == NccModule.NccModuleStatus.Duplicate)
                        {
                            //TODO: Raise duplicate error message
                            continue;
                        }                        
                        
                        // Register dependency in modules                            
                        moduleInstance.Init(services);                        
                        instantiatedModuleList.Add(moduleInstance);
                        GlobalContext.Modules.Add(moduleInstance);
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

        public List<IModule> GetModules()
        {
            return modules;
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
            else if(moduleEntity.ModuleId != module.ModuleId)
            {
                return NccModule.NccModuleStatus.Duplicate;
            }
            
            return moduleEntity.ModuleStatus;
        }

        public List<Widget> RegisterModuleWidgets(IMvcBuilder mvcBuilder, IServiceCollection services, IServiceProvider serviceProvider)
        {
            var widgetList = new List<Widget>();
            foreach (var module in instantiatedModuleList)
            {
                if (module.ModuleStatus == (int)NccModule.NccModuleStatus.Active)
                {
                    module.Widgets = new List<Widget>();
                    var widgetTypeList = module.Assembly.GetTypes().Where(x => typeof(Widget).IsAssignableFrom(x)).ToList();

                    foreach (var widgetType in widgetTypeList)
                    {
                        //var widgetInstance = (IWidget)Activator.CreateInstance(widgetType);                    
                        var widgetInstance = (Widget)serviceProvider.GetService(widgetType);                        
                        module.Widgets.Add(widgetInstance);
                        widgetList.Add(widgetInstance);
                        GlobalContext.Widgets.Add(widgetInstance);
                    }
                }
            }
            return widgetList;
        }

        public void AddModuleServices(IServiceCollection services)
        {
            foreach (var module in instantiatedModuleList)
            {
                if (module.ModuleStatus == (int)NccModule.NccModuleStatus.Active)
                {
                    var repositoryTypes = module.Assembly.GetTypes().Where( x => x.BaseType.IsGenericType).Where( y =>  y.BaseType.GetGenericTypeDefinition() == typeof(BaseRepository<,>)).ToList();
                    foreach (var item in repositoryTypes)
                    {
                        var singleton = item.GetInterfaces().Where(x => typeof(ISingleton).IsAssignableFrom(x)).FirstOrDefault();
                        if (singleton != null)
                        {
                            services.AddSingleton(item);
                            continue;
                        }

                        var scoped = item.GetInterfaces().Where(x => typeof(IScoped).IsAssignableFrom(x)).FirstOrDefault();
                        if (scoped != null)
                        {
                            services.AddScoped(item);
                            continue;
                        }

                        services.AddTransient(item);
                    }
                    
                    var serviceTypes = module.Assembly.GetTypes().Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IBaseService<>))).ToList();
                    foreach (var item in serviceTypes)
                    {
                        var singleton = item.GetInterfaces().Where(x => typeof(ISingleton).IsAssignableFrom(x)).FirstOrDefault();
                        if (singleton != null)
                        {
                            services.AddSingleton(item);
                            continue;
                        }

                        var scoped = item.GetInterfaces().Where(x => typeof(IScoped).IsAssignableFrom(x)).FirstOrDefault();
                        if (scoped != null)
                        {
                            services.AddScoped(item);
                            continue;
                        }

                        services.AddTransient(item);
                    }
                }
            }            
        }

        public void RegisterModuleShortCodes(IMvcBuilder mvcBuilder, IServiceProvider serviceProvider)
        {
            var _nccShortCodeProvider = serviceProvider.GetService<NccShortCodeProvider>();
            GlobalContext.ShortCodes = _nccShortCodeProvider.RegisterShortCodes(GlobalContext.Modules);
        }

        public void AddShortcodes(IServiceCollection services)
        {
            var activeModules = instantiatedModuleList.Where(x => x.ModuleStatus == (int)NccModule.NccModuleStatus.Active).ToList();
            foreach (var module in activeModules)
            {
                var shortCodeTypeList = module.Assembly.GetTypes().Where(x => typeof(IShortCode).IsAssignableFrom(x)).ToList();
                foreach (var item in shortCodeTypeList)
                {
                    services.AddTransient(item);
                }
            }
        }

        public void AddModuleFilters(IServiceCollection services)
        {
            var activeModules = instantiatedModuleList.Where(x => x.ModuleStatus == (int)NccModule.NccModuleStatus.Active).ToList();
            foreach (var module in activeModules)
            {
                var actionFilters = module.Assembly.GetTypes().Where(x => typeof(INccActionFilter).IsAssignableFrom(x)).ToList();
                foreach (var item in actionFilters)
                {
                    services.AddScoped(item);
                }
            }            
        }

        private NccModule CreateNccModuleEntity(IModule module)
        {
            var nccModule = new NccModule();
            nccModule.Name = module.Folder;
            nccModule.AntiForgery = module.AntiForgery;
            nccModule.ModuleId = module.ModuleId;
            nccModule.Dependencies = module.Dependencies;
            nccModule.MinNccVersion = module.MinNccVersion;
            nccModule.MaxNccVersion = module.MaxNccVersion;
            nccModule.Path = module.Path;
            nccModule.Folder = module.Folder;
            nccModule.Version = module.Version;
            nccModule.Description = module.Description;
            nccModule.Category = module.Category;
            nccModule.Author = module.Author;
            nccModule.WebSite = module.Website;
            nccModule.ModuleTitle = module.ModuleTitle;

            var coreModuleDir = Directory.GetParent(nccModule.Path);

            if (coreModuleDir.Name.Equals("Core"))
            {
                nccModule.ModuleStatus = NccModule.NccModuleStatus.Active;
            }
            else
            {
                nccModule.ModuleStatus = NccModule.NccModuleStatus.New;
            }

            return nccModule;
        }

        public void AddModuleWidgets( IServiceCollection services)
        {
            foreach (var module in instantiatedModuleList)
            {
                module.Widgets = new List<Widget>();
                var widgetTypeList = module.Assembly.GetTypes().Where(x => typeof(Widget).IsAssignableFrom(x)).ToList();
                foreach (var widgetType in widgetTypeList)
                {
                    services.AddTransient(widgetType);
                }
            }            
        }

        public void RegisterModuleFilters(IMvcBuilder mvcBuilder, IServiceProvider serviceProvider)
        {
            mvcBuilder.AddMvcOptions(option => {                
                option.Filters.Add(serviceProvider.GetService<NccLanguageFilter>());
                option.Filters.Add(serviceProvider.GetService<NccGlobalExceptionFilter>());
            });

            var actionFilterList = new List<INccActionFilter>();

            var activeModules = instantiatedModuleList.Where(x => x.ModuleStatus == (int)NccModule.NccModuleStatus.Active).ToList();
            foreach (var item in activeModules)
            {
                var actionFilters = item.Assembly.GetTypes().Where(x => typeof(INccActionFilter).IsAssignableFrom(x)).ToList();
                foreach (var filter in actionFilters)
                {
                    var filterInstance = (INccActionFilter) serviceProvider.GetService(filter);
                    actionFilterList.Add(filterInstance);
                }
            }

            actionFilterList.OrderBy(x=>x.Order);

            foreach (var item in actionFilterList)
            {
                mvcBuilder.AddMvcOptions(options => {
                    if (options.Filters.Contains(item) == false)
                    {
                        options.Filters.Add(item);
                    }
                });
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
                module.MinNccVersion = loadedModule.MinNccVersion;
                module.MaxNccVersion = loadedModule.MaxNccVersion;
                module.SortName = loadedModule.SortName;
                module.Version = loadedModule.Version;
                module.Website = loadedModule.Website;
                module.Assembly = moduleInfo.Assembly;
                module.Path = moduleInfo.Path;
                module.ModuleStatus = moduleInfo.ModuleStatus;
            }
            else
            {
                //RAISE GLOBAL ERROR
            }
        }

    }
}
