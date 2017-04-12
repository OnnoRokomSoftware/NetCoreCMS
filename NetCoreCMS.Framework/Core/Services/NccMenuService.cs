using System.Collections.Generic;
using System.Linq;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Mvc.Services;
using NetCoreCMS.Framework.Core.Repository;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccMenuService : IBaseService<NccMenu>
    {
        private readonly NccMenuRepository _entityRepository;

        public NccMenuService(NccMenuRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }
         
        public NccMenu Get(long entityId)
        {
            return _entityRepository.Query().FirstOrDefault(x => x.Id == entityId);
        }

        public NccMenu Save(NccMenu entity)
        {
            _entityRepository.Add(entity);
            _entityRepository.SaveChange();
            return entity;
        }

        public NccMenu Update(NccMenu entity)
        {
            var oldEntity = _entityRepository.Query().FirstOrDefault(x => x.Id == entity.Id);
            if(oldEntity != null)
            {
                using (var txn = _entityRepository.BeginTransaction())
                {
                    CopyNewData(oldEntity, entity);
                    _entityRepository.Eidt(oldEntity);
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
                _entityRepository.Eidt(entity);
                _entityRepository.SaveChange();
            }
        }

        public List<NccMenu> GetAll()
        {
            return _entityRepository.Query().ToList();
        }

        public List<NccMenu> GetAllByStatus(int status)
        {
            return _entityRepository.Query().Where(x => x.Status == status).ToList();
        }

        public List<NccMenu> GetAllByName(string name)
        {
            return _entityRepository.Query().Where(x => x.Name == name).ToList();
        }

        public List<NccMenu> GetAllByNameContains(string name)
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

        private void CopyNewData(NccMenu oldEntity, NccMenu entity)
        {                
            oldEntity.ModificationDate = entity.ModificationDate;
            oldEntity.ModifyBy = entity.GetCurrentUserId();
            oldEntity.Name = entity.Name;            
            oldEntity.Status = entity.Status;
            oldEntity.MenuFor = entity.MenuFor;
            oldEntity.MenuIconCls = entity.MenuIconCls;
            oldEntity.MenuPosition = entity.MenuPosition;
            oldEntity.Status = entity.Status;            
        }
        
    }
}
