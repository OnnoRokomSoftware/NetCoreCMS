/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

namespace NetCoreCMS.Framework.i18n
{
    public interface INccTranslator
    {
        string this[string key] { get; set; }
        string CultureCode { get; }
        string Get(string key);
        string T(string key);
        void Set(string key, string value);
        void SaveTranslations();
        INccTranslator GetTranslator(string cultureCode);
    }
}