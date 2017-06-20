using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccWebSiteService : IBaseService<NccWebSite>
    {
        private readonly NccWebSiteRepository _entityRepository;

        public NccWebSiteService()
        {
        }
        public NccWebSiteService(NccWebSiteRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccWebSite Get(long entityId)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
        }

        public NccWebSite Save(NccWebSite entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccWebSite Update(NccWebSite entity)
        {
            var oldEntity = _entityRepository.Query().FirstOrDefault(x => x.Id == entity.Id);
            if(oldEntity != null)
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
            var entity = _entityRepository.Query().FirstOrDefault(x => x.Id == entityId );
            if (entity != null)
            {
                entity.Status = EntityStatus.Deleted;
                _entityRepository.Edit(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<NccWebSite> LoadAll()
        {
            return _entityRepository.Query().ToList();
        }

        public List<NccWebSite> LoadAllByStatus(int status)
        {
            return _entityRepository.Query().Where(x => x.Status == status).ToList();
        }

        public List<NccWebSite> LoadAllByName(string name)
        {
            return _entityRepository.Query().Where(x => x.Name == name).ToList();
        }

        public List<NccWebSite> LoadAllByNameContains(string name)
        {
            return _entityRepository.Query().Where(x => x.Name.Contains(name)).ToList();
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

        private void CopyNewData(NccWebSite oldEntity, NccWebSite entity)
        {
            oldEntity.AllowRegistration = entity.AllowRegistration;
            oldEntity.Copyrights = entity.Copyrights;
            oldEntity.DateFormat = entity.DateFormat;
            oldEntity.DomainName = entity.DomainName;
            oldEntity.EmailAddress = entity.EmailAddress;
            oldEntity.FaviconUrl = entity.FaviconUrl;
            oldEntity.Language = entity.Language;
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.NewUserRole = entity.NewUserRole;
            oldEntity.SiteLogoUrl = entity.SiteLogoUrl;
            oldEntity.SiteTitle = entity.SiteTitle;
            oldEntity.Status = entity.Status;
            oldEntity.Tagline = entity.Tagline;
            oldEntity.TimeFormat = entity.TimeFormat;
            oldEntity.TimeZone = entity.TimeZone;
        }
        
    }
}
