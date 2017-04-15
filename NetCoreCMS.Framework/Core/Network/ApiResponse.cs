using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Network
{
    public class ApiResponse
    {
        public string Message{ get; set; }
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
    }
}
