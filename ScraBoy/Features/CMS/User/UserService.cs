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
            if(model.Username.Contains(" "))
            {
                modelState.AddModelError(string.Empty,"Cannot contains space");
                return false;
            }
            if(model.Username.Contains("@"))
            {
                modelState.AddModelError(string.Empty,"Cannot contains @");
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
                SlugUrl ="fuuser"+Guid.NewGuid().ToString() + DateTime.Now.ToString("yymmssfff")
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

            if(model.Username.Contains(" "))
            {
                modelState.AddModelError(string.Empty,"Cannot contains space");
                return false;
            }
            if(model.Username.Contains("@"))
            {
                modelState.AddModelError(string.Empty,"Cannot contains @");
                return false;
            }

            if(string.IsNullOrWhiteSpace(model.NewPassword))
            {
                modelState.AddModelError(string.Empty,"Your must type a password");
                return false;
            }

            var newUser = new CMSUser()
            {
                UrlImage = model.UrlImage,
                UserName = model.Username,
                Email = model.Username,
                Born = DateTime.Now,
                DisplayName = model.DisplayName,
                SlugUrl = "fuuser" + Guid.NewGuid().ToString() + DateTime.Now.ToString("yymmssfff")
            };

            await usersRepository.CreateAsync(newUser,model.NewPassword);

            await usersRepository.AddUserToRoleAsync(newUser,model.SelectedRole);

            return true;
        }

        public async Task<IEnumerable<Post>> GetPostByUserId(string userName)
        {
            using(var db = new CMSContext())
            {
                return await db.Post.Include("Author").
                   Where(a => a.Author.UserName.Equals(userName)).
                   Where(p => p.Published < DateTime.Now).
                   OrderByDescending(a => a.Published).
                   ToArrayAsync();
            }
        }
        public async Task<UserProfileModel> GetProfileModel(string userId)
        {
            CMSUser user = await GetUser(userId);

            if(user == null)
            {
                return null;
            }

            var role = await this.usersRepository.GetRolesForUserAsync(user);
            var posts = await GetPostByUserId(user.UserName);

            var userModel = new UserProfileModel();

            userModel.User = user;
            userModel.Role = role.FirstOrDefault();
            userModel.Posts = posts;

            return userModel;
        }
        public async Task<CMSUser> GetUser(string userId)
        {
            using(var db = new CMSContext())
            {
                return await db.Users.Include(a=>a.Posts).Where(a => a.SlugUrl==userId).FirstOrDefaultAsync();
            }
        }
        public List<CMSUser> GetPostsList(string name)
        {
            return this.usersRepository.GetPosts(name).OrderByDescending(a => a.UserName).ToList();
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
                FbProfile=user.FbProfile,
                InstagramProfile=user.InstagramProfile,
                TwitterProfile=user.TwitterProfile,
                UrlImage=user.UrlImage
                
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
            user.InstagramProfile = model.InstagramProfile;
            user.TwitterProfile = model.TwitterProfile;
            user.Bonus += model.Bonus;
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
            user.Bonus += model.Bonus;
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