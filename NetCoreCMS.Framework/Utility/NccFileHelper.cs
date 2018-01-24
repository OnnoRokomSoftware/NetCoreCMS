using Newtonsoft.Json;
using System.IO;

namespace NetCoreCMS.Framework.Utility
{
    public class NccFileHelper
    {
        public static string ReadAllText(string filePath)
        {
            var content = "";
             
            using (var file = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    content = sr.ReadToEnd();
                }
            }
            
            return content;
        }

        public static void WriteAllText(string filePath, string text)
        {
            using (File.Open(filePath, FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite)) { };
            using (var file = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(text);
                }
            }
        }

        public static ObjectT LoadObject<ObjectT>(string filePath)
        {
            var content = "";
            var obj = default(ObjectT);
            
            using (var file = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    content = sr.ReadToEnd();
                }
            }

            if (string.IsNullOrEmpty(content)) { return default(ObjectT); }
            obj = JsonConvert.DeserializeObject<ObjectT>(content);
            
            return obj;
        }

        public static void WriteObject<ObjectT>(string filePath, ObjectT @object)
        {
            using (File.Open(filePath, FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite)) { };
            using (var file = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                var text = JsonConvert.SerializeObject(@object, Formatting.Indented);
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(text);
                }
            }
        }
    }
}
