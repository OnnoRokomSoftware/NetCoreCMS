/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetCoreCMS.Framework.Core.Messages
{
    public class GlobalMessageRegistry
    {
        public static string MessageDbFile { get { return "global_message_db.json"; } }
        private static Dictionary<string, GlobalMessageEntry> _messageCache = new Dictionary<string, GlobalMessageEntry>();
        
        public static string RegisterMessage(GlobalMessage message, TimeSpan duration)
        {
            var msgId = Guid.NewGuid().ToString();
            var entry = new GlobalMessageEntry();
            message.MessageId = msgId;
            entry.ExpireTime = DateTime.Now.Add(duration);
            entry.Message = message;
            _messageCache.Add(msgId, entry);
            WriteMessagesToStorage();
            return msgId;
        }

        private static void WriteMessagesToStorage()
        {
            var messages = JsonConvert.SerializeObject(_messageCache, Formatting.Indented);
            var messageDbFolder = Path.Combine(GlobalContext.ContentRootPath,"Data","GlobalMessages");
            if (Directory.Exists(messageDbFolder) == false)
            {
                Directory.CreateDirectory(messageDbFolder);
            }

            var messageDbFilePath = Path.Combine(messageDbFolder, MessageDbFile);             
            using (var sw = File.CreateText(messageDbFilePath))
            {
                sw.WriteLine(messages);
            }             
        }

        public static void LoadMessagesFromStorage()
        {
           
            var messageDbFolder = Path.Combine(GlobalContext.ContentRootPath, "Data", "GlobalMessages"); 
            var messageDbFilePath = Path.Combine(messageDbFolder, MessageDbFile);
            if (File.Exists(messageDbFilePath))
            {
                var msgJson = File.ReadAllText(messageDbFilePath, Encoding.UTF8);
                if(string.IsNullOrEmpty(msgJson) == false)
                {
                    var msgs = JsonConvert.DeserializeObject<Dictionary<string, GlobalMessageEntry>>(msgJson);
                    if (msgs != null) {
                        _messageCache = msgs;
                    }
                }
            }
        }

        public static bool UnRegisterMessage(string messageId)
        {
            if (_messageCache.ContainsKey(messageId) )
            {
                return _messageCache.Remove(messageId);
            }
            return false;
        }

        public static List<GlobalMessage> GetMessages(GlobalMessage.MessageFor messageFor)
        {
            Cleanup();
            var msgList = new List<GlobalMessage>();
            List<KeyValuePair<string, GlobalMessageEntry>> messages;
            
            messages = _messageCache.Where(x => x.Value.Message.For == messageFor || x.Value.Message.For == GlobalMessage.MessageFor.Both).ToList();
            
            foreach (var item in messages)
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

            WriteMessagesToStorage();
        }
         
        public class GlobalMessageEntry
        {
            public GlobalMessage Message { get; set; }
            public DateTime ExpireTime { get; set; }
        } 
    } 
}
