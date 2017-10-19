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
