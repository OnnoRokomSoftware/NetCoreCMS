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
