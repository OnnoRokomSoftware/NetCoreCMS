using NetCoreCMS.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Admin.Models.ViewModels
{
    public class StartupViewModel
    {
        public List<NccPage> Pages { get; set; }
        public List<NccCategory> Categories { get; set; }

    }
}
