/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

namespace NetCoreCMS.Framework.Core.Auth
{
    public class NccCmsRoles
    {
        public static string SuperAdmin { get; } = "SuperAdmin";
        public static string Administrator { get; } = "Administrator";
        public static string Editor { get; } = "Editor";
        public static string Author { get; } = "Author";
        public static string Contributor { get; } = "Contributor";
        public static string Subscriber { get; } = "Subscriber";
    }
}
