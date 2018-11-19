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
        public async Task<ActionResult> Index(int? page,string currentFilter)
        {
            int pageNumber = (page ?? 1);

            var users = this.userservice.GetPagedList(currentFilter,pageNumber);

            foreach(var item in users)
            {
                var role = await this.usersRepository.GetRolesForUserAsync(item);
                item.CurrentRole = role.FirstOrDefault();
            }
            return View("Index","",users);
        }
        public async Task<ViewResult> Search(string search)
        {
            ViewBag.Filter = search;

            var users = this.userservice.GetPagedList(search,1);

            return View("Index","",users);
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
        [Route("profile/{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult> Profile(string userId)
        {
            var user = await userservice.GetUser(userId);

            if(user == null)
            {
                return HttpNotFound();
            }

            var profile = await userservice.GetProfileModel(userId);

            return View(profile);
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
            var userUpdated = await this.userservice.UpdateProfile(model);

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