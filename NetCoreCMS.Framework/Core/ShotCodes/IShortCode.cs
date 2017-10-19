/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

namespace NetCoreCMS.Framework.Core.ShotCodes
{
    /// <summary>
    /// Interface for implementing ShortCode.
    /// You must have set ShortCodeName. Which will be unique name.
    /// ShortCode of the content will be replaced by the returned string of Render() method.
    /// ShortCode Syntax:-
    /// [ShortCodeName ParamName="Value", ParamName2="Value", ParamName3="Value" ShortCodeName]
    /// </summary>
    public interface IShortCode
    {
        string ShortCodeName { get; }
        string Render(params object[] paramiters);
    }
}
