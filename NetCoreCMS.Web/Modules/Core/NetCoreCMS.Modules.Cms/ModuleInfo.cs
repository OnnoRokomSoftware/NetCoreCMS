using NetCoreCMS.Framework.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Cms
{
    public class ModuleInfo : ModuleInfoBase
    {
        public ModuleInfo()
        {
            ModuleName = "NetCoreCMS.Modules.Cms";
            Author = "Xonaki";
            Website = "http://xonaki.com";
            AntiForgery = true;
            Description = "Builtin Content Management System Module.";
            Version = new Version(0, 1, 1);
            NetCoreCMSVersion = new Version(0, 1, 1);
        }
    }
}
