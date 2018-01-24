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
using NetCoreCMS.Framework.Core.Security;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Framework.i18n
{
    public class TranslationFile
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public string Content { get; set; }
        public string Culture { get; set; }
        public string Group { get; set; }

        public void Save()
        {
            if (File.Exists(FullPath))
            {
                NccFileHelper.WriteAllText(FullPath, Content);
            }
        }

        public string Load()
        {
            if (File.Exists(FullPath))
            {
                var fi = new FileInfo(FullPath);
                FileName = fi.Name;
                Culture = GetCultureFromFileName(FileName);
                Content = NccFileHelper.ReadAllText(FullPath);
                Id = Cryptography.GetHash(FullPath);
            }
            return Content;
        }

        public static string GetCultureFromFileName(string name)
        {
            var nameParts = name.Split(".".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            if (nameParts.Length >= 2)
            {
                return nameParts[nameParts.Length - 2];
            }
            return "-";
        }

        public static FileInfo GetTranslationFilePath(string cultureCode)
        {
            var path = Path.Combine(GlobalContext.GetResourceFolder(), "Language");            
            var fileName = "NccTranslationFile." + cultureCode + ".lang";
            return new FileInfo(Path.Combine(path, fileName));
        }

        public static List<TranslationFile> GetTranslationFiles()
        {
            var translationFileList = new List<TranslationFile>();
            var languageFolder = Path.Combine(GlobalContext.GetResourceFolder(), "Language");

            if (Directory.Exists(languageFolder))
            {
                var files = Directory.GetFiles(languageFolder, "*.lang");
                foreach (var item in files)
                {
                    var fi = new FileInfo(item);
                    var tf = new TranslationFile();
                    tf.Content = NccFileHelper.ReadAllText(item);
                    tf.FileName = fi.Name;
                    tf.FullPath = item;
                    tf.Group = "Website";
                    tf.Culture = GetCultureFromFileName(fi.Name);
                    tf.Id = Cryptography.GetHash(item);
                    translationFileList.Add(tf);
                }
            }

            return translationFileList;
        }
    }
}
