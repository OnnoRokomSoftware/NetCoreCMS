/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Core.Modules.Media.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Mvc.Attributes;

namespace NetCoreCMS.Core.Modules.Media.Controllers
{
    [AdminMenu(Name = "Media", IconCls = "fa-folder-open-o", Order = 2)]
    public class MediaHomeController : NccController
    {
        #region Initialization

        private IHostingEnvironment _env;
        private readonly string _imageRoot = "\\media\\Images\\";
        private readonly string _imagePathPrefix = "/media/Images/";
        private readonly string _imageUploadPrefix = "media\\Images\\";
        string[] _allowedImageExtentions = { ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".ico", ".svg" };

        private readonly string _fileRoot = "\\media\\Files\\";
        private readonly string _filePathPrefix = "/media/Files/";
        private readonly string _fileUploadPrefix = "media\\Files\\";

        public MediaHomeController(ILoggerFactory factory, IHostingEnvironment env)
        {
            _logger = factory.CreateLogger<MediaHomeController>();
            _env = env;
        }
        #endregion

        #region Operations

        #region Upload operation
        [AdminMenuItem(Name = "Upload", Url = "/MediaHome/Upload", IconCls = "fa-upload", Order = 5)]
        public ActionResult Upload(bool isFile = false, string inputId = "")
        {
            ViewBag.IsFile = isFile;
            ViewBag.InputId = inputId;
            ViewBag.Layout = Constants.AdminLayoutName;
            if (!string.IsNullOrEmpty(inputId))
                ViewBag.Layout = "_SimpleFullWidthLayout";
            ViewBag.UploadPath = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files, bool isFile, string inputId = "")
        {
            ViewBag.InputId = inputId;
            ViewBag.Layout = Constants.AdminLayoutName;
            if (!string.IsNullOrEmpty(inputId))
                ViewBag.Layout = "_SimpleFullWidthLayout";
            string responseSuccess = "";
            string responseError = "";
            string uploadPath = _imageUploadPrefix + DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString());

