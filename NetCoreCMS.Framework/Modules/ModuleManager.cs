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

namespace NetCoreCMS.Framework.Modules
{
    public class ModuleManager
    {
        List<IModule> modules = new List<IModule>();
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
        
        public List<IModule> RegisterModules(IMvcBuilder mvcBuilder, IServiceCollection services)
        {
            foreach (var module in modules)
            {
                try
                {
                    // Register controller from modules
                    mvcBuilder.AddApplicationPart(module.Assembly);

                    // Register dependency in modules
                    var moduleInitializerType = module.Assembly.GetTypes().Where(x => typeof(IModule).IsAssignableFrom(x)).FirstOrDefault();
                    if (moduleInitializerType != null && moduleInitializerType != typeof(IModule))
                    {
                        var moduleInitializer = (IModule)Activator.CreateInstance(moduleInitializerType);
                        moduleInitializer.Init(services);
                    }
                }
                catch (Exception ex)
                {

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

            return modules;
        }
        
    }
}
