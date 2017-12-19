/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Core.Media.Services
{
    public class MediaServices
    {
        #region Initialization
        private readonly ILogger _logger;
        private IHostingEnvironment _env;

        public MediaServices(ILoggerFactory factory, IHostingEnvironment env)
        {
            _logger = factory.CreateLogger<MediaServices>();
            _env = env;
        }

        private readonly string _imageRoot = "\\media\\Images\\";
        private readonly string _imagePathPrefix = "/media/Images/";
        private readonly string _imageUploadPrefix = "media\\Images\\";
        private string[] _allowedImageExtentions = { ".jpg", ".jpeg", ".bmp", ".png", ".gif", };
        #endregion

        public string SaveImage(IFormFile file)
        {
            string response = "";
            string uploadPath = _imageUploadPrefix + DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString());

            var uploads = Path.Combine(_env.WebRootPath, uploadPath);

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            if (file.Length > 0 && _allowedImageExtentions.Any(x => file.FileName.ToLower().EndsWith(x)))
            {
                string fileName = file.FileName.ToLower().Substring(0, file.FileName.LastIndexOf("."));
                string fileExt = file.FileName.ToLower().Substring(file.FileName.LastIndexOf("."), 4);
                string fullFileName = fileName + "__" + DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;
                using (var fileStream = new FileStream(Path.Combine(uploads, fullFileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    response = uploadPath.Replace('\\', '/') + "/" + fullFileName;

                }
            }
            else
            {
                response = "Invalid File";
            }

            return response;
        }
    }
}