            if (isFile)
            {
                uploadPath = _fileUploadPrefix + DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString());
            }
            var uploads = Path.Combine(_env.WebRootPath, uploadPath);

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            foreach (var file in files)
            {
                if (file.Length > 0 && (isFile || _allowedImageExtentions.Any(x => file.FileName.ToLower().EndsWith(x))))
                {
                    string fileName = file.FileName.ToLower().Substring(0, file.FileName.LastIndexOf("."));
                    string fileExt = file.FileName.ToLower().Substring(file.FileName.LastIndexOf("."), 4);
                    string fullFileName = fileName + "__" + DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;
                    using (var fileStream = new FileStream(Path.Combine(uploads, fullFileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        if (responseSuccess == "")
                        {
                            responseSuccess = "<strong>" + file.FileName + "</strong> URL: /" + uploadPath.Replace('\\', '/') + "/" + fullFileName;
                        }
                        else
                        {
                            responseSuccess += ",<br /><strong>" + file.FileName + "</strong> URL: /" + uploadPath.Replace('\\', '/') + "/" + fullFileName;
                        }
                    }
                }
                else
                {
                    if (responseError == "")
                    {
                        responseError = "<strong>" + file.FileName + "</strong>";
                    }
                    else
                    {
                        responseError += ",<br /><strong>" + file.FileName + "</strong>";
                    }
                }
            }

            if (responseSuccess != "")
                TempData["SuccessMessage"] = responseSuccess + "<br /><b>Upload successfully.</b>";
            if (responseError != "")
                TempData["ErrorMessage"] = responseError + " <br /><b>Invalid formate.</b>";


            ViewBag.IsFile = isFile;
            string fileRoot = _imageUploadPrefix;
            if (isFile)
            {
                fileRoot = _fileUploadPrefix;
            }
            ViewBag.UploadPath = uploadPath.Replace(fileRoot, "");
            return View();
        }
        #endregion

        #region Manage Operation
        [AdminMenuItem(Name = "Manage Gallary", Url = "/MediaHome/Index", IconCls = "fa-image", SubActions = new string[] { "Delete" }, Order = 1)]
        public ActionResult Index(bool isFile = false, string inputId = "", string sq = "", string successMessage = "", string errorMessage = "")
        {
            ViewBag.IsFile = isFile;
            ViewBag.InputId = inputId;
            ViewBag.Layout = Constants.AdminLayoutName;
            if (!string.IsNullOrEmpty(inputId))
                ViewBag.Layout = "_SimpleFullWidthLayout";

            var dirFileList = new List<NccMediaViewModel>();
            if (successMessage.Trim() != "")
                TempData["SuccessMessage"] = successMessage;
            if (errorMessage.Trim() != "")
                TempData["ErrorMessage"] = errorMessage;
            if (sq == null) { sq = ""; }

            string fileRoot = _env.WebRootPath + _imageRoot;
            if (isFile)
            {
                fileRoot = _env.WebRootPath + _fileRoot;
            }
            try
            {
                DirectoryInfo Dir = new DirectoryInfo(Path.Combine(fileRoot, sq));

                #region Back Link Generation
                if (Dir.Parent.FullName.StartsWith(_env.WebRootPath + "\\media") && Dir.Parent.FullName.Length > (_env.WebRootPath + "\\media").Length)
                {
                    dirFileList.Add(new NccMediaViewModel
                    {
                        FileName = "Up",
                        FullPath = Dir.Parent.FullName,
                        IsDir = true,
                        CreationTime = Dir.CreationTime
                    });
                }
                #endregion

                #region Directory Listing
                DirectoryInfo[] DirList = Dir.GetDirectories("*", SearchOption.TopDirectoryOnly);
                foreach (DirectoryInfo di in DirList)
                {
                    DirectoryInfo SubDir = new DirectoryInfo(Path.Combine(di.FullName));
                    int SubDirCount = SubDir.GetDirectories("*", SearchOption.TopDirectoryOnly).Count();

                    int ImageCount = SubDir.GetFiles("*.*", SearchOption.AllDirectories).Count();
                    if (isFile == false)
                    {
                        ImageCount = SubDir.GetFiles("*.*", SearchOption.AllDirectories)
                                         .Where(f => _allowedImageExtentions.Any(x => f.Name.ToLower().EndsWith(x)))
                                         .Count();
                    }

                    dirFileList.Add(new NccMediaViewModel
                    {
                        FileName = di.Name,
                        FullPath = di.FullName.Replace(fileRoot, ""),
                        TotalSubDir = SubDirCount,
                        TotalFile = ImageCount,
                        IsDir = true,
                        CreationTime = di.CreationTime
                    });
                }
                #endregion

                #region File Listing
                FileInfo[] FileList = Dir.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                                         .Where(f => _allowedImageExtentions.Any(x => f.Name.ToLower().EndsWith(x)))
                                         .ToArray();
                if (isFile)
                {
                    FileList = Dir.GetFiles("*.*", SearchOption.TopDirectoryOnly).ToArray();
                }

                foreach (FileInfo fi in FileList)
                {
                    dirFileList.Add(new NccMediaViewModel
                    {
                        FileName = fi.Name,
                        FullPath = ((isFile == false ? _imageRoot : _fileRoot) + sq).Replace("\\", "/") + "/" + fi.Name,
                        ParrentDir = fi.Directory.FullName.Replace(fileRoot, ""),
                        ItemSize = BytesToString(fi.Length),
                        IsDir = false,
                        IsImage = !isFile,
                        CreationTime = fi.CreationTime
                    });
                }
                #endregion
            }
            catch (Exception ex)
            {
                ViewBag.MessageType = "ErrorMessage";
                ViewBag.Message = ex.Message;
            }
            return View(dirFileList);
        }

        [AdminMenuItem(Name = "Manage Files", Url = "/MediaHome/ManageFiles", IconCls = "fa-files-o", Order = 3)]
        public ActionResult ManageFiles()
        {
            return RedirectToAction("Index", "MediaHome", new { isFile = true });
        }
        #endregion

        #region Delete Operation
        public ActionResult Delete(string fullPath, string parrentDir, bool isFile = false, string inputId = "")
        {
            ViewBag.InputId = inputId;
            ViewBag.Layout = Constants.AdminLayoutName;
            if (!string.IsNullOrEmpty(inputId))
                ViewBag.Layout = "_SimpleFullWidthLayout";
            ViewBag.IsFile = isFile;
            var file = new NccMediaViewModel
            {
                FileName = fullPath.Substring(fullPath.LastIndexOf("\\") + 1),
                FullPath = fullPath,
                ParrentDir = parrentDir
            };
            string deletePath = Path.Combine(_env.WebRootPath, fullPath.Substring(1));
            //isFile.FullPath = deletePath;
            if (!System.IO.File.Exists(deletePath))
            {
                //ViewBag.MessageType = "ErrorMessage";
                //ViewBag.Message = "File does not exists";
                return RedirectToAction("Index", new { sq = parrentDir, errorMessage = "File does not exists.", isFile = isFile, inputId = inputId });
            }
            return View(file);
        }

        [HttpPost]
        public ActionResult Delete(string fullPath, string parrentDir, int status, bool isFile, string inputId = "")
        {
            ViewBag.InputId = inputId;
            ViewBag.Layout = Constants.AdminLayoutName;
            if (!string.IsNullOrEmpty(inputId))
                ViewBag.Layout = "_SimpleFullWidthLayout";
            ViewBag.IsFile = isFile;
            string deletePath = Path.Combine(_env.WebRootPath, fullPath.Substring(1));
            if (System.IO.File.Exists(deletePath))
            {
                System.IO.File.Delete(deletePath);
            }
            //ViewBag.MessageType = "SuccessMessage";
            //ViewBag.Message = "File deleted successful";
            return RedirectToAction("Index", new { sq = parrentDir, successMessage = "File deleted successfully.", isFile = isFile, inputId = inputId });
        }
        #endregion
        #endregion

        #region Helper
        private string BytesToString(long len)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }
        #endregion
    }
}