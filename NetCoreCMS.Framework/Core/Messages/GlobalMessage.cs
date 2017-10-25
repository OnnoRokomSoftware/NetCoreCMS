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

namespace NetCoreCMS.Framework.Core.Messages
{
    public class GlobalMessage
    {
        public GlobalMessage() { ForUsers = new List<string>(); }
        public GlobalMessage(string text, MessageType type, MessageFor msgFor, string registerer, List<string> forUsers)
        {
            ForUsers = new List<string>();
            if(forUsers != null)
            {
                ForUsers = forUsers;
            }
            MessageId = Guid.NewGuid().ToString();
            Text = text;
            Type = type;
            For = msgFor;
            Registrater = registerer;
        }

        public string MessageId { get; set; }
        public string Text { get; set; }
        public MessageType Type { get; set; }
        public MessageFor For { get; set; }
        public string Registrater { get; set; }
        public List<string> ForUsers { get; set; }

        public enum MessageType
        {
            Success,
            Info,
            Warning,
            Error
        }

        public enum MessageFor
        {
            Admin,
            Site,
            Both
        }
    }
}
