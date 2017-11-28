/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Core.App;
using NetCoreCMS.Framework.Core.Events.Themes;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Admin.Models.ViewModels;
using Newtonsoft.Json;

namespace NetCoreCMS.Core.Modules.Admin.Controllers
{
    [AdminMenu(Name = "Appearance", IconCls = "fa-tasks", Order = 4)]
    public class CmsThemeController : NccController
    {
        private IHostingEnvironment _env;
        ThemeManager _themeManager;
        //ILoggerFactory _loggerFactory;
        private readonly string _themePath = "Themes\\";
        private readonly IMediator _mediator;

        public CmsThemeController(ThemeManager themeManager, IMediator mediator, ILoggerFactory factory)
        {
            _themeManager = themeManager;
            _mediator = mediator;
            _logger = factory.CreateLogger<CmsThemeController>();
        }

        [AdminMenuItem(Name = "Theme", Url = "/CmsTheme", IconCls = "fa-laptop", Order = 3)]
        public ActionResult Index()
        {
            SetThemeViewData();
            return View();
        }
        
        [AdminMenuItem(Name = "Theme Settings", Url = "/CmsTheme/Settings", IconCls = "fa-laptop", Order = 4)]        
        public ActionResult Settings()
        {
            var activeTheme = ThemeHelper.ActiveTheme;
            return Redirect("/" + activeTheme.ThemeName + "Theme");
        }

        private void SetThemeViewData()
        {
            var themePath = Path.Combine(GlobalContext.ContentRootPath, NccInfo.ThemeFolder);
            ViewBag.Themes = GlobalContext.Themes.OrderByDescending(x=>x.IsActive);
            ViewBag.ThemePath = themePath;
        }

        public ActionResult Activate(string themeName)
        {
            _themeManager.ActivateTheme(themeName);
            NetCoreCmsHost.IsRestartRequired = true;
            string successMessage = "Theme " + themeName + " Activated Successfully.";
            TempData["ThemeSuccessMessage"] = successMessage;
            FireEvent(ThemeActivity.Type.Activated, GlobalContext.GetThemeByName(themeName));
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

        #region Private Methods
        private ThemeActivity FireEvent(ThemeActivity.Type type, Theme theme)
        {
            try
            {
                var rsp = _mediator.SendAll(new OnThemeActivity(new ThemeActivity()
                {
                    ActivityType = type,
                    Theme = theme
                })).Result;
                return rsp.LastOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return null;
        }
        #endregion
    }
}
