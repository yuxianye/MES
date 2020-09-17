using System.Web;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;

using OSharp.Core.Dependency;
using OSharp.Core.Security;
using Solution.Core.Identity;
using Solution.Core.Models.Identity;
using Solution.Core.OAuth;

using Owin;


namespace Solution.Core.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSolutionServices(this IServiceCollection services, IAppBuilder app)
        {
            //Identity
            services.AddScoped<RoleManager<Role, int>, RoleManager>();
            services.AddScoped<UserManager<User, int>, UserManager>();
            services.AddScoped<SignInManager<User, int>, SignInManager>();
            services.AddScoped<IAuthenticationManager>(_ => HttpContext.Current.GetOwinContext().Authentication);
            services.AddScoped<IDataProtectionProvider>(_ => app.GetDataProtectionProvider());

            //Security
            //services.AddScoped<FunctionMapStore>();
            //services.AddScoped<EntityMapStore>();

            //OAuth
            services.AddScoped<OAuthClientStore>();
            services.AddScoped<IOAuthClientRefreshTokenStore, OAuthClientRefreshTokenStore>();
        }
    }
}
