using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Core.Auth
{
    public class NccSignInManager<TUser> : SignInManager<TUser> where TUser : class
    {
         public NccSignInManager(UserManager<TUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<TUser>> logger,
            IAuthenticationSchemeProvider schemes
            )
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
            
        }

        public override async Task SignInAsync(TUser user, bool isPersistent, string authenticationMethod = null)
        {
            var userId = await UserManager.GetUserIdAsync(user);
            await base.SignInAsync(user, isPersistent, authenticationMethod);
        }
    }
}
