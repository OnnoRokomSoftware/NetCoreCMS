/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Setup;

namespace NetCoreCMS.Framework.Core.App
{
    public class NetCoreStartup
    {
        public void SelectDatabase(IServiceCollection services)
        {

            #region Database Selection

            if (SetupHelper.SelectedDatabase == "SqLite")
            {
                services.AddDbContext<NccDbContext>(options =>
                    options.UseSqlite(SetupHelper.ConnectionString, opt => opt.MigrationsAssembly("NetCoreCMS.Framework")), ServiceLifetime.Scoped, ServiceLifetime.Scoped
                );
            }
            else if (SetupHelper.SelectedDatabase == "MSSQL")
            {
                services.AddDbContext<NccDbContext>(options =>
                    options.UseSqlServer(SetupHelper.ConnectionString, opt => opt.MigrationsAssembly("NetCoreCMS.Framework")), ServiceLifetime.Scoped, ServiceLifetime.Scoped
                );
            }
            else if (SetupHelper.SelectedDatabase == "MySql")
            {
                services.AddDbContext<NccDbContext>(options =>
                    options.UseMySql(SetupHelper.ConnectionString, opt => opt.MigrationsAssembly("NetCoreCMS.Framework")), ServiceLifetime.Scoped, ServiceLifetime.Scoped
                );
                
            }
            else
            {
                services.AddDbContext<NccDbContext>(options =>
                    options.UseInMemoryDatabase("NetCoreCMS")
                );
            }

            #endregion
        }
    }
}
