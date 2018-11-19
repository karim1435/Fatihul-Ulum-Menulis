using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ScraBoy.Features.CMS.User
{
    public interface IUserRepository : IDisposable
    {
        Task<CMSUser> GetUserById(string userId);
        Task<CMSUser> GetUserByNameAsync(string username);
        Task CreateAsync(CMSUser user,string password);
        Task DeleteAsync(CMSUser user);
        Task UpdateAsync(CMSUser user);
        bool VerifyUserPassword(string hashedPassword,string providedPassword);
        string HashPassword(string password);
        Task AddUserToRoleAsync(CMSUser newUser,string role);
        Task<IEnumerable<string>> GetRolesForUserAsync(CMSUser user);
        Task RemoveUserFromRoleAsync(CMSUser user,params string[] roleNames);
        Task<CMSUser> GetLoginUserAsync(string userName,string pasword);
        Task<ClaimsIdentity> CrateIdentityAsync(CMSUser user);
        IQueryable<CMSUser> GetPosts(string name);
        Task<IdentityResult> ResetPasswordAsync(string userid,string code,string password);

    }
}
