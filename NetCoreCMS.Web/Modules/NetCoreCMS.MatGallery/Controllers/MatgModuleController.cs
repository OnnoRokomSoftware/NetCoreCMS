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
    public class MatgModuleController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;
        private NccUserModuleService _nccUserModuleService;
        private Settings settings;

        private IHostingEnvironment _env;
        private readonly string _tempPath = "MatGallery\\Modules\\_temp\\";
        private readonly string _moduleSourcePath = "MatGallery\\Modules\\_source\\";
        private readonly string _modulePath = "MatGallery\\Modules\\";
        private readonly string[] _allowedExtentions = { ".zip" };
        //private readonly string[] _requiredDirAndFiles = { "bin", "Views", "wwwroot", "Module.json" };
        private string tempFullFilepath = "";

        private string unzipFolderName = "";
        private string unzipFolderPath = "";

        private string finalFolderName = "";
        private string finalFolderPath = "";

        private string moduleFolderName = "";
        private string moduleFolderPath = "";
        private string moduleFolderSourcePath = "";

        private string jsonConfigFile = "";
        private string moduleRootFolderPath = "";

        public MatgModuleController(ILoggerFactory factory, IHostingEnvironment env, NccSettingsService nccSettingsService, NccUserModuleService nccUserModuleService)
        {
            _logger = factory.CreateLogger<MatGalleryController>();
            _env = env;
            _nccUserModuleService = nccUserModuleService;

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
        [AdminMenuItem(Name = "Module Upload", Url = "/MatgModule/Upload", IconCls = "fa-upload", Order = 1)]
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
            NccUserModule userModule = new NccUserModule();

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
                                    if (f.EndsWith("Module.json", StringComparison.OrdinalIgnoreCase))
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
                            moduleRootFolderPath = jsonConfigFile.Replace("\\Module.json", "");
                            userModule = JsonConvert.DeserializeObject<NccUserModule>(System.IO.File.ReadAllText(jsonConfigFile));
                            userModule.Name = userModule.ModuleId;
                            moduleFolderName = userModule.ModuleId;
                            moduleFolderPath = Path.Combine(_env.WebRootPath, _modulePath + "\\" + moduleFolderName);
                            if (!Directory.Exists(moduleFolderPath))
                            {
                                Directory.CreateDirectory(moduleFolderPath);
                            }
                            moduleFolderSourcePath = Path.Combine(_env.WebRootPath, _moduleSourcePath + "\\" + moduleFolderName);
                            if (!Directory.Exists(moduleFolderSourcePath))
                            {
                                Directory.CreateDirectory(moduleFolderSourcePath);
                            }
                            System.IO.File.Move(jsonConfigFile, finalFolderPath + "\\Module.json");
                        }
                        else
                        {
                            isSuccess = false;
                            responseMessage = "No <b>Module.json</b> file found in your zip.";
                        }
                        #endregion
                        #region Load Previous Module
                        NccUserModule oldUserModule = _nccUserModuleService.Get(userModule.ModuleId);
                        if (oldUserModule != null)
                        {
                            //Check version 
                            var oldVersion = new Version(oldUserModule.Version);
                            var newVersion = new Version(userModule.Version);
                            if (newVersion.CompareTo(oldVersion) > 0)
                            {
                                userModule.Id = oldUserModule.Id;
                            }
                            else
                            {
                                isSuccess = false;
                                responseMessage = "Version " + userModule.Version + " is backdated. An updated version for this module is already uploaded in Gallery.";
                            }
                        }
                        #endregion
                        #region Check and Move all necessary files and folders in a temporary folder
                        if (isSuccess)
                        {
                            //bin
                            if (!Directory.Exists(moduleRootFolderPath + "\\bin"))
                            {
                                isSuccess = false;
                                responseMessage = "<b>bin</b> folder not found";
                            }
                            else
                            {
                                Directory.Move(moduleRootFolderPath + "\\bin", finalFolderPath + "\\bin");
                            }
                            //Views
                            if (!Directory.Exists(moduleRootFolderPath + "\\Views"))
                            {
                                isSuccess = false;
                                responseMessage = "<b>Views</b> folder not found";
                            }
                            else
                            {
                                Directory.Move(moduleRootFolderPath + "\\Views", finalFolderPath + "\\Views");
                            }
                            //wwwroot
                            if (!Directory.Exists(moduleRootFolderPath + "\\wwwroot"))
                            {
                                isSuccess = false;
                                responseMessage = "<b>wwwroot</b> folder not found";
                            }
                            else
                            {
                                Directory.Move(moduleRootFolderPath + "\\wwwroot", finalFolderPath + "\\wwwroot");
                            }
                        }
                        #endregion
                        #region Zip final module and move to module folder
                        if (isSuccess)
                        {
                            ZipFile.CreateFromDirectory(finalFolderPath, moduleFolderPath + "\\" + moduleFolderName + "_v" + userModule.Version + ".zip");
                        }
                        #endregion
                        #region DB & DB Log save
                        if (isSuccess)
                        {
                            if (userModule.Id > 0)
                            {
                                _nccUserModuleService.Update(userModule);
                            }
                            else
                            {
                                _nccUserModuleService.Save(userModule);
                            }
                        }
                        #endregion
                        #region Move the source file in sorce location AND Delete from temp folder   
                        //Thread.Sleep(5000);
                        if (isSuccess)
                        {
                            if (System.IO.File.Exists(tempFullFilepath))
                            {
                                System.IO.File.Move(tempFullFilepath, moduleFolderSourcePath + "\\" + moduleFolderName + "_v" + userModule.Version + ".zip");
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
                TempData["SuccessMessage"] = userModule.ModuleId + "_v" + userModule.Version + " Uploaded Successfully.";
            else
                TempData["ErrorMessage"] = responseMessage;

            return View();
        }
        #endregion        
        [AdminMenuItem(Name = "Module Manage", Url = "/MatgModule/Manage", IconCls = "", Order = 2)]
        public ActionResult Manage()
        {
            var itemList = _nccUserModuleService.LoadAll().OrderByDescending(x => x.Id).ToList();
            return View(itemList);
        }

        public ActionResult StatusUpdate(long Id = 0)
        {
            if (Id > 0)
            {
                var item = _nccUserModuleService.Get(Id);
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

                _nccUserModuleService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult PrivateStatusUpdate(long Id = 0)
        {
            if (Id > 0)
            {
                var item = _nccUserModuleService.Get(Id);
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

                _nccUserModuleService.Update(item);
                ViewBag.MessageType = "SuccessMessage";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Log(long id) //nccUserModuleId
        {
            var itemList = _nccUserModuleService.LoadLog(id).OrderByDescending(x => x.Id).ToList();
            return View(itemList);
        }
        #endregion

        #region User Panel
        #endregion
    }
}