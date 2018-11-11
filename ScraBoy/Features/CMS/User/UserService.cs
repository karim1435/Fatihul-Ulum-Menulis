using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using ScraBoy.Features.CMS.Role;
using ScraBoy.Features.CMS.Admin;

namespace ScraBoy.Features.CMS.User
{
    public class UserService
    {
        private readonly IUserRepository usersRepository;
        private readonly IRoleRepository rolesRepostitory;
        private readonly ModelStateDictionary modelState;
        public UserService(ModelStateDictionary modelState,IUserRepository userRepository,IRoleRepository roleReporitoy)
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

            var existingUser = await this.usersRepository.GetUserByNameAsync(model.Email);

            if(existingUser != null)
            {
                modelState.AddModelError(string.Empty,"The user already exists");
                return false;
            }

            if(string.IsNullOrWhiteSpace(model.Password))
            {
                modelState.AddModelError(string.Empty,"Your must type a password");
                return false;
            }

            var newUser = new CMSUser()
            {
                DisplayName = model.DisplayName,
                UserName = model.Email,
                Email = model.Email
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

            var existingUser = await this.usersRepository.GetUserByNameAsync(model.UserName);

            if(existingUser != null)
            {
                modelState.AddModelError(string.Empty,"The user already exists");
                return false;
            }

            if(string.IsNullOrWhiteSpace(model.NewPassword))
            {
                modelState.AddModelError(string.Empty,"Your must type a password");
                return false;
            }

            var newUser = new CMSUser()
            {
                DisplayName = model.DisplayName,
                UserName = model.UserName,
                Email = model.Email
            };

            await usersRepository.CreateAsync(newUser,model.NewPassword);

            await usersRepository.AddUserToRoleAsync(newUser,model.SelectedRole);

            return true;
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
                UserName = user.UserName,
                Email = user.Email,
                DisplayName = user.DisplayName
            };

            var userRoles = await usersRepository.GetRolesForUserAsync(user);

            viewModel.SelectedRole = userRoles.Count() > 0 ?
                userRoles.FirstOrDefault() : userRoles.SingleOrDefault();

            viewModel.LoadUserRoles(await rolesRepostitory.GetAllRolesAsync());

            return viewModel;
        }

        public async Task<bool> UpdateUser(UserViewModel model)
        {
            var user = await this.usersRepository.GetUserByNameAsync(model.UserName);

            if(user == null)
            {
                modelState.AddModelError(string.Empty,"The specified user does not ecist.");
                return false;
            }

            if(!modelState.IsValid)
            {
                return false;
            }

            if(!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                if(string.IsNullOrWhiteSpace(model.CurrentPassword))
                {
                    modelState.AddModelError(string.Empty,"The current password must be supplied");
                    return false;
                }

                bool passwordVerified = usersRepository.VerifyUserPassword(user.PasswordHash,model.CurrentPassword);

                if(!passwordVerified)
                {
                    modelState.AddModelError(string.Empty,"The current password does not match our records");
                    return false;
                }

                var newHashedPassword = usersRepository.HashPassword(model.NewPassword);

                user.PasswordHash = newHashedPassword;
            }

            user.Email = model.Email;
            user.DisplayName = model.DisplayName;

            await usersRepository.UpdateAsync(user);

            var roles = await usersRepository.GetRolesForUserAsync(user);

            await usersRepository.RemoveUserFromRoleAsync(user,roles.ToArray());

            await usersRepository.AddUserToRoleAsync(user,model.SelectedRole);

            return true;
        }

        public async Task DeleteAsync(string username)
        {
            var user = await this.usersRepository.GetUserByNameAsync(username);
            if(user==null)
            {
                return;
            }
            await usersRepository.DeleteAsync(user);
        }
    }
}