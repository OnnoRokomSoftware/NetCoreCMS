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
    public class GlobalMessageRegistry
    {
        private static Dictionary<string, GlobalMessageEntry> _messageCache = new Dictionary<string, GlobalMessageEntry>();
        
        public static string RegisterMessage(GlobalMessage message, TimeSpan duration)
        {
            var msgId = Guid.NewGuid().ToString();
            var entry = new GlobalMessageEntry();
            message.MessageId = msgId;
            entry.ExpireTime = DateTime.Now.Add(duration);
            entry.Message = message;
            _messageCache.Add(msgId, entry);
            return msgId;
        }

        public static bool UnRegisterMessage(string messageId)
        {
            if (_messageCache.ContainsKey(messageId) )
            {
                return _messageCache.Remove(messageId);
            }
            return false;
        }

        public static List<GlobalMessage> GetAllMessage()
        {
            Cleanup();
            var msgList = new List<GlobalMessage>();
            foreach (var item in _messageCache)
            {
                msgList.Add(item.Value.Message);
            }
            return msgList;
        }

        private static void Cleanup()
        {
            List<string> removeList = new List<string>();
            foreach (var item in _messageCache)
            {
                if(item.Value.ExpireTime < DateTime.Now)
                {
                    removeList.Add(item.Key);
                }
            }
            foreach (var item in removeList)
            {
                _messageCache.Remove(item);
            }
        }

        public static void FlushMessages()
        {

        }

        class GlobalMessageEntry
        {
            public GlobalMessage Message { get; set; }
            public DateTime ExpireTime { get; set; }
        }


    }
    
}
