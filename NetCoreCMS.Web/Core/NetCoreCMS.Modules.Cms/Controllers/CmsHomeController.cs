using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Setup;

namespace NetCoreCMS.Modules.Cms.Controllers
{
    public class CmsHomeController : NccController
    {
        public ActionResult Index()
        {
            if (!SetupHelper.IsComplete)
            {
                return Redirect("/SetupHome/Index");
            }
            return View();
        }
        
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
    }
}
