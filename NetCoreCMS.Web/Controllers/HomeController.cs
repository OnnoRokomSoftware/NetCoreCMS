using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Helper;
using Microsoft.Extensions.Options;

namespace NetCoreCMS.Web.Controllers
{
    public class HomeController : Controller
    {
        IOptions<SetupOption> _setupOption;
        public HomeController(IOptions<SetupOption> setupOption)
        {
            _setupOption = setupOption;
        }
        public IActionResult Index()
        {
            if (!_setupOption.Value.IsComplete)
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
