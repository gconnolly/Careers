using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Careers.Models
{
    public class UserStore : IUserLoginStore<Careers.Models.User>, IUserClaimStore<Careers.Models.User>, IUserRoleStore<Careers.Models.User>, IUserPasswordStore<Careers.Models.User>, IUserSecurityStampStore<Careers.Models.User>, IUserStore<Careers.Models.User>, IDisposable
    {
        private readonly ApplicationDbContext context;

        public UserStore(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task AddLoginAsync(User user, UserLoginInfo login)
        {
            user.UserLogins.Add(new UserLogin
                {
                    UserId = user.Id,
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey
                });

            return context.SaveChangesAsync();
        }

        public Task<User> FindAsync(UserLoginInfo login)
        {
            return Task.Run(() => context.UserLogins.Single(l => l.ProviderKey == login.ProviderKey && l.LoginProvider == login.LoginProvider).User);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            return Task.Run(() => (IList<UserLoginInfo>)user.UserLogins.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList());
        }

        public Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            user.UserLogins.Remove(user.UserLogins.Single(l => l.ProviderKey == login.LoginProvider && l.LoginProvider == login.LoginProvider));
            return context.SaveChangesAsync();
        }

        public Task CreateAsync(User user)
        {
            context.Users.Add(user);
            return context.SaveChangesAsync();
        }

        public Task DeleteAsync(User user)
        {
            context.Users.Remove(user);
            return context.SaveChangesAsync();
        }

        public Task<User> FindByIdAsync(string userId)
        {
            return context.Users.SingleOrDefaultAsync(u => u.Id == userId);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return context.Users.SingleOrDefaultAsync(u => u.UserName == userName);
        }

        public Task UpdateAsync(User user)
        {
            return context.SaveChangesAsync();
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.Run(() => user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.Run(() => user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return context.SaveChangesAsync();
        }

        public Task AddToRoleAsync(User user, string role)
        {
            var roleEntity = context.Roles.SingleOrDefault(r => r.Id == role);
            roleEntity.Users.Add(user);

            return context.SaveChangesAsync();
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            return Task.Run(() => (IList<string>)user.Roles.Select(u => u.Id).ToList());
        }

        public Task<bool> IsInRoleAsync(User user, string role)
        {
            return Task.Run(() => user.Roles.Any(r => r.Id == role));
        }

        public Task RemoveFromRoleAsync(User user, string role)
        {
            user.Roles.Remove(user.Roles.Single( r => r.Id == role ));

            return context.SaveChangesAsync();
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            return Task.Run(() => user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(User user, string stamp)
        {
            user.SecurityStamp = stamp;

            return context.SaveChangesAsync();
        }

        public Task AddClaimAsync(User user, Claim claim)
        {
            user.UserClaims.Add(new UserClaim
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            });

            return context.SaveChangesAsync();
        }

        public Task<IList<Claim>> GetClaimsAsync(User user)
        {
            return Task.Run(() => (IList<Claim>)user.UserClaims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList());
        }

        public Task RemoveClaimAsync(User user, Claim claim)
        {
            user.UserClaims.Remove(user.UserClaims.Single(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value));
            return context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}