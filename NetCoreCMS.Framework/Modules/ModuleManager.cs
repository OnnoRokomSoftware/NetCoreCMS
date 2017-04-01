using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;


namespace NetCoreCMS.Framework.Modules
{
    public class ModuleManager
    {
        List<INccModule> modules = new List<INccModule>();
        public List<INccModule> LoadModules(IDirectoryContents moduleRootFolder)
        {   
            foreach (var moduleFolder in moduleRootFolder.Where(x => x.IsDirectory))
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
                    catch (FileLoadException ex)
                    { 
                        continue;
                    }
                    
                    if (assembly.FullName.Contains(moduleFolder.Name))
                    {
                        modules.Add(new NccModule { ModuleName = moduleFolder.Name, Assembly = assembly, Path = moduleFolder.PhysicalPath });
                    }
                }
            }
            return modules;
        }
        
        public void RegisterModules(IMvcBuilder mvcBuilder, IServiceCollection services)
        {
            foreach (var module in modules)
            {
                // Register controller from modules
                mvcBuilder.AddApplicationPart(module.Assembly);

                // Register dependency in modules
                var moduleInitializerType = module.Assembly.GetTypes().Where(x => typeof(INccModule).IsAssignableFrom(x)).FirstOrDefault();
                if (moduleInitializerType != null && moduleInitializerType != typeof(INccModule))
                {
                    var moduleInitializer = (INccModule) Activator.CreateInstance(moduleInitializerType);
                    moduleInitializer.Init(services);
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
        }

        public void RegisterStaticFiles(IApplicationBuilder app)
        {
            foreach (var module in modules)
            {
                var moduleDir = new DirectoryInfo(Path.Combine(module.Path,"wwwroot"));
                if (moduleDir.Exists)
                {
                    app.UseStaticFiles(new StaticFileOptions()
                    {
                        FileProvider = new PhysicalFileProvider(moduleDir.FullName),
                        RequestPath = new PathString("/" + module.ModuleName)
                    });
                }                
            }
        }
    }
}
