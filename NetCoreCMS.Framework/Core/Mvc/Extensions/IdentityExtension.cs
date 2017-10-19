/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System.Security.Claims;

namespace NetCoreCMS.Framework.Core.Mvc.Extensions
{
    public static class IdentityExtension
    {
        public static long GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                return 0;
            var iDs = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            long.TryParse(iDs, out long userId);
            return userId;
        }
    }
 }
