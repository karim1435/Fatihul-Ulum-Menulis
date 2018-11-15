using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ScraBoy.Features.CMS.Role;
using ScraBoy.Features.CMS.Topic;
using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.App_Start
{
    public class AuthDBConfig
    {
        public static async Task RegisterAdmin()
        {
            using(var users = new UserRepository())
            {
                var user = await users.GetUserByNameAsync("admin");
                if(user == null)
                {
                    var adminUser = new CMSUser
                    {
                        UserName = "karim",
                        Email = "rim.karim99@gmail.com",
                    };

                    await users.CreateAsync(adminUser,"07051999rim");
                    await users.AddUserToRoleAsync(user,"admin");
                }
                
            }
            
            using(var roles = new RoleRepository())
            {
                if(await roles.GetRoleByNameAsync("admin") == null)
                {
                    await roles.CreateAsync(new IdentityRole("admin"));
                }
                if(await roles.GetRoleByNameAsync("editor") == null)
                {
                    await roles.CreateAsync(new IdentityRole("editor"));
                }
                if(await roles.GetRoleByNameAsync("author") == null)
                {
                    await roles.CreateAsync(new IdentityRole("author"));
                }
            }
        }
    }
}
