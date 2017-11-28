using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Auth
{
    public class ControllerAction
    {
        public string ModuleId { get; set; }
        public string MainController { get; set; }
        public string MainAction { get; set; }
        public string MainMenuName { get; set; }
        public string SubController { get; set; }
        public string SubAction { get; set; }
    }
}
