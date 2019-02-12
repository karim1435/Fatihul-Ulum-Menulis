using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using ScraBoy.Features.CMS.Role;
using ScraBoy.Features.CMS.Admin;
using ScraBoy.Features.Utility;
using ScraBoy.Features.CMS.HomeBlog;
using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.Data;
using System.Data.Entity;
using PagedList;
using System.Text.RegularExpressions;

namespace ScraBoy.Features.CMS.User
{
    public class UserService
    {
        private readonly int pageSize = 10;
        private readonly IUserRepository usersRepository;
        private readonly IRoleRepository rolesRepostitory;
        private readonly ModelStateDictionary modelState;
        public UserService(ModelStateDictionary modelState,
            IUserRepository userRepository,
            IRoleRepository roleReporitoy)
        {
            this.usersRepository = userRepository;
            this.rolesRepostitory = roleReporitoy;
            this.modelState = modelState;
        }
        public async Task<Boolean> RegisterAsync(RegisterViewModel model)
        {
            if(!modelState.IsValid)
            {
                return false;
            }

            var existingUser = await this.usersRepository.GetUserByNameAsync(model.Username);

            if(existingUser != null)
            {
                modelState.AddModelError(string.Empty,"The user already exists");
                return false;
            }
            if(!Regex.IsMatch(model.Username,@"^[a-zA-Z0-9]+$"))
            {
                modelState.AddModelError(string.Empty,"Username must be letter and number only");
                return false;
            }
            if(string.IsNullOrWhiteSpace(model.Password))
            {
                modelState.AddModelError(string.Empty,"Your must type a password");
                return false;
            }

            var newUser = new CMSUser()
            {
                UserName = model.Username,
                Email = model.Username,
                Born = DateTime.Now,
                DisplayName = model.DisplayName,
                UrlImage = model.UrlImage,
                Security = model.Security,
                RegistrationDate = DateTime.Now,
                LastLoginTime = DateTime.Now,
                SlugUrl = "fuuser" + Guid.NewGuid().ToString() + DateTime.Now.ToString("yymmssfff")
            };

            await usersRepository.CreateAsync(newUser,model.Password);

            await usersRepository.AddUserToRoleAsync(newUser,"Author");

            return true;
        }
        public async Task<bool> CreateAsync(UserViewModel model)
        {
            if(!modelState.IsValid)
            {
                return false;
            }

            var existingUser = await this.usersRepository.GetUserByNameAsync(model.Username);

            if(existingUser != null)
            {
                modelState.AddModelError(string.Empty,"The user already exists");
                return false;
            }
            if(!Regex.IsMatch(model.Username,@"^[a-zA-Z0-9]+$"))
            {
                modelState.AddModelError(string.Empty,"Username must be letter and number only");
                return false;
            }
            if(string.IsNullOrWhiteSpace(model.NewPassword))
            {
                modelState.AddModelError(string.Empty,"Your must type a password");
                return false;
            }
            if(model.NewPassword.Length<6)
            {
                modelState.AddModelError(string.Empty,"Password Length must be greater than 6 ");
                return false;
            }

            var newUser = new CMSUser()
            {
                UrlImage = model.UrlImage,
                UserName = model.Username,
                Email = model.Username,
                Born = DateTime.Now,
                RegistrationDate = DateTime.Now,
                Security = model.Security,
                LastLoginTime = DateTime.Now,
                DisplayName = model.DisplayName,
                SlugUrl = "fuuser" + Guid.NewGuid().ToString() + DateTime.Now.ToString("yymmssfff")
            };

            await usersRepository.CreateAsync(newUser,model.NewPassword);

            await usersRepository.AddUserToRoleAsync(newUser,model.SelectedRole);

            return true;
        }
     
        public List<CMSUser> GetPostsList(string name)
        {
            return this.usersRepository.GetPosts(name).OrderByDescending(a => a.RegistrationDate).ToList();
        }

