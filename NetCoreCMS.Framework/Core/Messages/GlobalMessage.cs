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

namespace NetCoreCMS.Framework.Core.Messages
{
    public class GlobalMessage
    {
        public string MessageId { get; set; }
        public string Text { get; set; }
        public MessageType Type { get; set; }
        public MessageFor For { get; set; }
        public string Registrater { get; set; }

        public enum MessageType
        {
            Success,
            Warning,
            Error,
            Fatal
        }

        public enum MessageFor
        {
            Admin,
            Site,
            Both
        }
    }
}
