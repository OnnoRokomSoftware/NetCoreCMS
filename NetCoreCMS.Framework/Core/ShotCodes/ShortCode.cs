/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

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
