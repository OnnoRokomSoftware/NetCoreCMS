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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Core.Messages
{
    public class GlobalMessageRegistry
    {
        public static string MessageDbFile { get { return "global_message_db.json"; } }
        private static volatile ConcurrentDictionary<string, GlobalMessageEntry> _messageCache = new ConcurrentDictionary<string, GlobalMessageEntry>();
        
        public static string RegisterMessage(GlobalMessage message, TimeSpan duration)
        {
            var msgId = Guid.NewGuid().ToString();
            var entry = new GlobalMessageEntry();
            message.MessageId = msgId;
            entry.ExpireTime = DateTime.Now.Add(duration);
            entry.Message = message;
            _messageCache.TryAdd(msgId, entry);
            WriteMessagesToStorage();
            return msgId;
        }

        private static void WriteMessagesToStorage()
        {
            var task = new Task(() => {

                try
                {
                    var messageDbFilePath = GetMessageDbFilePath();

                    if(File.Exists(messageDbFilePath) == false)
                    {
                        using (File.Open(messageDbFilePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite)) { }
                    }

                    if (_messageCache.Count > 0)
                    {
                        NccFileHelper.WriteObject(messageDbFilePath, _messageCache);
                    }
                    else
                    {
                        using (var originalFileStream = File.Open(messageDbFilePath, FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite)) { }
                    }
                }
                catch (Exception ex)
                {
                    //TODO: have to improve this.
                }                
            });
            task.Start();
            task.Wait();            
        }

        public static void LoadMessagesFromStorage()
        {

            try
            {
                var messageDbFilePath = GetMessageDbFilePath();
                if (File.Exists(messageDbFilePath))
                {  
                    var msgs = NccFileHelper.LoadObject<Dictionary<string, GlobalMessageEntry>>(messageDbFilePath);
                    if (msgs != null)
                    {
                        foreach (var item in msgs)
                        {
                            _messageCache.AddOrUpdate(item.Key, item.Value, (key, val) =>
                            {
                                return item.Value;
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: have to chose better approch.
                using (var originalFileStream = File.Open(GetMessageDbFilePath(), FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {                    
                }
            }
        }

        private static string GetMessageDbFilePath()
        {
            var messageDbFolder = Path.Combine(GlobalContext.ContentRootPath, "Data", "GlobalMessages");            
            if (Directory.Exists(messageDbFolder) == false)
            {
                Directory.CreateDirectory(messageDbFolder);
            }                       
            return Path.Combine(messageDbFolder, MessageDbFile);
        }

        public static bool UnRegisterMessage(string messageId)
        {
            if (_messageCache.ContainsKey(messageId) )
            {
                return _messageCache.TryRemove(messageId, out GlobalMessageEntry msg);
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
            bool requireWrite = false;

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
                GlobalMessageEntry msg;
                _messageCache.TryRemove(item, out msg);
                requireWrite = true;
            }

            if (requireWrite)
            {
                WriteMessagesToStorage();
            }
        }
         
        public class GlobalMessageEntry
        {
            public GlobalMessage Message { get; set; }
            public DateTime ExpireTime { get; set; }
        } 
    } 
}
