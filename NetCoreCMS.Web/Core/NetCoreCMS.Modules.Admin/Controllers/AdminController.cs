using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;


namespace NetCoreCMS.Core.Modules.Admin.Controllers
{
    public class AdminController : NccController
    {
        public ActionResult Index()
        { 
            return View(); 
        } 
    }
}
