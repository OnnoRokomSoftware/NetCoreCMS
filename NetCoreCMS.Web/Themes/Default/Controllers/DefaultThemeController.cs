using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Utility;
using System.Collections.Generic;

namespace Default.Controllers
{
    public class DefaultThemeController : NccController
    {
        public DefaultThemeController()
        {

        }

        public ActionResult Index()
        {
            Dictionary<string, string> style = new Dictionary<string, string>() { { "light", "Light" }, { "dark", "Dark" } };

            ViewBag.style = new SelectList(style, "Key", "Value", GlobalConfig.ActiveTheme.Settings["style"]);
            return View();
        }
        [HttpPost]
        public ActionResult Index(string[] key, string[] value)
        {
            for (int i = 0; i < key.Length; i++)
            {
                GlobalConfig.ActiveTheme.Settings.Remove(key[i]);
                GlobalConfig.ActiveTheme.Settings.Add(key[i], value[i]);
            }
            GlobalConfig.ActiveTheme.Save();
            TempData["ThemeSuccessMessage"] = "Settings Updated Successfully";
            return RedirectToAction("Index");
        }
    }
}
