using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Microsoft.Extensions.DependencyModel;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.Modules.Loader
{
    public static class NccAssemblyLoader
    {
        public static Assembly LoadFromFileName(string assemblyFullPath)
        {
            var fileNameWithOutExtension = Path.GetFileNameWithoutExtension(assemblyFullPath);

            var inCompileLibraries = DependencyContext.Default.CompileLibraries.Any(l => l.Name.Equals(fileNameWithOutExtension, StringComparison.OrdinalIgnoreCase));
            var inRuntimeLibraries = DependencyContext.Default.RuntimeLibraries.Any(l => l.Name.Equals(fileNameWithOutExtension, StringComparison.OrdinalIgnoreCase));

            var assembly = (inCompileLibraries || inRuntimeLibraries)
                ? Assembly.Load(new AssemblyName(fileNameWithOutExtension))
                : AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyFullPath);

            return assembly;
        }

        public static Assembly LoadFromModuleFolders(string assemblyName)
        {
            foreach (var item in GlobalContext.GetActiveModules())
            {
                var assemblyFullPath = Path.Combine(item.Path, assemblyName);
                var fileNameWithOutExtension = Path.GetFileNameWithoutExtension(assemblyFullPath);

                var inCompileLibraries = DependencyContext.Default.CompileLibraries.Any(l => l.Name.Equals(fileNameWithOutExtension, StringComparison.OrdinalIgnoreCase));
                var inRuntimeLibraries = DependencyContext.Default.RuntimeLibraries.Any(l => l.Name.Equals(fileNameWithOutExtension, StringComparison.OrdinalIgnoreCase));

                var assembly = (inCompileLibraries || inRuntimeLibraries)
                    ? Assembly.Load(new AssemblyName(fileNameWithOutExtension))
                    : AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyFullPath);

                return assembly;
            }
            return null;
        }
    }
}