        public IPagedList<CMSUser> GetPagedList(string search,int currentPage)
        {
            var model = new List<CMSUser>();
            model = GetPostsList(search);

            return model.ToPagedList(currentPage,pageSize);
        }
        public async Task<UserViewModel> GetUserByNameAsync(string name)
        {
            var user = await this.usersRepository.GetUserByNameAsync(name);

            if(user == null)
            {
                return null;
            }

            var viewModel = new UserViewModel()
            {
                Username = user.UserName,
                Email = user.Email,
                Description = user.Description,
                DisplayName = user.DisplayName,
                Born = user.Born,
                FbProfile = user.FbProfile,
                Security = user.Security,
                InstagramProfile = user.InstagramProfile,
                TwitterProfile = user.TwitterProfile,
                UrlImage = user.UrlImage

            };

            var userRoles = await usersRepository.GetRolesForUserAsync(user);

            viewModel.SelectedRole = userRoles.Count() > 0 ?
                userRoles.FirstOrDefault() : userRoles.FirstOrDefault();

            viewModel.LoadUserRoles(await rolesRepostitory.GetAllRolesAsync());

            return viewModel;
        }
        public async Task<bool> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await usersRepository.GetUserByNameAsync(model.Username);

            if(user == null)
            {
                modelState.AddModelError(string.Empty,"The specified user does not exist.");
                return false;
            }

            if(!modelState.IsValid)
            {
                return false;
            }

            if(!user.Security.Equals(model.Security))
            {
                modelState.AddModelError(string.Empty,"Your security question doesn't correct");
                return false;
            }
            if(string.IsNullOrWhiteSpace(model.Password))
            {
                modelState.AddModelError(string.Empty,"The password must be supplied");
                return false;
            }
            var newHashedPassword = usersRepository.HashPassword(model.Password);

            user.PasswordHash = newHashedPassword;

            await usersRepository.UpdateAsync(user);
            return true;

        }
        public async Task<bool> ManageProfile(UserViewModel model)
        {
            var user = await usersRepository.GetUserByNameAsync(model.Username);

            if(user == null)
            {
                modelState.AddModelError(string.Empty,"The specified user does not exist.");
                return false;
            }

            if(!modelState.IsValid)
            {
                return false;
            }

            user.Email = model.Email;
            user.Description = model.Description;
            user.Born = model.Born;
            user.DisplayName = model.DisplayName;
            user.UrlImage = model.UrlImage;
            user.FbProfile = model.FbProfile;
            user.Security = model.Security;
            user.InstagramProfile = model.InstagramProfile;
            user.TwitterProfile = model.TwitterProfile;
            await usersRepository.UpdateAsync(user);

            var roles = await usersRepository.GetRolesForUserAsync(user);

            await usersRepository.RemoveUserFromRoleAsync(user,roles.ToArray());

            await usersRepository.AddUserToRoleAsync(user,model.SelectedRole);

            return true;
        }
        public async Task LastLoginUpdate(CMSUser user)
        {
            var model = await this.usersRepository.GetUserByNameAsync(user.UserName);
            model.LastLoginTime = DateTime.Now;
            await usersRepository.UpdateAsync(user);
        }
        public async Task<bool> UpdateProfile(UserViewModel model)
        {
            var user = await this.usersRepository.GetUserByNameAsync(model.Username);

            user.Description = model.Description;
            user.Email = model.Email;
            user.Born = model.Born;
            user.DisplayName = model.DisplayName;
            user.UrlImage = model.UrlImage;
            user.FbProfile = model.FbProfile;
            user.InstagramProfile = model.InstagramProfile;
            user.TwitterProfile = model.TwitterProfile;
            await usersRepository.UpdateAsync(user);

            return true;
        }

        public async Task DeleteAsync(string username)
        {
            var user = await this.usersRepository.GetUserByNameAsync(username);
            if(user == null)
            {
                return;
            }
            await usersRepository.DeleteAsync(user);
        }
    }
}