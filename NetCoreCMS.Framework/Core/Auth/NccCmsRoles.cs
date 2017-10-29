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
        /// <summary>
        /// All in all. Able to perform all operations of both CMS and Modules including development related operations. Able to delete All users except SuperAdmin.
        /// </summary>
        public static string SuperAdmin { get; } = "SuperAdmin";
        /// <summary>
        /// Able to do all CMS and all Modules specific operations. Able to create another administrator but not able to delete himself.
        /// </summary>
        public static string Administrator { get; } = "Administrator";
        /// <summary>
        /// Will be able to crate Editor and able to perform his level and Editor's level operations.
        /// </summary>
        public static string Manager { get; } = "Manager";
        /// <summary>
        /// Create/Edit/Delete/Publish/Unpublish/Approve/Reject/Download operations. Able to crate Editor.
        /// </summary>
        public static string Editor { get; } = "Editor";
        /// <summary>
        /// Create/Edit/Delte/Publish/Unpublish Post
        /// </summary>
        public static string Author { get; } = "Author";
        /// <summary>
        /// Create/Edit Post
        /// </summary>
        public static string Contributor { get; } = "Contributor";
        /// <summary>
        /// Create/Edit Comment, Update profile
        /// </summary>
        public static string Subscriber { get; } = "Subscriber";
    }
}
