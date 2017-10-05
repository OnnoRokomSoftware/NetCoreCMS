using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.MatGallery.Models;
using NetCoreCMS.MatGallery.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreCMS.MatGallery.Controllers
{

    [Authorize(Roles = "SuperAdmin,Administrator,Editor")]
    [AdminMenu(Name = "Gallery Management", IconCls = "", Order = 100)]
    public class MatgThemeController : NccController
    {
        #region Initialization
        private IHostingEnvironment _env;
        private NccSettingsService _nccSettingsService;
        private NccUserThemeService _nccUserThemeService;
        private Settings settings;

        private readonly string _tempPath = "MatGallery\\Themes\\_temp\\";
        private readonly string _moduleSourcePath = "MatGallery\\Themes\\_source\\";
        private readonly string _modulePath = "MatGallery\\Themes\\";
        private readonly string[] _allowedExtentions = { ".zip" };
        //private readonly string[] _requiredDirAndFiles = { "bin", "Views", "wwwroot", "Module.json" };
        private string tempFullFilepath = "";

        private string unzipFolderName = "";
        private string unzipFolderPath = "";

        private string finalFolderName = "";
        private string finalFolderPath = "";

        private string themeFolderName = "";
        private string themeFolderPath = "";
        private string themeFolderSourcePath = "";

        private string jsonConfigFile = "";
        private string themeRootFolderPath = "";

        public MatgThemeController(ILoggerFactory factory, IHostingEnvironment env, NccSettingsService nccSettingsService, NccUserThemeService nccUserThemeService)
        {
            _logger = factory.CreateLogger<MatGalleryController>();
            _env = env;
            _nccUserThemeService = nccUserThemeService;

            try
            {
                _nccSettingsService = nccSettingsService;
                settings = new Settings();
                var tempSettings = _nccSettingsService.GetByKey("Ncc_MatGallery_Settings");
                if (tempSettings != null)
                {
                    settings = JsonConvert.DeserializeObject<Settings>(tempSettings.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        #endregion

        #region Admin Panel
        #region Upload operation
        [AdminMenuItem(Name = "Theme Upload", Url = "/MatgTheme/Upload", IconCls = "fa-upload", Order = 5)]
        public ActionResult Upload()
        {
            ViewBag.UploadPath = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            bool isSuccess = true;
            string responseMessage = "";
            NccUserTheme userTheme = new NccUserTheme();

            var tempUploads = Path.Combine(_env.WebRootPath, _tempPath);

            if (!Directory.Exists(tempUploads))
            {
                Directory.CreateDirectory(tempUploads);
            }

            if (file != null)
            {
                if (file.Length > 0 && (_allowedExtentions.Any(x => file.FileName.EndsWith(x, StringComparison.OrdinalIgnoreCase))))
                {
                    try
                    {
                        string fileNameWithoutEx = file.FileName.ToLower().Substring(0, file.FileName.LastIndexOf("."));
                        string fileExt = file.FileName.ToLower().Substring(file.FileName.LastIndexOf("."), 4);
                        string fileName = fileNameWithoutEx + "__" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        string fullFileName = fileName + fileExt;

                        tempFullFilepath = Path.Combine(tempUploads, fullFileName);
                        using (var fileStream = new FileStream(tempFullFilepath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        /*Start processing*/

                        #region Unzip in temp location
                        unzipFolderName = "\\" + fileName;
                        unzipFolderPath = Path.Combine(_env.WebRootPath, Path.Combine(_env.WebRootPath, _tempPath + unzipFolderName));
                        finalFolderName = "\\" + fileName + "_final";
                        finalFolderPath = Path.Combine(_env.WebRootPath, Path.Combine(_env.WebRootPath, _tempPath + finalFolderName));
                        if (!Directory.Exists(unzipFolderPath))
                        {
                            Directory.CreateDirectory(unzipFolderPath);
                            Directory.CreateDirectory(finalFolderPath);
                        }
                        else
                        {
                            isSuccess = false;
                            responseMessage = "Redundant ZipFile found in server.";
                            //stop the process, because duplicate name situation occered.
                        }
                        if (isSuccess)
                        {
                            ZipFile.ExtractToDirectory(tempFullFilepath, unzipFolderPath);
                        }
                        #endregion
                        #region Find config file
                        if (isSuccess)
                        {
                            try
                            {
                                string[] files = Directory.GetFiles(unzipFolderPath, "*.*", SearchOption.AllDirectories);
                                foreach (var f in files)
                                {
                                    if (f.EndsWith("Theme.json", StringComparison.OrdinalIgnoreCase))
                                    {
                                        jsonConfigFile = f;
                                    }
                                }
                            }
                            catch (System.Exception excpt)
                            {
                                _logger.LogError(excpt.Message);
                                isSuccess = false;
                                responseMessage += "Error reading zip.";
                            }
                        }
                        #endregion
                        #region Read config file
                        if (string.IsNullOrEmpty(jsonConfigFile) == false)
                        {
                            themeRootFolderPath = jsonConfigFile.Replace("\\Theme.json", "");
                            userTheme = JsonConvert.DeserializeObject<NccUserTheme>(System.IO.File.ReadAllText(jsonConfigFile));
                            userTheme.Name = userTheme.ThemeId;
                            themeFolderName = userTheme.ThemeId;
                            themeFolderPath = Path.Combine(_env.WebRootPath, _modulePath + "\\" + themeFolderName);
                            if (!Directory.Exists(themeFolderPath))
                            {
                                Directory.CreateDirectory(themeFolderPath);
                            }
                            themeFolderSourcePath = Path.Combine(_env.WebRootPath, _moduleSourcePath + "\\" + themeFolderName);
                            if (!Directory.Exists(themeFolderSourcePath))
                            {
                                Directory.CreateDirectory(themeFolderSourcePath);
                            }
                            System.IO.File.Move(jsonConfigFile, finalFolderPath + "\\Theme.json");
                        }
                        else
                        {
                            isSuccess = false;
                            responseMessage = "No <b>Theme.json</b> file found in your zip.";
                        }
                        #endregion
                        #region Load Previous Module
                        NccUserTheme oldUserTheme = _nccUserThemeService.Get(userTheme.ThemeId);
                        if (oldUserTheme != null)
                        {
                            //Check version 
                            var oldVersion = new Version(oldUserTheme.Version);
                            var newVersion = new Version(userTheme.Version);
                            if (newVersion.CompareTo(oldVersion) > 0)
                            {
                                userTheme.Id = oldUserTheme.Id;
                            }
                            else
                            {
                                isSuccess = false;
                                responseMessage = "Version " + userTheme.Version + " is backdated. An updated version for this theme is already uploaded in Gallery.";
                            }
                        }
                        #endregion
                        #region Check and Move all necessary files and folders in a temporary folder
                        if (isSuccess)
                        {
                            //bin
                            if (!Directory.Exists(themeRootFolderPath + "\\bin"))
                            {
                                isSuccess = false;
                                responseMessage = "<b>bin</b> folder not found";
                            }
                            else
                            {
                                Directory.Move(themeRootFolderPath + "\\bin", finalFolderPath + "\\bin");
                            }
                            //Preview
                            if (!Directory.Exists(themeRootFolderPath + "\\Preview"))
                            {
                                isSuccess = false;
                                responseMessage = "<b>Preview</b> folder not found";
                            }
                            else
                            {
                                Directory.Move(themeRootFolderPath + "\\Preview", finalFolderPath + "\\Preview");
                            }
                            //Shared
                            if (!Directory.Exists(themeRootFolderPath + "\\Shared"))
                            {
                                isSuccess = false;
                                responseMessage = "<b>Shared</b> folder not found";
                            }
                            else
                            {
                                Directory.Move(themeRootFolderPath + "\\Shared", finalFolderPath + "\\Shared");
                            }
                            //Views
                            if (!Directory.Exists(themeRootFolderPath + "\\Views"))
                            {
                                isSuccess = false;
                                responseMessage = "<b>Views</b> folder not found";
                            }
                            else
                            {
                                Directory.Move(themeRootFolderPath + "\\Views", finalFolderPath + "\\Views");
                            }
                            //wwwroot
                            if (!Directory.Exists(themeRootFolderPath + "\\wwwroot"))
                            {
                                isSuccess = false;
                                responseMessage = "<b>wwwroot</b> folder not found";
                            }
                            else
                            {
                                Directory.Move(themeRootFolderPath + "\\wwwroot", finalFolderPath + "\\wwwroot");
                            }
                        }
                        #endregion
                        #region Zip final module and move to module folder
                        if (isSuccess)
                        {
                            ZipFile.CreateFromDirectory(finalFolderPath, themeFolderPath + "\\" + themeFolderName + "_v" + userTheme.Version + ".zip");
                        }
                        #endregion
                        #region DB & DB Log save
                        if (isSuccess)
                        {
                            if (userTheme.Id > 0)
                            {
                                _nccUserThemeService.Update(userTheme);
                            }
                            else
                            {
                                _nccUserThemeService.Save(userTheme);
                            }
                        }
                        #endregion
                        #region Move the source file in sorce location AND Delete from temp folder   
                        //Thread.Sleep(5000);
                        if (isSuccess)
                        {
                            if (System.IO.File.Exists(tempFullFilepath))
                            {
                                System.IO.File.Move(tempFullFilepath, themeFolderSourcePath + "\\" + themeFolderName + "_v" + userTheme.Version + ".zip");
                            }
                        }
                        if (System.IO.File.Exists(tempFullFilepath))
                        {
                            System.IO.File.Delete(tempFullFilepath);
                        }
                        if (Directory.Exists(unzipFolderPath))
                        {
                            Directory.Delete(unzipFolderPath, true);
                        }
                        if (Directory.Exists(finalFolderPath))
                        {
                            Directory.Delete(finalFolderPath, true);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        responseMessage = ex.Message;
                    }
                }
                else
                {
                    isSuccess = false;
                    responseMessage = "Invalid file formate.";
                }
            }
            else
            {
                isSuccess = false;
                responseMessage = "No File Found.";
            }

            if (isSuccess)
                TempData["SuccessMessage"] = userTheme.ThemeId + "_v" + userTheme.Version + " Uploaded Successfully.";
            else
                TempData["ErrorMessage"] = responseMessage;

            return View();
        }
        #endregion        
        [AdminMenuItem(Name = "Theme Manage", Url = "/MatgTheme/Manage", IconCls = "fa-picture-o", Order = 6)]
        public ActionResult Manage()
        {
            var itemList = _nccUserThemeService.LoadAll().OrderByDescending(x => x.Id).ToList();
            return View(itemList);
        }

        public ActionResult StatusUpdate(long Id = 0)
        {
            if (Id > 0)
            {
                var item = _nccUserThemeService.Get(Id);
                if (item.Status == EntityStatus.Active)
                {
                    item.Status = EntityStatus.Inactive;
                    ViewBag.Message = "In-activated successfull.";
                }
                else
                {
                    item.Status = EntityStatus.Active;
                    ViewBag.Message = "Activated successfull.";
                }

                _nccUserThemeService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult PrivateStatusUpdate(long Id = 0)
        {
            if (Id > 0)
            {
                var item = _nccUserThemeService.Get(Id);
                if (item.IsPrivate == true)
                {
                    item.IsPrivate= false;
                    ViewBag.Message = "Module is Public.";
                }
                else
                {
                    item.IsPrivate = true;
                    ViewBag.Message = "Module is Private.";
                }

                _nccUserThemeService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Log(long id) //nccUserModuleId
        {
            var itemList = _nccUserThemeService.LoadLog(id).OrderByDescending(x => x.Id).ToList();
            return View(itemList);
        }
        #endregion

        #region User Panel
        #endregion
    }
}