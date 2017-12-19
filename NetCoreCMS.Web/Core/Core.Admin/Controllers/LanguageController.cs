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
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Security;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using Core.Admin.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Core.Admin.Controllers
{
    [AdminMenu(Name ="Settings", Order = 7)]
    public class LanguageController : NccController
    {
        public ActionResult RedirectToDefaultLanguage()
        {
            var lang = CurrentLanguage;
            var redirectUrl = Request.Path.Value;
            if (Request.Path.Value.StartsWith("/en") || Request.Path.Value.StartsWith("/bn"))
            {
                return Redirect("~" + redirectUrl);
            }
            return Redirect("~/" + lang + Request.Path.Value);
        }

        [AdminMenuItem(Name ="Translation", Order = 200, IconCls ="fa fa-language", SubActions = new[] { "EditTranslationFile"})]
        public ActionResult TranslationFiles()
        {
            var resourceFileList = LoadTranslationFiles();
            return View(resourceFileList);
        }
          
        public ActionResult EditTranslationFile(string id)
        {
            var translationFiles = LoadTranslationFiles();
            var selectedTranslationFile = translationFiles.Where(x => x.Id == id).FirstOrDefault();

            if(selectedTranslationFile == null)
            {
                ViewBag.ErrorMessage = "File not found";
                return View(new TranslationFile());
            }

            ViewBag.TranslationFile = selectedTranslationFile;

            return View(selectedTranslationFile);
        }

        [HttpPost]
        public JsonResult EditTranslationFile(string id, string contentJson)
        {
            var translationFiles = LoadTranslationFiles();
            var selectedTranslationFile = translationFiles.Where(x => x.Id == id).FirstOrDefault();

            if (selectedTranslationFile == null)
            {
                ViewBag.ErrorMessage = "File not found";
                return Json(new ApiResponse() {
                     IsSuccess = false,
                     Message = "Translation file not found"
                });
            }
            else
            {
                selectedTranslationFile.Content = contentJson;
                selectedTranslationFile.Save();

                return Json(new ApiResponse()
                {
                    IsSuccess = true,
                    Message = "Update successful"
                });
            }
        }        

        #region Private Methods
        private List<TranslationFile> LoadTranslationFiles()
        {
            var resourceFileList = new List<TranslationFile>();
            var siteResources = GetSiteResources();
            var moduleResources = GetModuleResources();
            var themeResources = GetThemeResources();

            resourceFileList.AddRange(siteResources);
            resourceFileList.AddRange(moduleResources);
            resourceFileList.AddRange(themeResources);
            return resourceFileList;
        }

        private List<TranslationFile> GetThemeResources()
        {
            var translationFileList = new List<TranslationFile>();
            var activeTheme = ThemeHelper.ActiveTheme;
            translationFileList = GetTranslationFiles(activeTheme.ThemeName, activeTheme.ResourceFolder);
            return translationFileList;
        }

        private List<TranslationFile> GetModuleResources()
        {
            var list = new List<TranslationFile>();
            foreach (var item in GlobalContext.GetActiveModules())
            {
                var resourceFolder = "\\Bin\\Release\\netcoreapp2.0\\Resources";
                if (GlobalContext.HostingEnvironment.EnvironmentName.Equals("Development"))
                {
                    resourceFolder = "\\Bin\\Debug\\netcoreapp2.0\\Resources";
                }
                var tfList = GetTranslationFiles(item.ModuleTitle, item.Path + resourceFolder);
                list.AddRange(tfList);
            }
            return list;
        }

        private List<TranslationFile> GetSiteResources()
        {
            var resourceFolder = GlobalContext.ContentRootPath + "\\Bin\\Release\\netcoreapp2.0\\Resources";
            if (GlobalContext.HostingEnvironment.EnvironmentName.Equals("Development"))
            {
                resourceFolder = GlobalContext.ContentRootPath + "\\Bin\\Debug\\netcoreapp2.0\\Resources";
            }
            var translationFileList = GetTranslationFiles("Website",resourceFolder);
            return translationFileList;
        }

        private List<TranslationFile> GetTranslationFiles(string group, string resourceFolder)
        {
            var translationFileList = new List<TranslationFile>();
            if (Directory.Exists(resourceFolder))
            {
                var files = Directory.GetFiles(resourceFolder, "*.lang");
                foreach (var item in files)
                {
                    var fi = new FileInfo(item);
                    var tf = new TranslationFile();
                    tf.Content = System.IO.File.ReadAllText(item);
                    tf.FileName = fi.Name;
                    tf.FullPath = item;
                    tf.Group = group;
                    tf.Culture = TranslationFile.GetCultureFromFileName(fi.Name);
                    tf.Id = Cryptography.GetHash(item);
                    translationFileList.Add(tf);
                }
            }
            
            return translationFileList;
        }
        
        #endregion

    }
}
