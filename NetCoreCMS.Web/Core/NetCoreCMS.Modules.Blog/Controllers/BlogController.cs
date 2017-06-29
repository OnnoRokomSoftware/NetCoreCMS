using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Core.Modules.Media.Controllers
{
    [Authorize(Roles ="SuperAdmin,Adimistrator,Editor")]
    [AdminMenu(Name ="Blog", Order = 1)]
    public class BlogController : NccController
    {
        [AllowAnonymous]
        public ActionResult Index(int page = 0)
        { 
            return View(); 
        }

        [AdminMenuItem(Name ="New Post", Order = 1, Url = "/Blog/CreateEdit")]
        public ActionResult CreateEdit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEdit(NccPost post)
        {
            return View();
        }

        [AdminMenuItem(Name = "Manage", Order = 2, Url = "/Blog/Manage")]
        public ActionResult Manage()
        {
            return View();
        }
    }
}
