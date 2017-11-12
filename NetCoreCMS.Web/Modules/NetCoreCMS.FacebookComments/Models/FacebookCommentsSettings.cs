/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.FacebookComments.Models
{
    public class FacebookCommentsSettings
    {
        public FacebookCommentsSettings()
        {
            ModuleVersion = "1.0";
            IsActive = true;
            RemoveLanguageParamenter = false;
            ColorScheme = "light";
            NumberOfPost = 5;
        }

        public string ModuleVersion { get; set; }
        public bool IsActive { get; set; }
        public bool RemoveLanguageParamenter { get; set; }
        public string ColorScheme { get; set; }
        public string FacebookAppId { get; set; }
        public int NumberOfPost { get; set; }
    }
}