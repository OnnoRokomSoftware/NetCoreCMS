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
        
        public List<IModule> RegisterModules(IMvcBuilder mvcBuilder, IServiceCollection services, IServiceProvider serviceProvider)
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
                        RegisterWidgets(moduleInstance, services, serviceProvider);
                        instantiatedModuleList.Add(moduleInstance);
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
                        GlobalConfig.Widgets.Add(widgetInstance);
                    }
                }
            }
            return widgetList;
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

        private void RegisterWidgets(IModule module, IServiceCollection services, IServiceProvider serviceProvider)
        {
            module.Widgets = new List<Widget>();
            var widgetTypeList = module.Assembly.GetTypes().Where( x => typeof(Widget).IsAssignableFrom(x)).ToList();
             
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
