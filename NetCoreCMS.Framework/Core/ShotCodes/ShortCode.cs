using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.ShotCodes
{
    public class ShortCode
    {
        public ShortCode()
        {
            Paramiters = new List<string>();
        }
        public int Start { get; set; }
        public int End { get; set; }
        public string Name { get; set; }
        public List<string> Paramiters { get; set; }
    }
}
