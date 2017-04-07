using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Services.Auth;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Modules.Setup.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreCMS.Modules.Cms.Controllers
{
    public class SetupHomeController : NccController
    {
        IHostingEnvironment _env;
        private readonly UserManager<NccUser> _userManager;
        private readonly RoleManager<NccRole> _roleManager;
        private readonly SignInManager<NccUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;
        IHttpContextAccessor _httpContextAccessor;
        ILoggerFactory _loggerFactory;

        public SetupHomeController(IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<SetupHomeController>();
        }
        public async Task<ActionResult> Index()
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
        public async Task<ActionResult> Index(SetupViewModel setup)
        {
            if (ModelState.IsValid && !SetupHelper.IsDbCreateComplete)
            {
                SetupHelper.ConnectionString = DatabaseFactory.GetConnectionString(_env, setup.Database, setup);
                SetupHelper.SelectedDatabase = setup.Database.ToString();
                SetupHelper.IsDbCreateComplete = SetupHelper.CreateDatabase(_env, setup.Database, setup);               
                SetupHelper.SaveSetup(_env);
                return RedirectToAction("CreateAdmin");
            }
            else
            {
                ModelState.AddModelError("Database","Invalid Data");
            }

            return View();
        }

        public async Task<ActionResult> CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAdmin(AdminViewModel viewModel)
        {

            SetupHelper.InitilizeDatabase();

            var optionBuilder = new DbContextOptionsBuilder<NccDbContext>();
            optionBuilder.UseSqlite(SetupHelper.ConnectionString, options => options.MigrationsAssembly("NetCoreCMS.Web"));

            var nccDbConetxt = new NccDbContext(optionBuilder.Options);
            var userStore = new NccUserStore(nccDbConetxt);
            var identityOptions = Options.Create(new IdentityOptions());
            var passwordHasher = new PasswordHasher<NccUser>();
            var userValidatorList = new List<UserValidator<NccUser>>();
            var passwordValidatorList = new List<PasswordValidator<NccUser>>();
            var lookupNormalizer = new UpperInvariantLookupNormalizer();
            var identityErrorDescriber = new IdentityErrorDescriber();
            var logger = _loggerFactory.CreateLogger<UserManager<NccUser>>();

            var userManager = new UserManager<NccUser>(
                userStore,
                identityOptions,
                passwordHasher,
                userValidatorList,
                passwordValidatorList,
                lookupNormalizer,
                identityErrorDescriber,
                GlobalConfig.App.ApplicationServices,
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
                roleLogger,
                _httpContextAccessor
                );
            nccDbConetxt.Database.Migrate();

            await roleManager.CreateAsync(new NccRole()
            {
                Name = NccCmsRoles.Administrator,
                NormalizedName = NccCmsRoles.Administrator                
            });

            await roleManager.CreateAsync(new NccRole()
            {
                Name = NccCmsRoles.Author,
                NormalizedName = NccCmsRoles.Author
            });

            await roleManager.CreateAsync(new NccRole()
            {
                Name = NccCmsRoles.Contributor,
                NormalizedName = NccCmsRoles.Contributor
            });

            await roleManager.CreateAsync(new NccRole()
            {
                Name = NccCmsRoles.Editor,
                NormalizedName = NccCmsRoles.Editor
            });

            await roleManager.CreateAsync(new NccRole()
            {
                Name = NccCmsRoles.Reader,
                NormalizedName = NccCmsRoles.Reader
            });

            await roleManager.CreateAsync(new NccRole()
            {
                Name = NccCmsRoles.Subscriber,
                NormalizedName = NccCmsRoles.Subscriber
            });

            var admin = new NccUser()
            {
                Email = viewModel.Email,
                FullName = "Administrator",
                Name = "admin",
                UserName = viewModel.AdminUserName

            };
            var result = await userManager.CreateAsync(admin, viewModel.AdminPassword);

            await userManager.AddToRoleAsync(admin, NccCmsRoles.Administrator);

            SetupHelper.IsAdminCreateComplete = true;
            SetupHelper.SaveSetup(_env);

            return RedirectToAction("Success");
            
        }
  
        public ActionResult Success()
        {
            return View();
        }
    }
}
