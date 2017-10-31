/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core;
using System.Reflection;
using NetCoreCMS.Framework.Resources;

namespace NetCoreCMS.Framework.i18n
{
    public sealed class NccTranslator : INccTranslator
    {
        private string _fileName;
        private string _resourceFilePath;
        private string _cultureCode;
        private NccTranslationFile _translationFile;
        public string CultureCode { get { return _cultureCode; } }

        public string this[string key]
        {
            set {Set(key,value);}
            get{return Get(key);}
        }

        private NccTranslator() {}
        
        public NccTranslator(string cultureCode)
        {
            GetTranslator(cultureCode);         
        }

        public INccTranslator GetTranslator(string cultureCode) {
            
            _cultureCode = cultureCode;

            if (string.IsNullOrEmpty(cultureCode) || cultureCode.ToLower().Equals("en"))
                return this;

            var path = Path.Combine(GlobalContext.ContentRootPath, "bin", "Debug", "netcoreapp2.0", "Resources");

            if (RuntimeUtil.IsRelease(typeof(SharedResource).Assembly))
            {
                path = Path.Combine(GlobalContext.ContentRootPath, "bin", "Release", "netcoreapp2.0", "Resources");
            }
            
            _fileName = "NccTranslationFile."+cultureCode+".lang";
            _resourceFilePath = Path.Combine(path, _fileName);

            if (!File.Exists(_resourceFilePath))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.Create(_resourceFilePath).Dispose();
            }

            var translationFileData = File.ReadAllText(_resourceFilePath);
            if (string.IsNullOrWhiteSpace(translationFileData))
            {
                var assembly = typeof(SharedResource).GetTypeInfo().Assembly;
                using (Stream resource = assembly.GetManifestResourceStream("NetCoreCMS.Framework.Resources.SharedResource.lang")) {
                    var sr = new StreamReader(resource);
                    translationFileData = sr.ReadToEndAsync().Result;                   
                }
            }

            File.WriteAllText(_resourceFilePath, translationFileData);
            _translationFile = JsonConvert.DeserializeObject<NccTranslationFile>(translationFileData);

            return this;
        }

        public void Save()
        {
            OrderTranslations();
            var fileData = JsonConvert.SerializeObject(_translationFile, Formatting.Indented);

            using (var resFile = File.Create(_resourceFilePath))
            {
                var data = Encoding.UTF8.GetBytes(fileData);
                resFile.Write(data, 0, data.Length);
                resFile.Flush();
            }   
        }

        private void OrderTranslations()
        {
            var dic = new Dictionary<string, string>();
            var orderedList = _translationFile.Translations.OrderBy(x => x.Key).ToList();
            foreach (var item in orderedList)
            {
                dic.Add(item.Key, item.Value);
            }
            _translationFile.Translations = dic;
        }

        public void Set(string key, string value)
        {
            var tKey = key.ToLower();
            if (_cultureCode.Equals("en"))
            {
                return;
            }
            if (_translationFile.Translations.Keys.Contains(tKey))
            {
                _translationFile.Translations[tKey] = value;
            }

            _translationFile.Translations.Add(tKey, value);
        }

        public string Get(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key) || _cultureCode.Equals("en"))
                {
                    return key;
                }

                var tKey = key.ToLower();

                if (!_translationFile.Translations.Keys.Contains(tKey))
                {
                    Set(tKey, key);
                    Save();
                    return key;
                }
                return _translationFile.Translations.GetValueOrDefault(tKey);
            }
            catch (Exception ex)
            {
                return key;
            }
        }

        public Dictionary<string,string> LoadAll()
        {
            return _translationFile.Translations;
        }
    }
}
