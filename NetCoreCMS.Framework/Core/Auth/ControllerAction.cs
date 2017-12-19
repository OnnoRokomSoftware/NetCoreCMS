namespace NetCoreCMS.Framework.Core.Auth
{
    public class ControllerAction
    {
        public string ModuleName { get; set; }
        public string MainArea { get; set; }
        public string MainController { get; set; }
        public string MainAction { get; set; }
        public string MainMenuName { get; set; }
        public string SubArea { get; set; }
        public string SubController { get; set; }
        public string SubAction { get; set; }
        public bool HasAllowAnonymous { get; set; }
        public bool HasAllowAuthenticated { get; set; }

    }
}
