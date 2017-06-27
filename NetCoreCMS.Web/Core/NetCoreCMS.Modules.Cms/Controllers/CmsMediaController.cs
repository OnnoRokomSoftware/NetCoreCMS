/*
 * Author: TecRT
 * Website: http://tecrt.com
 * Copyright (c) tecrt.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Cms.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.Core.Modules.Cms.Controllers
{
    [Authorize]
    public class CmsMediaController : Controller
    {
        #region Initialization
        private readonly ILogger _logger;
        private IHostingEnvironment _env;
        private readonly string _imageRoot = "\\media\\Images\\";
        private readonly string _imagePathPrefix = "/media/Images/";
        private readonly string _imageUploadPrefix = "media\\Images\\";
        string[] _allowedImageExtentions = { ".jpg", ".jpeg", ".bmp", ".png", ".gif", };
        public CmsMediaController(ILoggerFactory factory, IHostingEnvironment env)
        {
            _logger = factory.CreateLogger<CmsMediaController>();
            _env = env;
        }
        #endregion


        #region Operations
        #region Manage Operation
        public ActionResult Index(string sq = "", string successMessage = "", string errorMessage = "")
        {
            var dirFileList = new List<NccMediaViewModel>();
            if (successMessage.Trim() != "")
                TempData["SuccessMessage"] = successMessage;
            if (errorMessage.Trim() != "")
                TempData["ErrorMessage"] = errorMessage;
            if (sq == null) { sq = ""; }

            try
            {
                DirectoryInfo Dir = new DirectoryInfo(Path.Combine(_env.WebRootPath + _imageRoot, sq));

                #region Back Link Generation
                if (Dir.Parent.FullName.StartsWith(_env.WebRootPath + "\\media") && Dir.Parent.FullName.Length > (_env.WebRootPath + "\\media").Length)
                {
                    dirFileList.Add(new NccMediaViewModel
                    {
                        FileName = "Back",
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

                    int ImageCount = SubDir.GetFiles("*.*", SearchOption.AllDirectories)
                                                 .Where(f => f.Name.EndsWith(".jpg") == true ||
                                                    f.Name.EndsWith(".jpeg") == true ||
                                                    f.Name.EndsWith(".bmp") == true ||
                                                    f.Name.EndsWith(".png") == true ||
                                                    f.Name.EndsWith(".gif") == true)
                                                 .Count();

                    dirFileList.Add(new NccMediaViewModel
                    {
                        FileName = di.Name,
                        FullPath = di.FullName.Replace(_env.WebRootPath + _imageRoot, ""),
                        TotalSubDir = SubDirCount,
                        TotalFile = ImageCount,
                        IsDir = true,
                        CreationTime = di.CreationTime
                    });
                }
                #endregion

                #region File Listing
                FileInfo[] FileList = Dir.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                                         .Where(f => f.Name.EndsWith(".jpg") == true ||
                                            f.Name.EndsWith(".jpeg") == true ||
                                            f.Name.EndsWith(".bmp") == true ||
                                            f.Name.EndsWith(".png") == true ||
                                            f.Name.EndsWith(".gif") == true)
                                         .ToArray();
                foreach (FileInfo fi in FileList)
                {
                    dirFileList.Add(new NccMediaViewModel
                    {
                        FileName = fi.Name,
                        FullPath = _imagePathPrefix + sq.Replace("\\", "/") + "/" + fi.Name,
                        ParrentDir = fi.Directory.FullName.Replace(_env.WebRootPath + _imageRoot, ""),
                        ItemSize = BytesToString(fi.Length),
                        IsDir = false,
                        IsImage = true,
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
        #endregion

        #region Upload operation
        public ActionResult Upload()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        {
            string responseSuccess = "";
            string responseError = "";
            string uploadPath = _imageUploadPrefix + DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString());
            var uploads = Path.Combine(_env.WebRootPath, uploadPath);

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            foreach (var file in files)
            {
                if (file.Length > 0 && _allowedImageExtentions.Any(x => file.FileName.ToLower().EndsWith(x)))
                {
                    string fileName = file.FileName.ToLower().Substring(0, file.FileName.LastIndexOf("."));
                    string fileExt = file.FileName.ToLower().Substring(file.FileName.LastIndexOf("."), 4);
                    string fullFileName = fileName + "__" + DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;
                    using (var fileStream = new FileStream(Path.Combine(uploads, fullFileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        if (responseSuccess == "")
                        {
                            responseSuccess = "<strong>" + file.FileName + "</strong>";
                        }
                        else
                        {
                            responseSuccess += ",<br /><strong>" + file.FileName + "</strong>";
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
                responseSuccess += " uploaded successfully.";
            if (responseError != "")
                responseError += " invalid image formate.";

            TempData["SuccessMessage"] = responseSuccess;
            TempData["ErrorMessage"] = responseError;

            return View();
        }
        #endregion

        #region Delete Operation
        public ActionResult Delete(string fullPath, string parrentDir)
        {
            var isFile = new NccMediaViewModel
            {
                FileName = fullPath.Substring(fullPath.LastIndexOf("\\") + 1),
                FullPath = fullPath,
                ParrentDir = parrentDir
            };
            string deletePath = Path.Combine(_env.WebRootPath , fullPath.Substring(1));
            //isFile.FullPath = deletePath;
            if (!System.IO.File.Exists(deletePath))
            {
                //ViewBag.MessageType = "ErrorMessage";
                //ViewBag.Message = "File does not exists";
                return RedirectToAction("Index", new { sq = parrentDir, errorMessage = "File does not exists." });
            }
            return View(isFile);
        }

        [HttpPost]
        public ActionResult Delete(string fullPath, string parrentDir, int status)
        {
            string deletePath = Path.Combine(_env.WebRootPath, fullPath.Substring(1));
            if (System.IO.File.Exists(deletePath))
            {
                System.IO.File.Delete(deletePath);
            }
            //ViewBag.MessageType = "SuccessMessage";
            //ViewBag.Message = "File deleted successful";
            return RedirectToAction("Index", new { sq = parrentDir, successMessage = "File deleted successfully." });
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