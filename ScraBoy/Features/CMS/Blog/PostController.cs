using ScraBoy.Features.CMS.ModelBinders;
using ScraBoy.Features.CMS.Topic;
using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.IO;
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
        public PostController(IPostRepository postRepo,IUserRepository userRepo,ICategoryRepository categoryRepository)
        {
            this.postRepository = postRepo;
            this.userRepository = userRepo;
            this.categoryRepositoriy = categoryRepository;
        }
        public PostController() : this(new PostRepository(),new UserRepository(),new CategoryRepository()) { }

        [Route("")]
        public async Task<ActionResult> Index()
        {
            if(!User.IsInRole("author"))
            {
                var posts = this.postRepository.GetPagedList("",1,null);
                return View(posts);
            }

            var user = await GetLoggedInUser();

            var post = this.postRepository.GetPagedList("",1,user.Id);

            return View(post);

        }
        public async Task<ActionResult> ChangePage(int page)
        {
            if(!User.IsInRole("author"))
            {
                var posts = this.postRepository.GetPagedList("",page,null);
                return View("Index",posts);
            }

            var user = await GetLoggedInUser();

            var post = this.postRepository.GetPagedList("",page,user.Id);

            return View("Index",post);
        }
        private async Task SetViewBag()
        {
            var catRepository = new CategoryRepository();
            ViewBag.Categories = await categoryRepositoriy.GetAllCategoriesAsync();
        }

        public string GetFileName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }
        public string GetExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }
        public string GetFullFile(string fileName)
        {
            return GetFileName(fileName) + DateTime.Now.ToString("yymmssfff") + GetExtension(fileName);
        }
        bool CheckFileType(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            switch(ext.ToLower())
            {
                case ".gif":
                    return true;
                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;
                case ".png":
                    return true;
                default:
                    return false;
            }

        }
        private string GetDirPath(string filePath)
        {
            return Path.Combine(Server.MapPath("~/Image/"),filePath);
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
            if(model.ImageFile==null)
            {
                ModelState.AddModelError(String.Empty,"Please Upload image to continue");
                await SetViewBag();
                return View(model);
            }
            var filePath = GetFullFile(model.ImageFile.FileName);

            if(!CheckFileType(filePath))
            {
                ModelState.AddModelError(string.Empty,"Please upload image");
                await SetViewBag();
                return View(model);
            }

            SaveImage(model.ImageFile,filePath);

            model.UrlImage = "~/Image/" + filePath;

            var user = await GetLoggedInUser();

            model.Tags = model.Tags.Select(tag => tag.MakeUrlFriednly()).ToList();
            model.Created = DateTime.Now;
            model.AuthorId = user.Id;
            model.Id = string.Join(model.AuthorId,model.Tags.Distinct()) + "iqro" + "fatihululum" + model.Created.ToString("yymmddss") + model.Title;
            model.Id = model.Id.MakeUrlFriednly();

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
        private void DeleteOldImage(string image)
        {
            var path = Server.MapPath(image);

            if(System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        private void SaveImage(HttpPostedFileBase file,string filePath)
        {
            var pathToSave = GetDirPath(filePath);

            file.SaveAs(pathToSave);
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

                if(post.AuthorId != user.Id)
                {
                    return new HttpUnauthorizedResult();
                }
            }

            return View(post);
        }

        [HttpPost]
        [Route("edit/{postId}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Post model,string postId)
        {
            if(!ModelState.IsValid)
            {
                await SetViewBag();
                return View(model);
            }
            var user = await GetLoggedInUser();

            var post = await postRepository.GetAsync(postId);

            if(User.IsInRole("author"))
            {
                try
                {
                    if(post.AuthorId != user.Id)
                    {
                        return new HttpUnauthorizedResult();
                    }
                }
                catch { }

            }

            if(model.ImageFile != null)
            {
                DeleteOldImage(post.UrlImage);

                var filePath = GetFullFile(model.ImageFile.FileName);

                if(!CheckFileType(filePath))
                {
                    ModelState.AddModelError(string.Empty,"Please upload image");
                    await SetViewBag();
                    return View(model);
                }
                SaveImage(model.ImageFile,filePath);

                model.UrlImage = "~/Image/" + filePath;
            }

            
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
        [Authorize(Roles = "admin, editor")]
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
        public async Task<ActionResult> Delete(string postId,string foo)
        {
            try
            {
                var post = await postRepository.GetAsync(postId);
                DeleteOldImage(post.UrlImage);

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