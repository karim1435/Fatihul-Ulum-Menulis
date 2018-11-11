using Microsoft.AspNet.Identity;
using ScraBoy.Features.CMS.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.User
{
    //
    [RoutePrefix("user")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepository usersRepository;
        private readonly IRoleRepository rolesRepository;

        private readonly UserService userservice;
        public UserController()
        {
            this.rolesRepository = new RoleRepository();
            this.usersRepository = new UserRepository();

            this.userservice = new UserService(ModelState,usersRepository,rolesRepository);
        }

        [Route("")]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult> Index()
        {
            var users = await this.usersRepository.GetAllUsersAsync();

            return View(users);

        }
    
        [HttpGet]
        [Route("Create")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Create()
        {
            var model = new UserViewModel();

            model.LoadUserRoles(await rolesRepository.GetAllRolesAsync());

            return View(model);
        }

        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Create(UserViewModel model)
        {
            var completed = await userservice.CreateAsync(model);

            model.LoadUserRoles(await rolesRepository.GetAllRolesAsync());

            if(completed)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
        [HttpGet]
        [Route("edit/{username}")]
        [Authorize(Roles = "admin, editor, author")]
        public async Task<ActionResult> Edit(string username)
        {
            var currentUser = User.Identity.Name;

            if(!User.IsInRole("admin") && 
                !string.Equals(currentUser,username,StringComparison.CurrentCultureIgnoreCase))
            {
                return new HttpUnauthorizedResult();
            }

            var user = await userservice.GetUserByNameAsync(username);

            if(user==null)
            {
                return HttpNotFound();
            }
            return View(user);

        }

        [HttpPost]
        [Route("edit/{username}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, editor, author")]
        public async Task<ActionResult> Edit(UserViewModel model, string username)
        {
            var currentUser = User.Identity.Name;

            var isAdmin = User.IsInRole("admin");

            if(!isAdmin && !string.Equals(currentUser,username,StringComparison.CurrentCultureIgnoreCase))
            {
                return new HttpUnauthorizedResult();
            }
            var userUpdated = await this.userservice.UpdateUser(model);

            if(userUpdated)
            {
                if(isAdmin)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("index","admin");
            }

            return View(model);
        }

        [HttpPost]
        [Route("delete/{username}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(string username)
        {
            if(username.Equals("Ainul"))
            {
                ModelState.AddModelError(string.Empty,"Cannot Delete Your Account");
                return View("Index");
            }
            await this.userservice.DeleteAsync(username);

            return RedirectToAction("Index");

        }
        private bool _isDisposed;
        protected override void Dispose(bool disposing)
        {

            if(!_isDisposed)
            {
                usersRepository.Dispose();
                rolesRepository.Dispose();
            }
            _isDisposed = true;

            base.Dispose(disposing);
        }
    }
}