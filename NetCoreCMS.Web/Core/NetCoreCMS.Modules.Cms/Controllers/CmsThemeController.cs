using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Core.App;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Cms.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreCMS.Core.Modules.Cms.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    [AdminMenu(Name = "Appearance", IconCls = "fa-tasks", Order = 5)]
    public class CmsThemeController : NccController
    {
        private IHostingEnvironment _env;
        ThemeManager _themeManager;
        //ILoggerFactory _loggerFactory;
        private readonly string _themePath = "Themes\\";

        public CmsThemeController(ThemeManager themeManager, ILoggerFactory factory)
        {
            _themeManager = themeManager;
            //_loggerFactory = factory;
            _logger = factory.CreateLogger<CmsThemeController>();
        }

        [AdminMenuItem(Name = "Theme", Url = "/CmsTheme", IconCls = "fa-laptop", Order = 3)]
        public ActionResult Index()
        {
            SetThemeViewData();
            return View();
        }

        private void SetThemeViewData()
        {
            var themePath = Path.Combine(GlobalConfig.ContentRootPath, NccInfo.ThemeFolder);
            ViewBag.Themes = GlobalConfig.Themes.OrderByDescending(x=>x.IsActive);
            ViewBag.ThemePath = themePath;
        }

        public ActionResult Activate(string themeName)
        {
            _themeManager.ActivateTheme(themeName);
            NetCoreCmsHost.IsRestartRequired = true;
            string successMessage = "Theme " + themeName + " Activated Successfully.";
            TempData["ThemeSuccessMessage"] = successMessage;
            return RedirectToAction("Index");
        }

        public ActionResult Install()
        {
            SetThemeViewData();
            return View();
        }

        #region Online Gallery
        public async Task<JsonResult> GetMatGalleryThemes()
        {
            ApiResponse resp = new ApiResponse();
            resp.IsSuccess = true;
            resp.Message = "";
            try
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri("https://gallery.osl.one/MatGalleryApi/Themes");
                        var response = await client.GetAsync("?key=abc");
                        response.EnsureSuccessStatusCode(); // Throw in not success

                        var stringResponse = await response.Content.ReadAsStringAsync();
                        resp.Data = JsonConvert.DeserializeObject<IEnumerable<NccThemeViewModel>>(stringResponse);

                        resp.IsSuccess = true;
                        resp.Message = "Success";
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine($"Request exception: {e.Message}");
                    }
                }
            }
            catch (Exception) { }
            return Json(resp);
        }

        public async Task<JsonResult> DownloadTheme(string key = "", string themeId = "", string themeName = "")
        {
            ApiResponse resp = new ApiResponse();
            resp.IsSuccess = true;
            resp.Message = "";
            if (themeId.Trim() != "")
            {
                try
                {
                    string url = "https://gallery.osl.one/MatGalleryApi/DownloadTheme?themeId=" + themeId;
                    #region Download in temp folder
                    var tempFullFilepath = Path.Combine(_env.ContentRootPath, _themePath + "\\_temp");
                    try
                    {
                        if (resp.IsSuccess == true)
                        {
                            if (!Directory.Exists(tempFullFilepath))
                            {
                                Directory.CreateDirectory(tempFullFilepath);
                            }
                            using (var client = new HttpClient())
                            {
                                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                                {
                                    using (Stream contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync(), stream = new FileStream(_themePath + "\\_temp\\" + themeName + ".zip", FileMode.Create, FileAccess.Write))
                                    {
                                        await contentStream.CopyToAsync(stream);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        resp.IsSuccess = false;
                        resp.Message += "Module Download Failed.";
                    }
                    #endregion

                    #region Check & Create module folder
                    var finalFolderPath = Path.Combine(_env.ContentRootPath, _themePath + "\\" + themeName);
                    try
                    {
                        if (resp.IsSuccess == true)
                        {
                            if (!Directory.Exists(finalFolderPath))
                            {
                                Directory.CreateDirectory(finalFolderPath);
                            }
                            else
                            {
                                //Delete is not working perfectly. Bin folder dll file is in use cannot delete.
                                Thread.Sleep(2000);
                                try
                                {
                                    string strCmdText;
                                    strCmdText = "rd /s /q \"" + finalFolderPath + "\"";
                                    System.Diagnostics.Process.Start("CMD.exe", strCmdText);
                                    //Directory.Delete(finalFolderPath, true);
                                    //DeleteDirectory(finalFolderPath);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex.ToString());
                                    resp.IsSuccess = false;
                                    resp.Message += "Previous module folder delete failed.";
                                }
                                Directory.CreateDirectory(finalFolderPath);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        resp.IsSuccess = false;
                        resp.Message += "Module folder creation failed.";
                    }
                    #endregion

                    #region Unzip in folder location     
                    try
                    {
                        if (resp.IsSuccess == true)
                        {
                            ZipFile.ExtractToDirectory(tempFullFilepath + "\\" + themeName + ".zip", finalFolderPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        resp.IsSuccess = false;
                        resp.Message += "Module Unzip Failed.";
                    }

                    try
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (System.IO.File.Exists(tempFullFilepath + "\\" + themeName + ".zip") == true)
                            {
                                Thread.Sleep(2000);
                                System.IO.File.Delete(tempFullFilepath + "\\" + themeName + ".zip");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        resp.IsSuccess = false;
                        resp.Message += "Downloaded temporary file remove failed.";
                    }
                    #endregion                       
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    resp.IsSuccess = false;
                    resp.Message = ex.Message;
                }
            }
            if (resp.IsSuccess == true)
            {
                resp.Message = "Module downloaded and restored successfully";
            }
            return Json(resp);
        }
        #endregion

        //[HttpPost]
        //public ActionResult Install(HttpPostedFileBase file)
        //{
        //    SetThemeViewData();
        //    return View();
        //}
    }
}
