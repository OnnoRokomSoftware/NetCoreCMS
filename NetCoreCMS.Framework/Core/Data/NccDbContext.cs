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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NetCoreCMS.Framework.Setup;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Design;
using NetCoreCMS.Framework.Core.Messages;

namespace NetCoreCMS.Framework.Core.Data
{
    public class NccDbContext : IdentityDbContext<NccUser, NccRole, long, IdentityUserClaim<long>, NccUserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>, IDesignTimeDbContextFactory<NccDbContext>
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
            foreach (var module in GlobalContext.Modules.Where(x=>x.ModuleStatus == (int) NccModule.NccModuleStatus.Active).ToList())
            {
                try
                {
                    typeToRegisters.AddRange(module.Assembly.DefinedTypes.Select(t => t.AsType()));
                }
                catch (Exception ex)
                {
                    GlobalMessageRegistry.RegisterMessage(
                        new GlobalMessage()
                        {
                            For = GlobalMessage.MessageFor.Admin,
                            Registrater = "OnModelCreating",
                            Text = ex.Message,
                            Type = GlobalMessage.MessageType.Error
                        },
                        new TimeSpan(0, 0, 60)
                    );
                }
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
            var customModelBuilderTypes = typeToRegisters.Where(x => typeof(IModelBuilder).IsAssignableFrom(x));
            foreach (var builderType in customModelBuilderTypes)
            {
                try
                {
                    if (builderType != null && builderType != typeof(IModelBuilder))
                    {
                        var builder = (IModelBuilder)Activator.CreateInstance(builderType);
                        builder.Build(modelBuilder);
                    }
                }
                catch (Exception ex)
                {
                    GlobalMessageRegistry.RegisterMessage(
                        new GlobalMessage()
                        {
                            For = GlobalMessage.MessageFor.Admin,
                            Registrater = "RegisterUserModuleModels",
                            Text = ex.Message,
                            Type = GlobalMessage.MessageType.Error
                        },
                        new TimeSpan(0, 0, 60)
                    );
                }
            }
        }

        public NccDbContext Create(DbContextFactoryOptions options)
        {
            SetupHelper.LoadSetup();
            var opts = SetupHelper.GetDbContextOptions();
            var nccDbConetxt = new NccDbContext(opts);
            return nccDbConetxt;
        }

        public NccDbContext CreateDbContext(string[] args)
        {
            SetupHelper.LoadSetup();
            var opts = SetupHelper.GetDbContextOptions();
            var nccDbConetxt = new NccDbContext(opts);
            return nccDbConetxt;
        }
    }

}
