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
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using Newtonsoft.Json;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccSettingsService : IBaseService<NccSettings>
    {
        private readonly NccSettingsRepository _entityRepository;

        public NccSettingsService(NccSettingsRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public NccSettings Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId);
        }

        public List<NccSettings> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch);
        }

        public NccSettings Save(NccSettings entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccSettings Update(NccSettings entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id);
            if (oldEntity != null)
            {
                using (var txn = _entityRepository.BeginTransaction())
                {
                    CopyNewData(oldEntity, entity);
                    _entityRepository.Edit(oldEntity);
                    _entityRepository.SaveChange();
                    txn.Commit();
                }
            }

            return entity;
        }

        public void Remove(long entityId)
        {
            var entity = _entityRepository.Get(entityId);
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public void DeletePermanently(long entityId)
        {
            var entity = _entityRepository.Get(entityId);
            if (entity != null)
            {
                _entityRepository.Remove(entity);
                _entityRepository.SaveChange();
            }
        }

        private void CopyNewData(NccSettings oldEntity, NccSettings entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;
            oldEntity.CreateBy = entity.CreateBy;
            oldEntity.CreationDate = entity.CreationDate;
            oldEntity.Metadata = entity.Metadata;
            oldEntity.Status = entity.Status;
            oldEntity.VersionNumber = entity.VersionNumber;
            oldEntity.Key = entity.Key;
            oldEntity.Value = entity.Value;
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
                return default(EntityT);
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
