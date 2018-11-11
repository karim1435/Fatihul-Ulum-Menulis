using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ScraBoy.Features.CMS.Role
{
    public class RoleRepository : IRoleRepository,IDisposable
    {
        private readonly RoleStore<IdentityRole> store;
        private readonly RoleManager<IdentityRole> manager;

        public RoleRepository()
        {
            store = new RoleStore<IdentityRole>();
            manager = new RoleManager<IdentityRole>(store);
        }
        public async Task<IdentityRole> GetRoleByNameAsync(string name)
        {
            return await store.FindByNameAsync(name);
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await store.Roles.ToArrayAsync();
        }

        public async Task CreateAsync(IdentityRole role)
        {
            await manager.CreateAsync(role);
        }

        private bool _disposed = false;
        public void Dispose()
        {
            if(!_disposed)
            {
                store.Dispose();
                manager.Dispose();
            }
            _disposed = true;
        }

     
    }
}