using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.MatGallery.Repository;
using NetCoreCMS.MatGallery.Models;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.MatGallery.Services
{
    public class NccUserThemeService : IBaseService<NccUserTheme>
    {
        private readonly NccUserThemeRepository _entityRepository;
        private readonly NccUserThemeLogRepository _logRepository;

        public NccUserThemeService(NccUserThemeRepository entityRepository, NccUserThemeLogRepository logRepository)
        {
            _entityRepository = entityRepository;
            _logRepository = logRepository;
        }

        public NccUserTheme Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId);
        }

        public NccUserTheme Get(string ThemeId)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.ThemeId == ThemeId);
        }

        public NccUserTheme Save(NccUserTheme entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();

            NccUserThemeLog log = new NccUserThemeLog();
            CopyToLog(log, entity);
            _logRepository.Add(log);
            _logRepository.SaveChange();

            return entity;
        }

        public NccUserTheme Update(NccUserTheme entity)
        {
            var oldEntity = _entityRepository.Query().FirstOrDefault(x => x.Id == entity.Id);
            if (oldEntity != null)
            {
                using (var txn = _entityRepository.BeginTransaction())
                {
                    CopyNewData(oldEntity, entity);
                    //_entityRepository.Edit(oldEntity);
                    _entityRepository.SaveChange();
                    txn.Commit();
                }

                NccUserThemeLog log = new NccUserThemeLog();
                CopyToLog(log, entity);
                _logRepository.Add(log);
                _logRepository.SaveChange();
            }

            return entity;
        }

        public void Remove(long entityId)
        {
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<NccUserTheme> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch);
        }

        public void DeletePermanently(long entityId)
        {
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
            if (entity != null)
            {
                _entityRepository.Remove(entity);
                _entityRepository.SaveChange();
            }
        }

        private void CopyNewData(NccUserTheme oldEntity, NccUserTheme entity)
        {
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;

            oldEntity.Version = entity.Version;
            oldEntity.NetCoreCMSVersion = entity.NetCoreCMSVersion;

            oldEntity.ThemeId = entity.ThemeId;
            oldEntity.ThemeName = entity.ThemeName;
            oldEntity.Description = entity.Description;

            oldEntity.Category = entity.Category;
            oldEntity.IsPrivate = entity.IsPrivate;

            oldEntity.Author = entity.Author;
            oldEntity.Email = entity.Email;
            oldEntity.Website = entity.Website;
        }



        public List<NccUserThemeLog> LoadLog(long nccUserThemeId)
        {
            return _logRepository.Query().Where(x=>x.nccUserTheme.Id == nccUserThemeId).ToList();
        }

        private void CopyToLog(NccUserThemeLog logEntity, NccUserTheme entity)
        {
            logEntity.ModificationDate = entity.ModificationDate;
            logEntity.ModifyBy = entity.ModifyBy;
            logEntity.Name = entity.Name;
            logEntity.Status = entity.Status;

            logEntity.Version = entity.Version;
            logEntity.NetCoreCMSVersion = entity.NetCoreCMSVersion;

            logEntity.ThemeId = entity.ThemeId;
            logEntity.ThemeName = entity.ThemeName;
            logEntity.Description = entity.Description;

            logEntity.Category = entity.Category;
            logEntity.IsPrivate = entity.IsPrivate;

            logEntity.Author = entity.Author;
            logEntity.Email = entity.Email;
            logEntity.Website = entity.Website;

            logEntity.nccUserTheme = entity;
        }
    }
}