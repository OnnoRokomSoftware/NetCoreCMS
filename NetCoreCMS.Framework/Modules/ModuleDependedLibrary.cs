using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Modules
{
    public class ModuleDependedLibrary
    {
        public ModuleDependedLibrary() {
            AssemblyPaths = new List<string>();
        }

        public string ModuleId { get; set; }
        public List<string> AssemblyPaths { get; set; }
    }
}
