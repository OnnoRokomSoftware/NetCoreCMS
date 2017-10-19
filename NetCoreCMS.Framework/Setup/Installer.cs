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
using System.Text;

namespace NetCoreCMS.Framework.Setup
{
    public class Installer
    {
        public bool SetupDatabase()
        {
            return false;
        }

        public bool InstallCoreModules()
        {
            return false;
        }

        public bool InsertSampleData()
        {
            return false;
        }
    }
}
