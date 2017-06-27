/*
 * Author: TecRT
 * Website: http://tecrt.com
 * Copyright (c) tecrt.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        public CmsMediaController(ILoggerFactory factory, IHostingEnvironment env)
        {
            _logger = factory.CreateLogger<CmsMediaController>();
            _env = env;
        }
        #endregion


        #region Operations
        #region Manage Operation
        public ActionResult Index(string sq = "")
        {
            var dirFileList = new List<NccMediaViewModel>();
            if (sq == null) { sq = ""; }

            try
            {
                var webRoot = _env.WebRootPath;
                DirectoryInfo Dir = new DirectoryInfo(Path.Combine(webRoot + _imageRoot, sq));

                #region Back Link Generation
                if (Dir.Parent.FullName.StartsWith(webRoot + "\\media") && Dir.Parent.FullName.Length > (webRoot + "\\media").Length)
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
                        FullPath = di.FullName.Replace(webRoot + _imageRoot, ""),
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
        public ActionResult Upload(NccPage model, string PageContent, long ParentId, string SubmitType)
        {

            return View();
        }
        #endregion

        #region Delete Operation
        public ActionResult Delete(string fullPath)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(string fullPath, int status)
        {
            ViewBag.MessageType = "SuccessMessage";
            ViewBag.Message = "Page deleted successful";
            return RedirectToAction("Index");
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