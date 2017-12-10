using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TokenAuthentication.Models;

namespace TokenAuthentication.Services
{
    public interface IAuthService : IDisposable
    {
        Task<IdentityResult> Register(User user);

        Task<IdentityUser> FindUser(string userName, string password);
    }


    public class AuthService : IAuthService
    {
        private UserManager<IdentityUser> userManager;

        private AuthContext context;

        public AuthService()
        {
            context = new AuthContext();

            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<IdentityResult> Register(User user)
        {
            IdentityUser identityUser = new IdentityUser(user.UserName);

            var result = await userManager.CreateAsync(identityUser, user.Password);

            return result;
        }

        public void Dispose()
        {
            context.Dispose();
            userManager.Dispose();
        }
    }
}