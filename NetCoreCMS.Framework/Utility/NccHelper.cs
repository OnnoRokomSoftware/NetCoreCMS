using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace NetCoreCMS.Framework.Utility
{
    /// <summary>
    /// Utility functions for NetCoreCMS
    /// </summary>
    public static class NccHelper
    {
        /// <summary>
        /// Clone any object or object list deeply.
        /// </summary>
        /// <typeparam name="T">Class type of the object you want to clone.</typeparam>
        /// <param name="obj">The object which will be cloned.</param>
        /// <returns></returns>
        public static T DeepClone<T>(T obj)
        {
            T objResult;
            using (MemoryStream ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(ms, obj);
                ms.Position = 0;
                objResult = (T)serializer.Deserialize(ms);
            }
            
            return objResult;
        }
    }
}
