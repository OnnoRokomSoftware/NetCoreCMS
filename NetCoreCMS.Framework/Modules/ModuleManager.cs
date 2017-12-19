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
using NetCoreCMS.Framework.Core.Mvc.Filters;
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

        public void LoadModules(IDirectoryContents moduleRootFolder)
        {   
            foreach (var moduleFolder in moduleRootFolder.Where(x => x.IsDirectory).ToList())
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
                                modules.Add(new Module { ExecutionOrder = 100, ModuleName = moduleFolder.Name, Folder = moduleFolder.Name, Assembly = assembly, Path = moduleFolder.PhysicalPath });
                            }
                        }
                        else
                        {
                            DependencyContextLoader.Default.Load(assembly);
                        }                        
                    }                    
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not load module from " + moduleFolder);
                }
            }
        }

        private void LoadModuleDependencies(string modulePath, string moduleName)
        {

            var moduleDependencyFolder = new DirectoryInfo(Path.Combine(modulePath, Constants.ModuleDepencencyFolder, "Module"));
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


            var viewFolder = new DirectoryInfo(Path.Combine(modulePath, Constants.ModuleDepencencyFolder,"View"));
            if (viewFolder.Exists)
            {
                foreach (var file in viewFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
                {
                    Assembly assembly;
                    try
                    {
                        var moduleDependency = new ModuleDependedLibrary();
                        if (_moduleDependencies.ContainsKey(moduleName))
                        {
                            moduleDependency = (ModuleDependedLibrary)_moduleDependencies[moduleName];
                        }
                        else
                        {
                            _moduleDependencies.Add(moduleName, moduleDependency);
                        }
                        
                        if (moduleDependency.AssemblyPaths.Contains(Path.Combine(file.FullName)) == false)
                        {
                            assembly = NccAssemblyLoader.LoadFromFileName(file.FullName);
                            DependencyContextLoader.Default.Load(assembly);
                            moduleDependency.AssemblyPaths.Add(file.FullName);
                            _moduleDependencies[moduleName] = moduleDependency;
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
            var nccSettingsService = serviceProvider.GetService<INccSettingsService>();

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
                            // Register dependency in modules
                            LoadModuleDependencies(module.Path, module.Folder);
                            moduleInstance.Init(services, nccSettingsService);
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

            GlobalContext.SetModuleDependencies(_moduleDependencies);

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ModuleViewLocationExpendar());
            });
            
            mvcBuilder.AddRazorOptions(o =>
            {
                foreach (var module in instantiatedModuleList.Where(x=>x.ModuleStatus == (int) NccModule.NccModuleStatus.Active).ToList())
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
            NccModule moduleEntity = moduleService.GetByModuleName(module.ModuleName);
            if(moduleEntity == null)
            {
                moduleEntity = CreateNccModuleEntity(module);
                moduleService.Save(moduleEntity);
            }
            else if(moduleEntity.ModuleName != module.ModuleName)
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
                var adminMenulist = MakeMenuList(module, adminMenus, item, Menu.MenuType.Admin);
                menuList.AddRange(adminMenulist);

                var siteMenus = item.GetCustomAttributes<SiteMenu>();
                var siteMenulist = MakeMenuList(module, siteMenus, item, Menu.MenuType.WebSite);
                menuList.AddRange(siteMenulist);
            }
            
            return menuList;
        }

        private List<Menu> MakeMenuList(IModule module, IEnumerable<IMenu> moduleMenus, Type controllerType, Menu.MenuType menuType)
        {
            var menuList = new List<Menu>();
            var hasAllowAnonymousOnController = false;
            var hasAllowAuthenticatedOnController = false;

            var anonymous = controllerType.GetCustomAttributes<AllowAnonymousAttribute>();

            if(anonymous != null && anonymous.Count() > 0)
            {
                hasAllowAnonymousOnController = true;
            }

            var allowAuthenticated = controllerType.GetCustomAttributes<AllowAuthenticated>();

            if (allowAuthenticated != null && allowAuthenticated.Count() > 0)
            {
                hasAllowAuthenticatedOnController = true;
            }

            foreach (IMenu menu in moduleMenus)
            {
                var existingMenu = menuList.Where(x => x.DisplayName == menu.Name).FirstOrDefault();
                if(existingMenu == null)
                {
                    existingMenu = new Menu();
                    menuList.Add(existingMenu);
                }

                existingMenu.Area = module.Area;
                existingMenu.Controller = controllerType.Name;
                existingMenu.Action = "Index";
                existingMenu.DisplayName = menu.Name;
                existingMenu.ModuleName = module.Folder;
                
                existingMenu.Type = menuType;
                existingMenu.MenuItems = GetMenuItems(module, controllerType, menuType, hasAllowAnonymousOnController, hasAllowAuthenticatedOnController); 
            }
            return menuList;
        }

        private List<MenuItem> GetMenuItems(IModule module, Type controller, Menu.MenuType menuType, bool hasAllowAnonymousOnController, bool hasAllowAuthenticatedOnController)
        {
            var menuItemList = new List<MenuItem>();
            var actions = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var item in actions)
            {
                if(menuType == Menu.MenuType.Admin)
                {
                    var adminMenuAttributes = item.GetCustomAttributes<AdminMenuItem>();
                    var amiList = MakeMenuItemList(module, controller, item, adminMenuAttributes, hasAllowAnonymousOnController, hasAllowAuthenticatedOnController);
                    menuItemList.AddRange(amiList);
                }
                else if(menuType == Menu.MenuType.WebSite)
                {
                    var siteMenuAttributes = item.GetCustomAttributes<SiteMenuItem>();
                    var smiList = MakeMenuItemList(module, controller, item, siteMenuAttributes, hasAllowAnonymousOnController, hasAllowAuthenticatedOnController);
                    menuItemList.AddRange(smiList);
                }
            }
            return menuItemList;
        }

        private List<MenuItem> MakeMenuItemList(IModule module, Type controller, MethodInfo action, IEnumerable<IMenuItem> attributes, bool hasAllowAnonymousOnController, bool hasAllowAuthenticatedOnController)
        {
            var menuItemList = new List<MenuItem>();
            var actionHasAllowAnonymous = false;

            if (hasAllowAnonymousOnController || action.GetCustomAttributes<AllowAnonymousAttribute>().Count() > 0)
            {
                actionHasAllowAnonymous = true;
            }

            var actionHasAllowAuthenticated = false;

            if (hasAllowAuthenticatedOnController || action.GetCustomAttributes<AllowAuthenticated>().Count() > 0)
            {
                actionHasAllowAuthenticated = true;
            }

            foreach (IMenuItem smi in attributes)
            {
                var ca = new MenuItem();                
                ca.Name = action.Name;
                ca.DisplayName = smi.Name;
                ca.Area = module.Area;
                ca.Controller = controller.Name.Substring(0, controller.Name.Length - 10);
                ca.Action = action.Name;
                ca.Url = smi.Url;
                ca.HasAllowAnonymous = actionHasAllowAnonymous;
                ca.HasAllowAuthenticated = actionHasAllowAuthenticated;

                if (string.IsNullOrWhiteSpace(ca.Url))
                {
                    if (string.IsNullOrWhiteSpace(module.Area))
                    {
                        ca.Url = "/" + ca.Controller + "/" + ca.Action;
                    }
                    else
                    {
                        ca.Url = "/" + ca.Area + "/" + ca.Controller + "/" + ca.Action;
                    }
                }
                                
                ca.IconCls = smi.IconCls;
                ca.Order = smi.Order;

                ca.SubActions = string.Join(",", smi.SubActions);
                
                menuItemList.Add(ca);

                var controllerAction = new ControllerAction();

                controllerAction.MainArea = ca.Area;
                controllerAction.MainAction = ca.Action;
                controllerAction.MainController = ca.Controller;
                controllerAction.MainMenuName = ca.DisplayName;
                controllerAction.ModuleName = module.Folder;

                controllerAction.SubArea = ca.Area;
                controllerAction.SubController = ca.Controller;
                controllerAction.SubAction = ca.Action;

                controllerAction.HasAllowAnonymous = actionHasAllowAnonymous;
                controllerAction.HasAllowAuthenticated = actionHasAllowAuthenticated;

                ControllerActionCache.ControllerActions.Add(controllerAction);

                foreach (var item in smi.SubActions)
                {
                    var controllerActionForSub = new ControllerAction();
                    controllerActionForSub.MainArea = ca.Area;
                    controllerActionForSub.MainAction = ca.Action;
                    controllerActionForSub.MainController = ca.Controller;
                    controllerActionForSub.MainMenuName = ca.DisplayName;
                    controllerActionForSub.ModuleName = module.Folder;

                    (controllerActionForSub.SubArea, controllerActionForSub.SubController, controllerActionForSub.SubAction) = GetControllerActionFromSubActionUrl(ca.Area, ca.Controller, ca.Action, item);

                    ControllerActionCache.ControllerActions.Add(controllerActionForSub);
                }
            }

            return menuItemList;
        }

        private (string Area, string Controller, string Action) GetControllerActionFromSubActionUrl(string mainArea, string mainController, string mainAction, string subAction)
        {
            if(string.IsNullOrEmpty(subAction) == false)
            {
                string area = "", controller = "", action = "";
                var parts = subAction.Split("/".ToArray(), StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 0)
                {                    
                    if (parts.Length > 2)
                    {
                        area = parts[0];
                        controller = parts[1];
                        action = parts[2];
                    }
                    else if (parts.Length > 1)
                    {
                        controller = parts[0];
                        action = parts[1];
                    }
                    else if (parts.Length > 0)
                    {
                        controller = mainController;
                        action = parts[0];
                    }

                    return (area, controller, action);
                }
                else
                {
                    return (mainArea, mainController, mainAction);
                }
            }
            return ("", "","");
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
                        services.AddScoped(item);
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
            services.AddScoped<NccLoggerFilter>();
            services.AddScoped<NccLanguageFilter>();
            services.AddScoped<NccGlobalExceptionFilter>();
            
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
                        services.AddScoped(widgetType);
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
                            GlobalContext.WidgetTypes.Add(widgetInstance.WidgetId, widgetType);
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
        
        public void LoadModuleInfo(IModule module, IModule moduleAssembly)
        {
            var moduleConfigFile = Path.Combine(moduleAssembly.Path, Constants.ModuleConfigFileName);
            if (File.Exists(moduleConfigFile))
            {                
                var moduleInfoFileJson = File.ReadAllText(moduleConfigFile);
                var loadedModuleInfo = JsonConvert.DeserializeObject<Module>(moduleInfoFileJson);
                //module.ModuleName = loadedModule.ModuleName;                
                module.AntiForgery = loadedModuleInfo.AntiForgery;
                module.Author = loadedModuleInfo.Author;
                module.Category = loadedModuleInfo.Category;
                module.Dependencies = loadedModuleInfo.Dependencies;
                module.Description = loadedModuleInfo.Description;
                //module.ModuleName = loadedModule.ModuleName;
                module.TablePrefix = loadedModuleInfo.TablePrefix;
                module.ModuleTitle = loadedModuleInfo.ModuleTitle;
                module.NccVersion = loadedModuleInfo.NccVersion;                
                module.SortName = loadedModuleInfo.SortName;
                module.Version = loadedModuleInfo.Version;
                module.Website = loadedModuleInfo.Website;
                module.Assembly = moduleAssembly.Assembly;
                module.Path = moduleAssembly.Path;
                module.ModuleStatus = moduleAssembly.ModuleStatus;
                module.Folder = moduleAssembly.Folder;
                module.ModuleName = moduleAssembly.ModuleName;
                module.ExecutionOrder = moduleAssembly.ExecutionOrder;

                //moduleInfo.ModuleName = loadedModule.ModuleName;                
                moduleAssembly.AntiForgery = loadedModuleInfo.AntiForgery;
                moduleAssembly.Author = loadedModuleInfo.Author;
                moduleAssembly.Category = loadedModuleInfo.Category;
                moduleAssembly.Dependencies = loadedModuleInfo.Dependencies;
                moduleAssembly.Description = loadedModuleInfo.Description;
                moduleAssembly.ModuleName = loadedModuleInfo.ModuleName;
                moduleAssembly.TablePrefix = loadedModuleInfo.TablePrefix;
                moduleAssembly.ModuleTitle = loadedModuleInfo.ModuleTitle;
                moduleAssembly.NccVersion = loadedModuleInfo.NccVersion;                
                moduleAssembly.SortName = loadedModuleInfo.SortName;
                moduleAssembly.Version = loadedModuleInfo.Version;
                moduleAssembly.Website = loadedModuleInfo.Website; 

                if(loadedModuleInfo.ExecutionOrder != 0)
                {
                    module.ExecutionOrder = loadedModuleInfo.ExecutionOrder;
                    moduleAssembly.ExecutionOrder = loadedModuleInfo.ExecutionOrder;
                }
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
            nccModule.ModuleName = module.ModuleName;
            nccModule.Dependencies = module.Dependencies;
            nccModule.NccVersion = module.NccVersion;            
            nccModule.Path = module.Path;
            nccModule.Folder = module.Folder;
            nccModule.Version = module.Version;
            nccModule.Description = module.Description;
            nccModule.Category = module.Category;
            nccModule.Author = module.Author;
            nccModule.WebSite = module.Website;
            nccModule.ModuleTitle = module.ModuleTitle;
            nccModule.Folder = module.Folder;
            nccModule.ExecutionOrder = module.ExecutionOrder;
            
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
