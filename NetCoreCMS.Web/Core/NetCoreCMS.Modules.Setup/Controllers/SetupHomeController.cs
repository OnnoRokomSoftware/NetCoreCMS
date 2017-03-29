using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;


namespace NetCoreCMS.Modules.Cms.Controllers
{
    public class SetupHomeController : NccController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
