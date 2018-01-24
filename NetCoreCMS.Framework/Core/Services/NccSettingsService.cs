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
using System.Linq;
using System.Reflection;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Service;
using NetCoreCMS.Framework.Core.Repository;
using Newtonsoft.Json;

namespace NetCoreCMS.Framework.Core.Services
{
    public interface INccSettingsService
    {
        bool CreateKey(string key, string value, out string message);
        bool CreateKey<EntityT>(string key, EntityT value, out string message);
        void DeletePermanently(long entityId);
        NccSettings Get(long entityId, bool isAsNoTracking = false, bool withDeleted = false);
        NccSettings GetByKey(string key = "Settings");
        EntityT GetByKey<EntityT>();
        List<NccSettings> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false, bool withDeleted = false);
        void Remove(long entityId);
        NccSettings Save(NccSettings entity);
        NccSettings SetByKey(string value, string key = "Settings");
        EntityT SetByKey<EntityT>(EntityT value);
        NccSettings Update(NccSettings entity);
    }

    public class NccSettingsService : BaseService<NccSettings>, INccSettingsService
    {
        private readonly NccSettingsRepository _entityRepository;

        public NccSettingsService(NccSettingsRepository entityRepository) : base(entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public bool CreateKey(string key, string value, out string message)
        {
            var existingKey = _entityRepository.GetByKey(key);
            if (existingKey != null)
            {
                message = "Key already exists. Please use another key";
                return false;
            }
            else
            {
                _entityRepository.Add(new NccSettings() { Key = key, Value = value });
                _entityRepository.SaveChange();
                message = "Create successful";
                return true;
            }
        }

        public bool CreateKey<EntityT>(string key, EntityT value, out string message)
        {
            var existingKey = _entityRepository.GetByKey(key);
            if (existingKey != null)
            {
                message = "Key already exists. Please use another key";
                return false;
            }
            else
            {
                _entityRepository.Add(new NccSettings() { Key = key, Value = JsonConvert.SerializeObject(value) });
                _entityRepository.SaveChange();
                message = "Create successful";
                return true;
            }
        }

        public NccSettings GetByKey(string key = "Settings")
        {
            return _entityRepository.GetByKey(GetKeyName(key));
        }
        public NccSettings SetByKey(string value, string key = "Settings")
        {
            key = GetKeyName(key);
            var settings = _entityRepository.Query().FirstOrDefault(x => x.Key == key);
            if (settings != null)
            {
                settings.Value = value;
                _entityRepository.Edit(settings);
            }
            else
            {
                settings = new NccSettings() { Key = key, Value = value };
                _entityRepository.Add(settings);
            }
            _entityRepository.SaveChange();

            return settings;
        }


        public EntityT GetByKey<EntityT>()
        {
            string key = GetKeyName(typeof(EntityT).Name);
            var settings = _entityRepository.GetByKey(key);
            if (settings == null)
            {
                //return default(EntityT);
                return Activator.CreateInstance<EntityT>();
            }
            return JsonConvert.DeserializeObject<EntityT>(settings.Value);
        }
        public EntityT SetByKey<EntityT>(EntityT value)
        {
            string key = GetKeyName(typeof(EntityT).Name);

            var json = JsonConvert.SerializeObject(value);
            var settings = _entityRepository.GetByKey(key);
            if (settings != null)
            {
                settings.Value = json;
                _entityRepository.Edit(settings);
            }
            else
            {
                settings = new NccSettings() { Key = key, Value = json };
                _entityRepository.Add(settings);
            }
            _entityRepository.SaveChange();

            return value;
        }

        #region Helper
        private string GetKeyName(string key)
        {
            key = key.Trim();
            var callingAssembly = Assembly.GetEntryAssembly().ManifestModule.Name;
            string prefix = callingAssembly;
            if (prefix.EndsWith(".dll"))
            {
                prefix = prefix.Replace(".dll", "");
            }
            prefix = prefix.Trim() + "_";
            return prefix + key;
        }
        #endregion
    }
}
