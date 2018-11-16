using Microsoft.Owin.Security;
using ScraBoy.Features.CMS.Role;
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

    public class AdminController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly UserService userService;
        public AdminController() : this(new UserRepository())
        {
            this.roleRepository = new RoleRepository();
            this.userRepository = new UserRepository();

            this.userService = new UserService(ModelState,userRepository,roleRepository);
        }

        public AdminController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
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
        public ActionResult Register()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            bool completed = await this.userService.RegisterAsync(model);

            if(completed)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
       
        [Route("logout")]
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

        [AllowAnonymous]
        public async Task<PartialViewResult> AdminMenu()
        {
            var items = new List<AdminMenuItem>();

            if(User.Identity.IsAuthenticated)
            {
                items.Add(new AdminMenuItem
                {
                    Text = "Home",
                    Action = "Index",
                    Icon = "fa fa-home",
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
                }
                else
                {
                    items.Add(new AdminMenuItem
                    {
                        Text = "Profile",
                        Action = "Edit",
                        Icon = "fa fa-user",
                        RouteInfo = new { controller = "user",username = User.Identity.Name }
                    });
                }

                if(!User.IsInRole("author"))
                {
                    items.Add(new AdminMenuItem
                    {
                        Text = "Tags",
                        Action = "Index",
                        Icon = "fa fa-tags",
                        RouteInfo = new { controller = "tag"}
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
                    Text = "Comment",
                    Action = "Index",
                    Icon = "fa fa-comment-alt",
                    RouteInfo = new { controller = "comment" }
                });
                items.Add(new AdminMenuItem
                {
                    Text = "Category",
                    Action = "Index",
                    Icon = "fa fa-folder-open",
                    RouteInfo = new { controller = "Category" }
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
                item.Action = "Logout";
                item.Icon = "fa fa-sign-out-alt";
            }
            else
            {
                item.Text = "Login";
                item.Icon = "fa fa-bars";
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