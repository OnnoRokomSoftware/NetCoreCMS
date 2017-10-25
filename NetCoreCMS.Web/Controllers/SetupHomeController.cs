/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetCoreCMS.Core.Modules.Setup.Models.ViewModels;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreCMS.Core.Modules.Setup.Controllers
{
    [AllowAnonymous]
    public class SetupHomeController : NccController
    { 

        IHttpContextAccessor _httpContextAccessor;
        ILoggerFactory _loggerFactory;

        public SetupHomeController(IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<SetupHomeController>();
        }
        public ActionResult Index()
        {
            if (!SetupHelper.IsDbCreateComplete)
            {
                return View();
            }
            else if(!SetupHelper.IsAdminCreateComplete)
            {
                return RedirectToAction("CreateAdmin");
            }

            return RedirectToAction("Success");
        }

        [HttpPost]
        public ActionResult Index(SetupViewModel setup)
        {
            if (ModelState.IsValid && !SetupHelper.IsDbCreateComplete)
            {
                SetupHelper.ConnectionString = DatabaseFactory.GetConnectionString(setup.Database, setup);
                SetupHelper.SelectedDatabase = setup.Database.ToString();
                SetupHelper.IsDbCreateComplete = SetupHelper.CreateDatabase(setup.Database, setup);               
                SetupHelper.SaveSetup();
                return RedirectToAction("CreateAdmin");
            }
            else
            {
                ModelState.AddModelError("Database","Invalid Data");
            }

            return View();
        }

        public ActionResult Back()
        {
            if (SetupHelper.IsDbCreateComplete)
            {
                SetupHelper.IsDbCreateComplete = false;
                SetupHelper.DeleteSetup();
            }
            return RedirectToAction("Index");
        }

        public ActionResult CreateAdmin()
        {
            ViewBag.Languages = new SelectList(SupportedCultures.Cultures.Select(x => new { Value = x.TwoLetterISOLanguageName, Text = x.DisplayName }).ToList(), "Value","Text");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAdmin(AdminViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Languages = new SelectList(SupportedCultures.Cultures.Select(x => new { Value = x.TwoLetterISOLanguageName, Text = x.DisplayName }).ToList(), "Value", "Text");
                return View(viewModel);
            }

            SetupHelper.InitilizeDatabase();

            var optionBuilder = new DbContextOptionsBuilder<NccDbContext>();

            DatabaseEngine dbe = TypeConverter.TryParseDatabaseEnum(SetupHelper.SelectedDatabase);

            switch (dbe)
            {
                case DatabaseEngine.MSSQL:
                    optionBuilder.UseSqlServer(SetupHelper.ConnectionString, opts => opts.MigrationsAssembly("NetCoreCMS.Framework"));                    
                    break;
                case DatabaseEngine.MsSqlLocalStorage:
                    break;
                case DatabaseEngine.MySql:
                    optionBuilder.UseMySql(SetupHelper.ConnectionString, opts => opts.MigrationsAssembly("NetCoreCMS.Framework"));                    
                    break;                
                case DatabaseEngine.SqLite:
                    optionBuilder.UseSqlite(SetupHelper.ConnectionString, opts => opts.MigrationsAssembly("NetCoreCMS.Framework"));
                    break;
                case DatabaseEngine.PgSql:
                    break;
            }
            
            var nccDbConetxt = new NccDbContext(optionBuilder.Options);

            var userStore = new NccUserStore(nccDbConetxt);
            var identityOptions = Options.Create(new IdentityOptions());
            var passwordHasher = new PasswordHasher<NccUser>();
            var userValidatorList = new List<UserValidator<NccUser>>();
            var passwordValidatorList = new List<PasswordValidator<NccUser>>();
            var lookupNormalizer = new UpperInvariantLookupNormalizer();
            var identityErrorDescriber = new IdentityErrorDescriber();
            var logger = _loggerFactory.CreateLogger<UserManager<NccUser>>();
            var authOption = new AuthenticationOptions();            
            var options = Options.Create<AuthenticationOptions>(authOption);            
            var scheme = new AuthenticationSchemeProvider(options);
            
            var userManager = new UserManager<NccUser>(
                userStore,
                identityOptions,
                passwordHasher,
                userValidatorList,
                passwordValidatorList,
                lookupNormalizer,
                identityErrorDescriber,
                GlobalContext.App.ApplicationServices,
                logger
            );
            
            var roleStore = new NccRoleStore(nccDbConetxt);
            var roleValidatorList = new List<RoleValidator<NccRole>>();
            var roleLogger = _loggerFactory.CreateLogger<RoleManager<NccRole>>();

            var roleManager = new RoleManager<NccRole>(
                roleStore,
                roleValidatorList,
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                roleLogger
                );

            var claimsFactory = new UserClaimsPrincipalFactory<NccUser,NccRole>(userManager, roleManager, identityOptions);
            var signInLogger = _loggerFactory.CreateLogger<SignInManager<NccUser>>();
            var signInManager = new NccSignInManager<NccUser>(userManager, _httpContextAccessor, claimsFactory, identityOptions, signInLogger, scheme);
            
            //nccDbConetxt.Database.Migrate();

            var setupInfo = new WebSiteInfo()
            {
                SiteName = viewModel.SiteName,
                Tagline = viewModel.Tagline,
                AdminPassword = viewModel.AdminPassword,
                AdminUserName = viewModel.AdminUserName,
                ConnectionString = SetupHelper.ConnectionString,
                Database = TypeConverter.TryParseDatabaseEnum(SetupHelper.SelectedDatabase),
                Email = viewModel.Email,
                Language = viewModel.Language
            };

            SetupHelper.RegisterAuthServices(dbe);
            var admin = await SetupHelper.CreateSuperAdminUser(userManager, roleManager, signInManager,  setupInfo );            
            SetupHelper.IsAdminCreateComplete = true;
            SetupHelper.Language = viewModel.Language;
            SetupHelper.SaveSetup();
            SetupHelper.CrateNccWebSite(nccDbConetxt, setupInfo);

            return Redirect("/Home/SetupSuccess");
            
        }
  
        public ActionResult Success()
        {
            return View();
        }
        
    }
}
