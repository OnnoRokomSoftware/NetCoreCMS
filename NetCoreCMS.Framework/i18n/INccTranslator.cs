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
using System.Collections.Generic;

namespace NetCoreCMS.Framework.i18n
{
    public interface INccTranslator
    {
        string this[string key] { get; set; }

        string CultureCode { get; }

        string Get(string key);
        INccTranslator GetTranslator(string cultureCode);
        Dictionary<string, string> LoadAll();
        void Save();
        void Set(string key, string value);
    }
}