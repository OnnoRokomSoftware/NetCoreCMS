using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using NetCoreCMS.Framework.Setup;

namespace NetCoreCMS.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {
        IHostingEnvironment _env;
        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            if (!SetupHelper.IsComplete)
            {
                return Redirect("/SetupHome/Index");
            }
            return View();
        }
        
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
