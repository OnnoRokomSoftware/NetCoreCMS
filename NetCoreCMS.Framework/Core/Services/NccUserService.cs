using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccUserService : IBaseService<NccUser>
    {
        private readonly NccUserRepository _entityRepository;
         
        public NccUserService(NccUserRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccUser Get(long entityId, bool isAsNoTracking = false)
        {
            return _entityRepository.Get(entityId);
        }

        public List<NccUser> LoadAll(bool isActive = true, int status = -1, string name = "", bool isLikeSearch = false)
        {
            return _entityRepository.LoadAll(isActive, status, name, isLikeSearch);
        }

        public NccUser GetByUserName(string userName)
        {
            return _entityRepository.Query().Include("Roles").FirstOrDefault(x => x.UserName == userName);
        }

        public NccUser Save(NccUser entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccUser Update(NccUser entity)
        {
            var oldEntity = _entityRepository.Get(entity.Id);
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
            var entity = _entityRepository.Get(entityId );
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

        private void CopyNewData(NccUser oldEntity, NccUser entity)
        { 
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.ModifyBy;
            oldEntity.Name = entity.Name; 
            oldEntity.Status = entity.Status; 
            oldEntity.CreateBy = entity.CreateBy;
            oldEntity.CreationDate = entity.CreationDate;  
            
            oldEntity.Status = entity.Status; 
            oldEntity.VersionNumber = entity.VersionNumber;
            oldEntity.AccessFailedCount = entity.AccessFailedCount;
            
            oldEntity.ConcurrencyStamp = entity.ConcurrencyStamp;
            oldEntity.Email = entity.Email;
            oldEntity.EmailConfirmed = entity.EmailConfirmed;
            oldEntity.FullName = entity.FullName;
            oldEntity.LockoutEnabled = entity.LockoutEnabled;
            oldEntity.LockoutEnd = entity.LockoutEnd;
            
            oldEntity.Mobile = entity.Mobile;
            oldEntity.NormalizedEmail = entity.NormalizedEmail;
            oldEntity.NormalizedUserName = entity.NormalizedUserName;
            oldEntity.PasswordHash = entity.PasswordHash;
            oldEntity.PhoneNumber = entity.PhoneNumber;
            oldEntity.PhoneNumberConfirmed = entity.PhoneNumberConfirmed;
            oldEntity.SecurityStamp = entity.SecurityStamp;
            oldEntity.Slug = entity.Slug;
            oldEntity.Status = entity.Status;
            oldEntity.TwoFactorEnabled = entity.TwoFactorEnabled;
            oldEntity.UserName = entity.UserName;
            oldEntity.Metadata = entity.Metadata;
        } 
    }
}
