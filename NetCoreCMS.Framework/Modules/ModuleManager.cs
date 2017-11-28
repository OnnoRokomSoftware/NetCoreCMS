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
using System.Threading.Tasks;
using NetCoreCMS.Framework.Core.Models.ViewModels;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Auth.Handlers;
using Microsoft.AspNetCore.Authorization;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Core.Messages;
using Microsoft.Extensions.DependencyModel;
using NetCoreCMS.Framework.Modules.Loader;
using System.Collections;

namespace NetCoreCMS.Framework.Modules
{
    public class ModuleManager
    {
        List<IModule> modules;
        List<IModule> instantiatedModuleList;
        Hashtable _moduleDependencies = new Hashtable();

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
                            assembly = NccAssemblyLoader.LoadFromFileName(file.FullName);
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
                            if(modules.Count(x=>x.Folder == moduleFolder.Name && x.Path == moduleFolder.PhysicalPath) == 0)
                            {
                                modules.Add(new Module { Folder = moduleFolder.Name, Assembly = assembly, Path = moduleFolder.PhysicalPath });
                            }
                        }
                        else
                        {
                            DependencyContextLoader.Default.Load(assembly);
                        }                        
                    }

                    LoadModuleDependencies(moduleFolder);
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not load module from " + moduleFolder);
                }
            }

            GlobalContext.SetModuleDependencies(_moduleDependencies);

            return modules;
        }

        private void LoadModuleDependencies(IFileInfo moduleFolder)
        {

            var moduleDependencyFolder = new DirectoryInfo(Path.Combine(moduleFolder.PhysicalPath, Constants.ModuleDepencencyFolder, "Module"));
            if (moduleDependencyFolder.Exists)
            {
                foreach (var file in moduleDependencyFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
                {
                    Assembly assembly;
                    try
                    {
                        assembly = NccAssemblyLoader.LoadFromFileName(file.FullName);
                        DependencyContextLoader.Default.Load(assembly);
                    }
                    catch (FileLoadException ex)
                    {
                        continue;
                    }
                    catch (BadImageFormatException ex)
                    {
                        continue;
                    }
                }
            }


            var viewFolder = new DirectoryInfo(Path.Combine(moduleFolder.PhysicalPath, Constants.ModuleDepencencyFolder,"View"));
            if (viewFolder.Exists)
            {
                foreach (var file in viewFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
                {
                    Assembly assembly;
                    try
                    {
                        var moduleDependency = new ModuleDependedLibrary();
                        if (_moduleDependencies.ContainsKey(moduleFolder.Name))
                        {
                            moduleDependency = (ModuleDependedLibrary)_moduleDependencies[moduleFolder.Name];
                        }
                        else
                        {
                            _moduleDependencies.Add(moduleFolder.Name, moduleDependency);
                        }
                        
                        if (moduleDependency.AssemblyPaths.Contains(Path.Combine(file.FullName)) == false)
                        {
                            assembly = NccAssemblyLoader.LoadFromFileName(file.FullName);
                            DependencyContextLoader.Default.Load(assembly);
                            moduleDependency.AssemblyPaths.Add(file.FullName);

                            _moduleDependencies[moduleFolder.Name] = moduleDependency;
                        }
                    }
                    catch (FileLoadException ex)
                    {
                        continue;
                    }
                    catch (BadImageFormatException ex)
                    {
                        continue;
                    }
                }
            } 
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
                            GlobalMessageRegistry.RegisterMessage(
                                new GlobalMessage()
                                {
                                    For = GlobalMessage.MessageFor.Admin,
                                    Registrater = "AddModulesAsApplicationPart",
                                    Text = $"Duplicate module {module.ModuleTitle}",
                                    Type = GlobalMessage.MessageType.Error
                                },
                                new TimeSpan(0, 0, 30)
                            );
                            continue;
                        }                        
                        
                        // Register dependency in modules                            
                        moduleInstance.Init(services);                        
                        instantiatedModuleList.Add(moduleInstance);
                        GlobalContext.Modules.Add(moduleInstance);
                    }
                }
                catch(ReflectionTypeLoadException rtle)
                {

                }
                catch (Exception ex)
                {
                    GlobalMessageRegistry.RegisterMessage(
                        new GlobalMessage() {
                            For = GlobalMessage.MessageFor.Admin,
                            Registrater = "AddModulesAsApplicationPart",
                            Text = ex.Message,
                            Type = GlobalMessage.MessageType.Error
                        }, 
                        new TimeSpan(0, 0, 60)
                    );
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

        /// <summary>
        /// Loaded modules list
        /// </summary>
        /// <param name="isInstance">Wheather it will retun loaded module list or module instance list.</param>
        /// <returns>Module list</returns>
        public List<IModule> GetModules(bool isInstance = false)
        {
            if (isInstance)
            {
                return instantiatedModuleList;
            }            
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

        public async Task LoadModuleMenus()
        {
            foreach (var item in instantiatedModuleList)
            {
                item.Menus = LoadMenus(item);
            }            
        }

        public List<Menu> LoadMenus(IModule module)
        {
            var menuList = new List<Menu>();
            var controllers =  module.Assembly.GetTypes().Where(x => typeof(NccController).IsAssignableFrom(x)).ToList();
            foreach (var item in controllers)
            {
                var adminMenus = item.GetCustomAttributes<AdminMenu>();
                var adminMenulist = MakeMenuList(module.ModuleId, adminMenus, item, Menu.MenuType.Admin);
                menuList.AddRange(adminMenulist);

                var siteMenus = item.GetCustomAttributes<SiteMenu>();
                var siteMenulist = MakeMenuList(module.ModuleId, siteMenus, item, Menu.MenuType.WebSite);
                menuList.AddRange(siteMenulist);
            }
            
            return menuList;
        }

        private List<Menu> MakeMenuList(string moduleId, IEnumerable<IMenu> moduleMenus, Type controllerType, Menu.MenuType menuType)
        {
            var menuList = new List<Menu>();
            var hasAllowAnonymousOnController = false;
            var anonymous = controllerType.GetCustomAttributes<AllowAnonymousAttribute>();

            if(anonymous != null && anonymous.Count() > 0)
            {
                hasAllowAnonymousOnController = true;
            }
            
            foreach (IMenu menu in moduleMenus)
            {
                var existingMenu = menuList.Where(x => x.DisplayName == menu.Name).FirstOrDefault();
                if(existingMenu == null)
                {
                    existingMenu = new Menu();
                    menuList.Add(existingMenu);
                }
                
                existingMenu.Action = controllerType.Name;
                existingMenu.DisplayName = menu.Name;
                existingMenu.ModuleId = moduleId;
                existingMenu.Controller = controllerType.Name;
                existingMenu.Type = menuType;
                existingMenu.MenuItems = GetMenuItems(moduleId, controllerType, menuType, hasAllowAnonymousOnController); 
            }
            return menuList;
        }

        private List<MenuItem> GetMenuItems(string moduleId, Type controller, Menu.MenuType menuType, bool hasAllowAnonymousOnController)
        {
            var menuItemList = new List<MenuItem>();
            var actions = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var item in actions)
            {
                if(menuType == Menu.MenuType.Admin)
                {
                    var adminMenuAttributes = item.GetCustomAttributes<AdminMenuItem>();
                    var amiList = MakeMenuItemList(moduleId, controller, item, adminMenuAttributes, hasAllowAnonymousOnController);
                    menuItemList.AddRange(amiList);
                }
                else if(menuType == Menu.MenuType.WebSite)
                {
                    var siteMenuAttributes = item.GetCustomAttributes<SiteMenuItem>();
                    var smiList = MakeMenuItemList(moduleId, controller, item, siteMenuAttributes, hasAllowAnonymousOnController);
                    menuItemList.AddRange(smiList);
                }
            }
            return menuItemList;
        }

        private List<MenuItem> MakeMenuItemList(string moduleId, Type controller, MethodInfo action, IEnumerable<IMenuItem> attributes, bool hasAllowAnonymousOnController)
        {
            var menuItemList = new List<MenuItem>();
            var actionHasAllowAnonymous = false;
            if (hasAllowAnonymousOnController && ( action.GetCustomAttributes<NccAuthorize>().Count() == 0 || action.GetCustomAttributes<AllowAnonymousAttribute>().Count() > 0) )
            {
                actionHasAllowAnonymous = true;
            }

            foreach (IMenuItem smi in attributes)
            {
                var ca = new MenuItem();                
                ca.Name = action.Name;
                ca.DisplayName = smi.Name;
                ca.Controller = controller.Name.Substring(0, controller.Name.Length - 10);
                ca.Action = action.Name;
                ca.Url = smi.Url;
                ca.IconCls = smi.IconCls;
                ca.Order = smi.Order;
                ca.SubActions = string.Join(",", smi.SubActions);
                ca.HasAllowAnonymous = actionHasAllowAnonymous;
                menuItemList.Add(ca);

                var controllerAction = new ControllerAction();
                controllerAction.MainAction = ca.Action;
                controllerAction.MainController = ca.Controller;
                controllerAction.MainMenuName = ca.DisplayName;
                controllerAction.ModuleId = moduleId;
                controllerAction.SubController = ca.Controller;
                controllerAction.SubAction = ca.Action;
                
                ControllerActionCache.ControllerActions.Add(controllerAction);

                foreach (var item in smi.SubActions)
                {
                    var controllerActionForSub = new ControllerAction();
                    controllerActionForSub.MainAction = ca.Action;
                    controllerActionForSub.MainController = ca.Controller;
                    controllerActionForSub.MainMenuName = ca.DisplayName;
                    controllerActionForSub.ModuleId = moduleId;
                    (controllerActionForSub.SubController, controllerActionForSub.SubAction) = GetControllerActionFromUrl(ca.Controller, item);                    
                    ControllerActionCache.ControllerActions.Add(controllerActionForSub);
                }
            }
            return menuItemList;
        }

        private (string Controller, string Action) GetControllerActionFromUrl(string mainController, string subAction)
        {
            if(string.IsNullOrEmpty(subAction) == false)
            {
                string controller = "", action = "";

                if (subAction.StartsWith('/'))
                {
                    var parts = subAction.Split("/".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 1)
                    {
                        controller = parts[0];
                        action = parts[1];
                    }
                    else if (parts.Length == 1)
                    {
                        controller = parts[0];
                        action = "Index";
                    }

                    return (controller, action);
                }
                else
                {
                    return (mainController, subAction);
                }
            }
            return ("","");
        }

        public void AddModuleServices(IServiceCollection services)
        {
            foreach (var module in instantiatedModuleList)
            {
                try
                {
                    if (module.ModuleStatus == (int)NccModule.NccModuleStatus.Active)
                    {
                        var repositoryTypes = module.Assembly.GetTypes().Where(x => x.BaseType.IsGenericType).Where(y => y.BaseType.GetGenericTypeDefinition() == typeof(BaseRepository<,>)).ToList();
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

                            var transient = item.GetInterfaces().Where(x => typeof(ITransient).IsAssignableFrom(x)).FirstOrDefault();
                            if (transient != null)
                            {
                                services.AddTransient(item);
                                continue;
                            }

                            services.AddScoped(item);
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

                            var transient = item.GetInterfaces().Where(x => typeof(ITransient).IsAssignableFrom(x)).FirstOrDefault();
                            if (transient != null)
                            {
                                services.AddTransient(item);
                                continue;
                            }

                            services.AddScoped(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobalMessageRegistry.RegisterMessage(
                        new GlobalMessage()
                        {
                            For = GlobalMessage.MessageFor.Admin,
                            Registrater = "AddModuleServices",
                            Text = ex.Message,
                            Type = GlobalMessage.MessageType.Error
                        },
                        new TimeSpan(0, 0, 30)
                    );
                }
            }            
        }       

        public void AddShortcodes(IServiceCollection services)
        {
            var activeModules = instantiatedModuleList.Where(x => x.ModuleStatus == (int)NccModule.NccModuleStatus.Active).ToList();
            foreach (var module in activeModules)
            {
                try
                {
                    var shortCodeTypeList = module.Assembly.GetTypes().Where(x => typeof(IShortCode).IsAssignableFrom(x)).ToList();
                    foreach (var item in shortCodeTypeList)
                    {
                        services.AddTransient(item);
                    }
                }
                catch (Exception ex)
                {
                    GlobalMessageRegistry.RegisterMessage(
                        new GlobalMessage()
                        {
                            For = GlobalMessage.MessageFor.Admin,
                            Registrater = "AddShortcodes",
                            Text = ex.Message,
                            Type = GlobalMessage.MessageType.Error
                        },
                        new TimeSpan(0, 0, 30)
                    );
                }
            }
        }

        public void AddModuleFilters(IServiceCollection services)
        {
            services.AddScoped<NccAuthFilter>();
            services.AddTransient<NccLoggerFilter>();
            services.AddTransient<NccLanguageFilter>();
            services.AddTransient<NccGlobalExceptionFilter>();

            var activeModules = instantiatedModuleList.Where(x => x.ModuleStatus == (int)NccModule.NccModuleStatus.Active).ToList();
            foreach (var module in activeModules)
            {
                try
                {
                    var actionFilters = module.Assembly.GetTypes().Where(x => typeof(INccActionFilter).IsAssignableFrom(x)).ToList();
                    foreach (var item in actionFilters)
                    {
                        services.AddScoped(item);
                    }
                }
                catch (Exception ex)
                {
                    GlobalMessageRegistry.RegisterMessage(
                        new GlobalMessage()
                        {
                            For = GlobalMessage.MessageFor.Admin,
                            Registrater = "AddModuleFilters",
                            Text = ex.Message,
                            Type = GlobalMessage.MessageType.Error
                        },
                        new TimeSpan(0, 0, 30)
                    );
                }
            }            
        } 

        public void AddModuleWidgets( IServiceCollection services)
        {
            foreach (var module in instantiatedModuleList)
            {
                try
                {
                    module.Widgets = new List<Widget>();
                    var widgetTypeList = module.Assembly.GetTypes().Where(x => typeof(Widget).IsAssignableFrom(x)).ToList();
                    foreach (var widgetType in widgetTypeList)
                    {
                        services.AddTransient(widgetType);
                    }
                }
                catch (Exception ex)
                {
                    GlobalMessageRegistry.RegisterMessage(
                        new GlobalMessage()
                        {
                            For = GlobalMessage.MessageFor.Admin,
                            Registrater = "AddModuleWidgets",
                            Text = ex.Message,
                            Type = GlobalMessage.MessageType.Error
                        },
                        new TimeSpan(0, 0, 30)
                    );
                }
            }            
        }

        public void AddModuleAuthorizationHandlers(IServiceCollection services)
        {
            services.AddScoped<NccAuthRequireHandler>();
            var activeModules = instantiatedModuleList.Where(x => x.ModuleStatus == (int)NccModule.NccModuleStatus.Active).ToList();
            foreach (var module in activeModules)
            {
                try
                {
                    var actionFilters = module.Assembly.GetTypes().Where(x => typeof(INccAuthorizationHandler).IsAssignableFrom(x)).ToList();
                    foreach (var item in actionFilters)
                    {
                        services.AddScoped(item);
                    }
                }
                catch (Exception ex)
                {
                    GlobalMessageRegistry.RegisterMessage(
                        new GlobalMessage()
                        {
                            For = GlobalMessage.MessageFor.Admin,
                            Registrater = "AddModuleAuthorizationHandlers",
                            Text = ex.Message,
                            Type = GlobalMessage.MessageType.Error
                        },
                        new TimeSpan(0, 0, 30)
                    );
                }
            }
        }

        public List<Widget> RegisterModuleWidgets(IMvcBuilder mvcBuilder, IServiceCollection services, IServiceProvider serviceProvider)
        {
            var widgetList = new List<Widget>();
            foreach (var module in instantiatedModuleList)
            {
                try
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
                catch (Exception ex)
                {
                    GlobalMessageRegistry.RegisterMessage(
                        new GlobalMessage()
                        {
                            For = GlobalMessage.MessageFor.Admin,
                            Registrater = "RegisterModuleWidget",
                            Text = ex.Message,
                            Type = GlobalMessage.MessageType.Error
                        },
                        new TimeSpan(0, 0, 30)
                    );
                }
            }
            return widgetList;
        }       

        public void RegisterModuleFilters(IMvcBuilder mvcBuilder, IServiceProvider serviceProvider)
        {
            mvcBuilder.AddMvcOptions(option => {
                option.Filters.Add(serviceProvider.GetService<NccGlobalExceptionFilter>());
                option.Filters.Add(serviceProvider.GetService<NccAuthFilter>());                       
                option.Filters.Add(serviceProvider.GetService<NccDataAuthFilter>());
                option.Filters.Add(serviceProvider.GetService<NccLanguageFilter>());
                option.Filters.Add(serviceProvider.GetService<NccLoggerFilter>());
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

        public void RegisterModuleShortCodes(IMvcBuilder mvcBuilder, IServiceProvider serviceProvider)
        {
            var _nccShortCodeProvider = serviceProvider.GetService<NccShortCodeProvider>();
            GlobalContext.ShortCodes = _nccShortCodeProvider.RegisterShortCodes(GlobalContext.Modules);
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
                module.TablePrefix = loadedModule.TablePrefix;
                module.ModuleTitle = loadedModule.ModuleTitle;
                module.MinNccVersion = loadedModule.MinNccVersion;
                module.MaxNccVersion = loadedModule.MaxNccVersion;
                module.SortName = loadedModule.SortName;
                module.Version = loadedModule.Version;
                module.Website = loadedModule.Website;
                module.Assembly = moduleInfo.Assembly;
                module.Path = moduleInfo.Path;
                module.ModuleStatus = moduleInfo.ModuleStatus;
                module.Folder = moduleInfo.Folder;
            }
            else
            {
                GlobalMessageRegistry.RegisterMessage(
                    new GlobalMessage()
                    {
                        For = GlobalMessage.MessageFor.Admin,
                        Registrater = "LoadModuleInfo",
                        Text = $"Could not load module info for '{module.ModuleTitle}'",
                        Type = GlobalMessage.MessageType.Error
                    },
                    new TimeSpan(0, 0, 30)
                );
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
            nccModule.Folder = module.Folder;
            
            var coreModuleDir = Directory.GetParent(nccModule.Path);

            if (coreModuleDir.Name.Equals("Core"))
            {
                nccModule.ModuleStatus = NccModule.NccModuleStatus.Active;
                module.IsCore = true;
                nccModule.IsCore = true;
            }
            else
            {
                nccModule.ModuleStatus = NccModule.NccModuleStatus.New;
            }

            return nccModule;
        }
    }
}
