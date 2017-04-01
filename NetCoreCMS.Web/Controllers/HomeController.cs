using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Helper;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;

namespace NetCoreCMS.Web.Controllers
{
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

        public IActionResult Error()
        {
            return View();
        }
    }
}
