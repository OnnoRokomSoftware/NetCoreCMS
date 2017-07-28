using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Setup;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreCMS.Framework.Core.Mvc.Repository
{
    public class BaseRepository<EntityT, IdT> : IBaseRepository<EntityT, IdT> where EntityT : class, IBaseModel<IdT>
    {
        public BaseRepository(NccDbContext context)
        {
            Context = context;
            DbSet = Context.Set<EntityT>();
        }

        protected DbContext Context { get; }

        protected DbSet<EntityT> DbSet { get; }

        public EntityT Add(EntityT entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        public EntityT Edit(EntityT entity)
        {
            entity.Status = EntityStatus.Modified;
            Context.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public EntityT Get(IdT id)
        {
            return DbSet.FirstOrDefault(x => x.Id.Equals(id));
        }

        public List<EntityT> LoadAll()
        {
            return DbSet.ToList();
        }

        public List<EntityT> LoadAllActive()
        {
            return DbSet.Where(x=> x.Status >= EntityStatus.New).ToList();
        }

        public List<EntityT> LoadAllByStatus(int status)
        {
            return DbSet.Where(x => x.Status == status).ToList();
        }

        public List<EntityT> LoadAllByName(string name)
        {
            return DbSet.Where(x => x.Name == name).ToList();
        }

        public List<EntityT> LoadAllByNameContains(string name)
        {
            return DbSet.Where(x => x.Name.Contains(name)).ToList();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        public void SaveChange()
        {
            Context.SaveChanges();
        }

        public IQueryable<EntityT> Query()
        {
            return DbSet;
        }

        public EntityEntry GetEntityEntry(EntityT T)
        {
            return (EntityEntry)Context.Entry(T);
        }

        public void Remove(EntityT entity)
        {
            DbSet.Remove(entity);
        }

        public void DeletePermanently(EntityT entity)
        {
            DbSet.Remove(entity);
        }

        public int ExecuteSqlCommand(NccDbQueryText query)
        {
            var queryText = "";
            if (SetupHelper.SelectedDatabase == "SqLite")
            {
                queryText = query.SQLite_QueryText;
            }
            else if (SetupHelper.SelectedDatabase == "MSSQL")
            {
                queryText = query.MSSql_QueryText;
            }
            else if (SetupHelper.SelectedDatabase == "MySql")
            {
                queryText = query.MySql_QueryText;
            }
            else
            {
                return -1;
            }

            var effRow = Context.Database.ExecuteSqlCommand(queryText);
            return effRow;
        }
    }
}
