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

namespace NetCoreCMS.Framework.Core.Data
{
    public class NccDbQueryText
    {
        public string MySql_QueryText { get; set; }
        public string MSSql_QueryText { get; set; }
        public string SQLite_QueryText { get; set; }
    }
}
