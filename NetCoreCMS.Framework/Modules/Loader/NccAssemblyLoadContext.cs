using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace NetCoreCMS.Framework.Modules.Loader
{
    public class NccAssemblyLoadContext : AssemblyLoadContext
    {
        protected override Assembly Load(AssemblyName assemblyName) => NccAssemblyLoader.LoadFromFileName(assemblyName.Name);
    }
}
