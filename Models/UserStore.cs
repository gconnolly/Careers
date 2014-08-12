using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Careers.Models
{
    public class UserStore : IUserLoginStore<Careers.Models.User>, IUserRoleStore<Careers.Models.User>, IUserPasswordStore<Careers.Models.User>, IUserStore<Careers.Models.User>, IDisposable
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
            return Task<User>.Run(() => {
                var userLogin = context.UserLogins.SingleOrDefault(l => l.ProviderKey == login.ProviderKey && l.LoginProvider == login.LoginProvider);
                return userLogin != null ? userLogin.User : null;
            });
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
            try
            {
                return context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            
        }

        public Task DeleteAsync(User user)
        {
            context.Users.Remove(user);
            return context.SaveChangesAsync();
        }

        public Task<User> FindByIdAsync(string userId)
        {
            return context.Users.SingleOrDefaultAsync(u => u.Id.ToString() == userId);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return context.Users.SingleOrDefaultAsync(u => u.FullName == userName);
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
            var roleEntity = context.Roles.SingleOrDefault(r => r.Name == role);
            roleEntity.Users.Add(user);

            return context.SaveChangesAsync();
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            return Task.Run(() => (IList<string>)user.Roles.Select(r => r.Name).ToList());
        }

        public Task<bool> IsInRoleAsync(User user, string role)
        {
            return Task.Run(() => user.Roles.Any(r => r.Name == role));
        }

        public Task RemoveFromRoleAsync(User user, string role)
        {
            user.Roles.Remove(user.Roles.Single(r => r.Name == role));

            return context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}