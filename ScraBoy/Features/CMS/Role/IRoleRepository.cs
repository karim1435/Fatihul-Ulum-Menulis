using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraBoy.Features.CMS.Role
{
    public interface IRoleRepository
    {
        Task<IdentityRole> GetRoleByNameAsync(string name);
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task CreateAsync(IdentityRole role);
        void Dispose();
    }
}
