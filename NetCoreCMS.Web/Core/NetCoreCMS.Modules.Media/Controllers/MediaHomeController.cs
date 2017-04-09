using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;


namespace NetCoreCMS.Core.Modules.Media.Controllers
{
    public class MediaHomeController : NccController
    {
        public ActionResult Index()
        { 
            return View(); 
        } 
    }
}
