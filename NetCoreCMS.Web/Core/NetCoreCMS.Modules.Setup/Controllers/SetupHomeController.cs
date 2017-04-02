using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Setup.Models.ViewModels;

namespace NetCoreCMS.Modules.Cms.Controllers
{
    public class SetupHomeController : NccController
    {
        IOptions<SetupConfig> _setupOption;
        IHostingEnvironment _env;
        public SetupHomeController(IHostingEnvironment env)
        {
            _env = env;
        }
        public ActionResult Index()
        {
            if (SetupHelper.IsComplete)
                return RedirectToAction("Success");
            return View();
        }

        [HttpPost]
        public ActionResult Index(SetupViewModel setup)
        {
            if (ModelState.IsValid)
            {
                SetupHelper.ConnectionString = DbContextManager.GetConnectionString(_env, setup.Database, setup.DatabaseHost, setup.DatabasePort, setup.DatabaseName, setup.DatabaseUserName, setup.DatabasePassword);
                SetupHelper.IsComplete = true;
                SetupHelper.SelectedDatabase = setup.Database.ToString();
                SetupHelper.SaveSetup(_env);
                return RedirectToAction("Success");
            }
            else
            {
                ModelState.AddModelError("SiteName", "Invalid Datas");
            }

            return View();
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}
