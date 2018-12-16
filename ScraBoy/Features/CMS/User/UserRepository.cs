using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using ScraBoy.Features.Data;

namespace ScraBoy.Features.CMS.User
{
    public class UserRepository : IUserRepository
    {
        private readonly CmsUserStore store;
        private readonly CmsUserManager manager;

        public UserRepository()
        {
            store = new CmsUserStore();
            manager = new CmsUserManager(store);
        }
        public async Task<CMSUser> GetUserById(string userId)
        {
            return await store.FindByIdAsync(userId);
        }
        public async Task<CMSUser> GetUserByNameAsync(string username)
        {
            return await store.FindByNameAsync(username);
        }
        public async Task<IdentityResult> ResetPasswordAsync(string userid, string code, string password)
        {
            return await manager.ResetPasswordAsync(userid,code,password);
        }
    
        public IQueryable<CMSUser> GetPosts(string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                return this.GetAllUsersAsync(name).Where(a => a.UserName.Contains(name)).AsQueryable();
            }
            return store.Users;
        }
        public  IEnumerable<CMSUser> GetAllUsersAsync(string userName)
        {
            return store.Users.Where(a=>a.UserName.Contains(userName)).ToList();
        }
        public async Task CreateAsync(CMSUser user,string password)
        {
            await manager.CreateAsync(user,password);
        }
  
        public async Task DeleteAsync(CMSUser user)
        {
            await manager.DeleteAsync(user);
        }

        public async Task UpdateAsync(CMSUser user)
        {
            await manager.UpdateAsync(user);
        }

        public bool VerifyUserPassword(string hashedPassword,string providedPassword)
        {
            return manager.PasswordHasher.VerifyHashedPassword(hashedPassword,providedPassword) ==
                   PasswordVerificationResult.Success;
        }

        public string HashPassword(string password)
        {
            return manager.PasswordHasher.HashPassword(password);
        }

        public async Task AddUserToRoleAsync(CMSUser user,string role)
        {
            await manager.AddToRoleAsync(user.Id,role);
        }

        public async Task<IEnumerable<string>> GetRolesForUserAsync(CMSUser user)
        {
            return await manager.GetRolesAsync(user.Id);
        }

        public async Task RemoveUserFromRoleAsync(CMSUser user,params string[] roleNames)
        {
            await manager.RemoveFromRolesAsync(user.Id,roleNames);
        }
        public async Task<CMSUser> GetLoginUserAsync(string userName,string pasword)
        {
            return await manager.FindAsync(userName,pasword);
        }
        public async Task<ClaimsIdentity> CrateIdentityAsync(CMSUser user)
        {
            return await manager.CreateIdentityAsync(
                user,DefaultAuthenticationTypes.ApplicationCookie);
        }

        private bool _disposed = false;
        public void Dispose()
        {
            if(!_disposed)
            {
                manager.Dispose();
                store.Dispose();
            }

            _disposed = true;
        }
    }
}