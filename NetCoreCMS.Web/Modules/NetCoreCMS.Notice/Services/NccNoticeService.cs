using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Notice.Repository;
using NetCoreCMS.Notice.Models;

namespace NetCoreCMS.Notice.Services
{
    public class NccNoticeService : IBaseService<NccNotice>
    {
        private readonly NccNoticeRepository _entityRepository;

        public NccNoticeService()
        {
        }
        public NccNoticeService(NccNoticeRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccNotice Get(long entityId)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
        }

        public NccNotice Save(NccNotice entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccNotice Update(NccNotice entity)
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

        internal List<NccNotice> LoadByStatusAndPage(NccNotice.NccNoticeStatus noticeStatus, NccNotice.NccNoticeType noticeType, int page, int pageSize)
        {
            return _entityRepository.Query().Where(
                x => x.NoticeStatus == noticeStatus
                && x.NoticeType == noticeType
            ).OrderByDescending(x => x.PublishDate)
            .OrderBy(x => x.NoticeOrder)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
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

        public List<NccNotice> LoadTopNoticesForWebSite(int top)
        {
            return _entityRepository.Query().Where(
                x => x.NoticeStatus == NccNotice.NccNoticeStatus.Published
                && x.NoticeType == NccNotice.NccNoticeType.Site    
            ).OrderByDescending( x => x.PublishDate)
            .OrderBy( x => x.NoticeOrder)            
            .Take(top)
            .ToList();
        }

        public List<NccNotice> LoadAll()
        {
            return _entityRepository.Query().ToList();
        }

        public List<NccNotice> LoadAllByStatus(int status)
        {
            return _entityRepository.Query().Where(x => x.Status == status).ToList();
        }

        public List<NccNotice> LoadAllByName(string name)
        {
            return _entityRepository.Query().Where(x => x.Name == name).ToList();
        }

        public List<NccNotice> LoadAllByNameContains(string name)
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

        private void CopyNewData(NccNotice oldEntity, NccNotice entity)
        {   
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name;
            oldEntity.Status = entity.Status;
            oldEntity.Content = entity.Content;
            oldEntity.ExpireDate = entity.ExpireDate;
            oldEntity.NoticeStatus = entity.NoticeStatus;
            oldEntity.NoticeType = entity.NoticeType;
            oldEntity.NoticeOrder = entity.NoticeOrder;
            oldEntity.PublishDate = entity.PublishDate;
            oldEntity.Title = entity.Title;
            
        }
        
    }
}
