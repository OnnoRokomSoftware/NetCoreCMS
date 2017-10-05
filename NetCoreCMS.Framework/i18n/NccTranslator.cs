using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using NetCoreCMS.Framework.Core.Exceptions;
using System.Collections.Generic;
using System;
using NetCoreCMS.Framework.Setup;

namespace NetCoreCMS.Framework.i18n
{
    public sealed class NccTranslator<T>
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
            GetTranslator(typeof(T), cultureCode);
        }

        public NccTranslator<T> GetTranslator(Type resourceType, string cultureCode) {
            
            _cultureCode = cultureCode;

            if (string.IsNullOrEmpty(cultureCode) || cultureCode.ToLower().Equals("en"))
                return this;

            var assembly = resourceType.Assembly;
            var fileInfo = new FileInfo(assembly.Location);            
            var path = Path.Combine(fileInfo.DirectoryName, "Resources");

            _fileName = resourceType.Namespace+"."+resourceType.Name+ "."+cultureCode+".lang";
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
            if (string.IsNullOrEmpty(translationFileData))
            {
                translationFileData = JsonConvert.SerializeObject(new NccTranslationFile() {
                    Translations = new Dictionary<string, string>() { { "FileName", _fileName }, { "Type", resourceType.Name } }
                }, Formatting.Indented );
                File.WriteAllText(_resourceFilePath, translationFileData);
            }
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
