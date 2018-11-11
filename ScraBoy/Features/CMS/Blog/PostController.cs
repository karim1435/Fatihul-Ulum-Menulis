using ScraBoy.Features.CMS.ModelBinders;
using ScraBoy.Features.CMS.Topic;
using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Blog
{
    [RoutePrefix("Post")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository postRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepositoriy;
        public PostController(IPostRepository postRepo, IUserRepository userRepo, ICategoryRepository categoryRepository)
        {
            this.postRepository  = postRepo;
            this.userRepository = userRepo;
            this.categoryRepositoriy = categoryRepository;
        }
        public PostController() : this(new PostRepository(), new UserRepository(), new CategoryRepository()) { }

        [Route("")]
        public async Task<ActionResult> Index()
        {
            if(!User.IsInRole("author"))
            {
                var posts =  await postRepository.GetAllAsync();

                return View(posts);
            }

            var user = await GetLoggedInUser();

            var post = await postRepository.GetPostsByAuthorAsync(user.Id);

            return View(post);
            
        }
        private async Task SetViewBag()
        {
            var catRepository = new CategoryRepository();
            ViewBag.Categories = await categoryRepositoriy.GetAllCategoriesAsync();
        }
        [HttpGet]
        [Route("create")]
        public async Task<ActionResult> Create()
        {
            await SetViewBag();

            return View(new Post());
        }

        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Post model)
        {
           if(!ModelState.IsValid)
            {
                await SetViewBag();

                return View(model);
            }
            var user = await GetLoggedInUser();

            if(string.IsNullOrWhiteSpace(model.Id))
            {
                model.Id = model.Title;
            }

            model.Id = model.Id.MakeUrlFriednly();
            model.Tags = model.Tags.Select(tag => tag.MakeUrlFriednly()).ToList();

            model.Created = DateTime.Now;
            model.AuthorId = user.Id;

            try
            {
                await postRepository.Create(model);

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ModelState.AddModelError("key",e);
                await SetViewBag();
                return View(model);
            }

        }

        [HttpGet]
        [Route("edit/{postId}")]
        public async Task<ActionResult> Edit(string postId)
        {
            await SetViewBag();

            var post = await postRepository.GetAsync(postId);
            
            if(post == null)
            {
                return HttpNotFound();
            }

            if(User.IsInRole("author"))
            {
                var user = await GetLoggedInUser();

                if(post.AuthorId!=user.Id)
                {
                    return new HttpUnauthorizedResult();
                }
            }
            
            return View(post);
        }

        [HttpPost]
        [Route("edit/{postId}")]
        //post id parameters will bind to the url
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Post model,string postId)
        {
            if(!ModelState.IsValid)
            {
                await SetViewBag();
                return View(model);
            }

            if(User.IsInRole("author"))
            {
                var user = await GetLoggedInUser();
                var post = await postRepository.GetAsync(postId);
                try
                {
                    if(post.AuthorId != user.Id)
                    {
                        return new HttpUnauthorizedResult();
                    }
                }
                catch { }
                
            }
            if(string.IsNullOrWhiteSpace(model.Id))
            {
                model.Id = model.Title;
            }

            model.Id = model.Id.MakeUrlFriednly();
            model.Tags = model.Tags.Select(tag => tag.MakeUrlFriednly()).ToList();


            try
            {
                postRepository.Edit(postId,model);

                return RedirectToAction("Index");
            }
            catch(KeyNotFoundException e)
            {
                return HttpNotFound();
            }
            catch(Exception e)
            {
                ModelState.AddModelError("",e.Message);
                await SetViewBag();
                return View(model);
            }
        }

        [HttpGet]
        [Route("delete/{postId}")]
        [Authorize(Roles ="admin, editor")]
        public async Task<ActionResult> Delete(string postId)
        {
            var post = await postRepository.GetAsync(postId);

            if(post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }

        [HttpPost]
        [Route("delete/{postId}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string postId,string foo)
        {
            try
            {
                postRepository.Delete(postId);

                return RedirectToAction("Index");
            }
            catch(KeyNotFoundException e)
            {
                return HttpNotFound();
            }
        }
        private async Task<CMSUser> GetLoggedInUser()
        {
            return await userRepository.GetUserByNameAsync(User.Identity.Name);
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