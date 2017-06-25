/*
 * Author: TecRT
 * Website: http://tecrt.com
 * Copyright (c) tecrt.com
 * License: BSD (3 Clause)
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NetCoreCMS.Framework.Setup;

namespace NetCoreCMS.Framework.Core.Data
{
    public class NccDbContext : IdentityDbContext<NccUser, NccRole, long, IdentityUserClaim<long>, NccUserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>, IDbContextFactory<NccDbContext>
    {
        public NccDbContext()
        {

        }
        public NccDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<Type> typeToRegisters = new List<Type>();
            foreach (var module in GlobalConfig.Modules)
            {
                typeToRegisters.AddRange(module.Assembly.DefinedTypes.Select(t => t.AsType()));
            }
            
            //ScanEntities(modelBuilder, typeToRegisters);
            //SetTableNameByConvention(modelBuilder);
            base.OnModelCreating(modelBuilder);
            RegisterUserModuleModels(modelBuilder, typeToRegisters);
            RegisterCoreModels(modelBuilder);
            
        }

        private void RegisterCoreModels(ModelBuilder modelBuilder)
        {
            CoreModelBuilder imb = new CoreModelBuilder();
            imb.Build(modelBuilder);
        }

        private static void SetTableNameByConvention(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (entity.ClrType.Namespace != null)
                {
                    //var nameParts = entity.ClrType.Namespace.Split('.');
                    //var tableName = string.Concat(nameParts[2], "_", entity.ClrType.Name);
                    modelBuilder.Entity(entity.Name).ToTable(entity.ClrType.Name);
                }
            }
        }

        private static void ScanEntities(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
        {
            var entityTypes = typeToRegisters.Where(x => typeof(IBaseModel<long>).IsAssignableFrom(x) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract);
            foreach (var type in entityTypes)
            {
                modelBuilder.Entity(type);
            }
        }

        private static void RegisterUserModuleModels(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
        {
            var customModelBuilderTypes = typeToRegisters.Where(x => typeof(INccModuleBuilder).IsAssignableFrom(x));
            foreach (var builderType in customModelBuilderTypes)
            {
                if (builderType != null && builderType != typeof(INccModuleBuilder))
                {
                    var builder = (INccModuleBuilder)Activator.CreateInstance(builderType);
                    builder.Build(modelBuilder);
                }
            }
        }

        public NccDbContext Create(DbContextFactoryOptions options)
        {
            var opts = SetupHelper.GetDbContextOptions();
            var nccDbConetxt = new NccDbContext(opts);
            return nccDbConetxt;
        }
    }

}
