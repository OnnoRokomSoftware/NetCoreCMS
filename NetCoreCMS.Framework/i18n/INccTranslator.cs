/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
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