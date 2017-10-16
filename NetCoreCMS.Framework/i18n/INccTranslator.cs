using System;
using System.Collections.Generic;

namespace NetCoreCMS.Framework.i18n
{
    public interface INccTranslator<T>
    {
        string this[string key] { get; set; }

        string CultureCode { get; }

        string Get(string key);
        NccTranslator<T> GetTranslator(Type resourceType, string cultureCode);
        Dictionary<string, string> LoadAll();
        void Save();
        void Set(string key, string value);
    }
}