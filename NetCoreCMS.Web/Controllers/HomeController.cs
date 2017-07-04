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
                var setupConfig = SetupHelper.LoadSetup();
                if(setupConfig == null)
                {
                    TempData["Message"] = "Setup config file is missed. Please reinstall.";
                    return Redirect("~/CmsHome/ResourceNotFound");
                }
                return Redirect(setupConfig.StartupUrl);
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
