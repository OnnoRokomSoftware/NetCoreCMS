﻿/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.HelloWorld.Models
{
    public class HelloModel : BaseModel
    {
        public string Hello { get; set; }
    }
}