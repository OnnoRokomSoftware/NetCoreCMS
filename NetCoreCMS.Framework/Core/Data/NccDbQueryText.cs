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
