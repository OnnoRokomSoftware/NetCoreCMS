/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

using NetCoreCMS.Framework.Utility;
using System.Reflection;
using NetCoreCMS.Framework.Resources;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace NetCoreCMS.Framework.i18n
{
    public sealed class NccTranslator : INccTranslator
    {
        private string _fileName;
        private string _resourceFilePath;
        private string _cultureCode;
        private volatile static ConcurrentDictionary<string, TranslationFileData> _translationFile;

        public string CultureCode { get { return _cultureCode; } }

        public volatile static object _lockObject = new object();

        public string this[string key]
        {
            set { Set(key, value); }
            get { return Get(key); }
        }

        private NccTranslator()
        {

            if (_translationFile == null)
            {
                _translationFile = new ConcurrentDictionary<string, TranslationFileData>();
                foreach (var culture in SupportedCultures.Cultures)
                {
                    LoadTranslationFile(culture.TwoLetterISOLanguageName);
                }
            }
            //else
            //{
            //    foreach (var culture in SupportedCultures.Cultures)
            //    {
            //        if(_translationFile.ContainsKey(
            //            culture.TwoLetterISOLanguageName) == false 
            //            || _translationFile[culture.TwoLetterISOLanguageName].Translations == null 
            //            || _translationFile[culture.TwoLetterISOLanguageName].Translations.Count == 0)
            //        {
            //            LoadTranslationFile(culture.TwoLetterISOLanguageName);
            //        }   
            //    }
            //}
        }

        public NccTranslator(string cultureCode) : this()
        {
            GetTranslator(cultureCode);
        }

        public INccTranslator GetTranslator(string cultureCode)
        {

            _cultureCode = cultureCode;

            if (string.IsNullOrEmpty(cultureCode) || cultureCode.ToLower().Equals("en"))
                return this;

            if (_translationFile[_cultureCode] == null)
            {
                LoadTranslationFile(_cultureCode);
            }

            return this;
        }

        public void SaveTranslations()
        {
            OrderTranslations();
            //var fileData = JsonConvert.SerializeObject(_translationFile[_cultureCode], Formatting.Indented);

            lock (_lockObject)
            {
                //using (var resFile = File.Create(_resourceFilePath))
                //{
                //    var data = Encoding.UTF8.GetBytes(fileData);
                //    resFile.Write(data, 0, data.Length);
                //    resFile.Flush();
                //}

                foreach (var item in SupportedCultures.Cultures)
                {
                    if (item.TwoLetterISOLanguageName == "en")
                    {
                        continue;
                    }

                    var file = TranslationFile.GetTranslationFilePath(item.TwoLetterISOLanguageName);
                    NccFileHelper.WriteObject(file.FullName, _translationFile[item.TwoLetterISOLanguageName]);
                }
            }
        }

        private void OrderTranslations()
        {
            foreach (var culture in SupportedCultures.Cultures)
            {
                var cultureCode = culture.TwoLetterISOLanguageName;
                if (cultureCode == "en")
                {
                    continue;
                }

                var dic = new Dictionary<string, string>();
                if (_translationFile[cultureCode] != null && _translationFile[cultureCode].Translations != null)
                {
                    var orderedList = _translationFile[cultureCode].Translations.OrderBy(x => x.Key).ToList();
                    foreach (var item in orderedList)
                    {
                        dic.Add(item.Key, item.Value);
                    }
                    _translationFile[cultureCode].Translations = dic;
                }
            }
        }

        public void Set(string key, string value)
        {
            var tKey = key.ToLower();
            if (_cultureCode.Equals("en"))
            {
                return;
            }

            if (_translationFile[_cultureCode].Translations.Keys.Contains(tKey))
            {
                _translationFile[_cultureCode].Translations[tKey] = value;
            }

            _translationFile[_cultureCode].Translations.Add(tKey, value);
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

                if (!_translationFile[_cultureCode].Translations.Keys.Contains(tKey))
                {
                    Set(tKey, key);
                    return key;
                }
                return _translationFile[_cultureCode].Translations.GetValueOrDefault(tKey);
            }
            catch (Exception ex)
            {
                return key;
            }
        }

        public string T(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key) || _cultureCode.Equals("en"))
                {
                    return key;
                }

                var tKey = key.ToLower();

                if (!_translationFile[_cultureCode].Translations.Keys.Contains(tKey))
                {
                    #region Special parse and translete method
                    var tempKeys = key.Split(new char[] { ' ', '.', ',', ';', '-', '_', ':', '(', ')', '[', ']', '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tempKeys.Count() > 0)
                    {
                        foreach (var item in tempKeys)
                        {
                            var tItem = item.ToLower();
                            if (!_translationFile[_cultureCode].Translations.Keys.Contains(tItem))
                            {
                                if (Regex.IsMatch(tItem, @"^\d+$") == true)
                                {
                                    var tempNum = tItem;
                                    for (int i = 0; i < tItem.Length; i++)
                                    {
                                        tempNum = tempNum.Replace(tItem[i].ToString(), _translationFile[_cultureCode].Translations.GetValueOrDefault(tItem[i].ToString()));
                                    }
                                    key = key.Replace(tItem, tempNum);
                                }
                            }
                            else
                            {
                                key = key.Replace(item, _translationFile[_cultureCode].Translations.GetValueOrDefault(item));
                            }
                        }
                    }
                    #endregion

                    return key;
                }
                return _translationFile[_cultureCode].Translations.GetValueOrDefault(tKey);
            }
            catch (Exception ex)
            {
                return key;
            }
        }

        public static void LoadTranslationFile(string cultureCode)
        {
            if (cultureCode == "en")
            {
                return;
            }

            FileInfo translationFile = TranslationFile.GetTranslationFilePath(cultureCode);

            if (File.Exists(translationFile.FullName) == false)
            {
                if (Directory.Exists(translationFile.DirectoryName) == false)
                {
                    Directory.CreateDirectory(translationFile.DirectoryName);
                }
                File.Create(translationFile.FullName).Dispose();
            }

            var translationFileData = NccFileHelper.ReadAllText(translationFile.FullName);

            if (string.IsNullOrWhiteSpace(translationFileData))
            {
                var assembly = typeof(SharedResource).GetTypeInfo().Assembly;
                using (Stream resource = assembly.GetManifestResourceStream("NetCoreCMS.Framework.Resources.SharedResource.lang"))
                {
                    var sr = new StreamReader(resource);
                    translationFileData = sr.ReadToEndAsync().Result;
                    NccFileHelper.WriteAllText(translationFile.FullName, translationFileData);
                }
            }

            _translationFile[cultureCode] = JsonConvert.DeserializeObject<TranslationFileData>(translationFileData);
        }

        public static ConcurrentDictionary<string, TranslationFileData> LoadTranslations()
        {
            foreach (var item in SupportedCultures.Cultures)
            {
                LoadTranslationFile(item.TwoLetterISOLanguageName);
            }
            return _translationFile;
        }
    }
}
