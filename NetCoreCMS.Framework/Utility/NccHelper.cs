using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

        public static Dictionary<int, string> LoadEmumToDictionary<T>(List<int> excludeList = null, bool? includeAll = null)
        {
            var enumType = typeof(T);
            Dictionary<int, string> enumToDictionary = new Dictionary<int, string>();
            if (includeAll != null && includeAll == true)
            {
                enumToDictionary.Add(0, "All");
            }
            var enumAllItems = Enum.GetValues(enumType);
            foreach (var currentItem in enumAllItems)
            {
                if ((excludeList != null && !excludeList.Contains((int)currentItem)) || (excludeList == null))
                {
                    DescriptionAttribute[] allDescAttributes = (DescriptionAttribute[])enumType.GetField(currentItem.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                    string description = allDescAttributes.Length > 0
                        ? allDescAttributes[0].Description
                        : currentItem.ToString();

                    enumToDictionary.Add((int)currentItem, description);
                }
            }
            return enumToDictionary;
        }

        public static string GetEmumIdToValue<T>(int enumId)
        {
            try
            {
                string value = "-";
                Dictionary<int, string> enumToDictionary = LoadEmumToDictionary<T>();
                if (enumToDictionary.Any())
                {
                    value = enumToDictionary.Where(x => x.Key == enumId).Select(x => x.Value).Take(1).SingleOrDefault();
                }
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Dictionary<int, string> LoadStatusDictionary(bool withDeleted = false)
        {
            Dictionary<int, string> statusDictionary = new Dictionary<int, string>();
            statusDictionary.Add(EntityStatus.Active, GetStatusText(EntityStatus.Active));
            statusDictionary.Add(EntityStatus.Inactive, GetStatusText(EntityStatus.Inactive));
            if (withDeleted)
                statusDictionary.Add(EntityStatus.Deleted, GetStatusText(EntityStatus.Deleted));
            return statusDictionary;
        }

        public static string GetStatusText(int status)
        {
            string statusString = "-";

            if (status == EntityStatus.Active)
                statusString = "Active";
            else if (status == EntityStatus.Inactive)
                statusString = "Inactive";
            else if (status == EntityStatus.Deleted)
                statusString = "Deleted";

            return statusString;
        }
        
        public static List<T> LoadItemToList<T>(T item)
        {
            try
            {
                return new List<T> { item };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
