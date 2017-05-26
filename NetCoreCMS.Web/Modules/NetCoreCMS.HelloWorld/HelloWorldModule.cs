/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.HelloWorld.Widgets;

namespace NetCoreCMS.Modules.HelloWorld
{
    public class HelloWorldModule : IModule
    {
        List<IWidget> _widgets;
        public HelloWorldModule()
        {
            _widgets = new List<IWidget>();
        }

        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string SortName { get; set; }
        public bool AntiForgery { get; set ; }
        public string Author { get ; set ; }
        public string Website { get ; set ; }
        public Version Version { get ; set ; }
        public Version NetCoreCMSVersion { get ; set ; }
        public string Description { get ; set ; }

        public List<string> Dependencies { get ; set ; }
        public string Category { get ; set ; }

        [NotMapped]
        public Assembly Assembly { get; set; }
        public string Path { get ; set ; }
        public NccModule.NccModuleStatus Status { get; set; }
        public string ModuleTitle { get ; set ; }
        [NotMapped]
        public List<IWidget> Widgets { get { return _widgets; } set { _widgets = value; } }

        public bool Activate()
        {
            throw new NotImplementedException();
        }

        public bool Inactivate()
        {
            throw new NotImplementedException();
        }

        public void Init(IServiceCollection services)
        {
            _widgets.Add(new HelloWidget());
        }

        public bool Install()
        {
            throw new NotImplementedException();
        }
          
        public bool Uninstall()
        {
            throw new NotImplementedException();
        }
    }
}
