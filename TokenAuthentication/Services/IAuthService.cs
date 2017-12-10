using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;
using TokenAuthentication.Models;

namespace TokenAuthentication.Services
{
    public interface IAuthService : IDisposable
    {
        Task<IdentityResult> Register(User user);

        Task<IdentityUser> FindUser(string userName, string password);
    }
}