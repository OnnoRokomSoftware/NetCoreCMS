using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using NetCoreCMS.Framework.Setup;

namespace NetCoreCMS.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        IHostingEnvironment _env;
        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            if (SetupHelper.IsDbCreateComplete && SetupHelper.IsAdminCreateComplete)
            {
                return Redirect("/CmsHome/Index");
            }
            return Redirect("/SetupHome/Index");
        }
         
        public IActionResult Error()
        {
            return View();
        }
         
        public IActionResult SetupSuccess()
        {
            Program.Shutdown();
            return View();
        }
         
        public IActionResult RestartHost()
        {
            NetCoreCmsHost.IsRestartRequired = true;
            Program.Shutdown();
            return View();
        }
    }
}
