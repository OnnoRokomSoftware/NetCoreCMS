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

namespace NetCoreCMS.Framework.Modules
{
    public interface IModuleMigration
    {
        void Up(Version existingModuleVersion, Version cmsVersion, Func<string, string> executeMigrationQuery);
        void Down(Version existingModuleVersion, Version cmsVersion, Func<string, string> executeMigrationQuery);
    }
}
