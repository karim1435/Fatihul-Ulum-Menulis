using Microsoft.Owin.Security;
using ScraBoy.Features.CMS.Gzip;
using ScraBoy.Features.CMS.Role;
using ScraBoy.Features.CMS.Topic;
using ScraBoy.Features.CMS.Upload;
using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Admin
{
    [RoutePrefix("admin")]
    [Authorize]
    
    public class AdminController : UploadController
    {
        private readonly string pathFolder = "~/Image/profile/";
        private readonly string defaultProfile = "~/Image/profile/default.jpg";
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly UserService userService;
        public AdminController() : this(new UserRepository())
        {
            this.roleRepository = new RoleRepository();
            this.userRepository = new UserRepository();
            this.categoryRepository = new CategoryRepository();
            this.userService = new UserService(ModelState,userRepository,roleRepository);
        }

        public AdminController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        [Route("")]
        [CompressContent]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("login")]
        [AllowAnonymous]
        [CompressContent]
        public async Task<ActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [CompressContent]
        public async Task<ActionResult> Login(LoginViewModel model,string returnUrl)
        {
            var user = await this.userRepository.GetLoginUserAsync(model.UserName,model.Pasword);

            if(user == null)
            {
                ModelState.AddModelError(string.Empty,"The User with the supplied credentials does not exist");
                return View(model);
            }

            var authManager = HttpContext.GetOwinContext().Authentication;

            var userIdentity = await userRepository.CrateIdentityAsync(user);

            authManager.SignIn(new AuthenticationProperties(){
                IsPersistent = model.RememberMe
            },userIdentity);

            await this.userService.LastLoginUpdate(user);

            if(Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index","HomeBlog");
            }
        }
        [AllowAnonymous]
        [Route("register")]
        [CompressContent]
        public ActionResult Register()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [CompressContent]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            model.UrlImage = defaultProfile;
            bool completed = await this.userService.RegisterAsync(model);

            if(completed)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
        [AllowAnonymous]
        [CompressContent]
        public async Task<ActionResult> ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [CompressContent]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var result = await this.userService.ResetPassword(model);

            if(result)
            {
                return RedirectToAction("Login");
            }
            return View(model);
        }
        [Route("logout")]
        [CompressContent]
        public async Task<ActionResult> Logout(string returnUrl)
        {
            var authManager = HttpContext.GetOwinContext().Authentication;

            authManager.SignOut();

            if(Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index","HomeBlog");
            }
        }
        [Route("manage/{username}")]
        [HttpGet]
        [Authorize(Roles = "admin")]
        [CompressContent]
        public async Task<ActionResult> Manage(string username)
        {
            var currentUser = User.Identity.Name;

            if(!User.IsInRole("admin") &&
                !string.Equals(currentUser,username,StringComparison.CurrentCultureIgnoreCase))
            {
                return new HttpUnauthorizedResult();
            }

            var user = await userService.GetUserByNameAsync(username);

            if(user == null)
            {
                return HttpNotFound();
            }

            return View("~/Views/User/Edit.cshtml",user);
        }

        [Route("manage/{username}")]
        [HttpPost]
        [Authorize(Roles = "admin")]
        [CompressContent]
        public async Task<ActionResult> Manage(UserViewModel model,string username)
        {
            var user = await userService.GetUserByNameAsync(username);

            if(user == null)
            {
                return HttpNotFound();
            }

            var currentUser = User.Identity.Name;
            var isAdmin = User.IsInRole("admin");

            if(!isAdmin &&
                !string.Equals(currentUser,username,StringComparison.CurrentCultureIgnoreCase))
            {
                return new HttpUnauthorizedResult();
            }

            if(model.ImageFile != null)
            {
                if(!user.UrlImage.Equals(defaultProfile))
                {
                    DeleteOldImage(user.UrlImage);
                }

                var filePath = GetFullFile(model.ImageFile.FileName);

                if(!CheckFileType(filePath))
                {
                    ModelState.AddModelError(string.Empty,"Upload Image with JPEG, JPG OR PNG Extension");
                    return View(model);
                }

                SaveImage(model.ImageFile,pathFolder,filePath);

                model.UrlImage = pathFolder + filePath;
            }
            else
            {
                model.UrlImage = user.UrlImage;
            }


            var userUpdated = await userService.ManageProfile(model);

            if(userUpdated)
            {
                if(isAdmin)
                {
                    return RedirectToAction("index","User");
                }
            }

            return View("~/Views/User/Edit.cshtml",model);
        }
        [AllowAnonymous]
        public async Task<PartialViewResult> AdminMenu()
        {
            var items = new List<AdminMenuItem>();

            if(User.Identity.IsAuthenticated)
            {
                items.Add(new AdminMenuItem
                {
                    Text = "My Site",
                    Action = "Index",
                    Icon = "fa fa-globe",
                    RouteInfo = new { controller = "HomeBlog" }
                });
                if(User.IsInRole("admin"))
                {
                    items.Add(new AdminMenuItem
                    {
                        Text = "Users",
                        Action = "Index",
                        Icon = "fa fa-user-secret",
                        RouteInfo = new { controller = "user"}
                    });
                    items.Add(new AdminMenuItem
                    {
                        Text = "Alert",
                        Action = "Index",
                        Icon = "fa fa-warning",
                        RouteInfo = new { controller = "Violation" }
                    });
                    items.Add(new AdminMenuItem
                    {
                        Text = "Contest",
                        Action = "ContestList",
                        Icon = "fa fa-trophy",
                        RouteInfo = new { controller = "Competition" }
                    });
                    items.Add(new AdminMenuItem
                    {
                        Text = "Tags",
                        Action = "Index",
                        Icon = "fa fa-tags",
                        RouteInfo = new { controller = "tag" }
                    });
                }
                items.Add(new AdminMenuItem
                {
                    Text = "Posts",
                    Action = "Index",
                    Icon = "fa fa-book",
                    RouteInfo = new { controller = "post"}
                });
                items.Add(new AdminMenuItem
                {
                    Text = "Category",
                    Action = "Index",
                    Icon = "fa fa-thumb-tack",
                    RouteInfo = new { controller = "Category" }
                });
                items.Add(new AdminMenuItem
                {
                    Text = "Comment",
                    Action = "Index",
                    Icon = "fa fa-comment",
                    RouteInfo = new { controller = "comment" }
                });
                items.Add(new AdminMenuItem
                {
                    Text = "Activity",
                    Action = "Index",
                    Icon = "fa fa-history",
                    RouteInfo = new { controller = "Voting" }
                });

                items.Add(new AdminMenuItem
                {
                    Text = "Report",
                    Action = "Index",
                    Icon = "fa fa-tasks",
                    RouteInfo = new { controller = "Report" }
                });
            }
            return PartialView(items);
        }
        [AllowAnonymous]
        public async Task<PartialViewResult> AuthenticationLink()
        {
            var item = new AdminMenuItem
            {
                RouteInfo = new { controller = "admin"}
            };

            if(User.Identity.IsAuthenticated)
            {
                item.Text = "Logout";
                item.Icon = "fa fa-sign-out";
                item.Action = "Logout";
            }
            else
            {
                item.Text = "Login";
                item.Icon = "fa fa-sign-in";
                item.Action = "Login";
            }

            return PartialView(item);
        }

        private bool _isDisposed;
        

        protected override void Dispose(bool disposing)
        {

            if(!_isDisposed)
            {
                userRepository.Dispose();
            }
            _isDisposed = true;

            base.Dispose(disposing);
        }
    }
}