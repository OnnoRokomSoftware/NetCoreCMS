using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.Extensions.DependencyModel;

namespace NetCoreCMS.Framework.Modules.AssemblyLoader
{
    public class ReferencePluginAssemblyProvider
    {
        public ReferencePluginAssemblyProvider()
        {
            var runtimeLib = DependencyContext.Default.RuntimeLibraries;
            var compileLibrary = DependencyContext.Default.CompileLibraries;            
        }
    }
    //public class ReferencePluginAssemblyProvider : DefaultAssemblyProvider
    //{
    //    //NOTE: The DefaultAssemblyProvider uses ILibraryManager to do the library/assembly querying
    //    public ReferencePluginAssemblyProvider(ILibraryManager libraryManager) : base(libraryManager)
    //    {
    //    }

    //    protected override HashSet<string> ReferenceAssemblies
    //        => new HashSet<string>(new[] { "MyProduct.Web", "MyProduct.Core" });
    //}
}
