using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.Comments;
using ScraBoy.Features.CMS.Interest;
using ScraBoy.Features.CMS.Tag;
using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.HomeBlog
{
    [RoutePrefix("root")]
    [Authorize]
    public class HomeBlogController : Controller
    {
        private readonly IPostRepository posRepository;
        private readonly IUserRepository userRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IVotingRepository votingRepository;
        
        private BlogService blogService = new BlogService();

        private readonly int pageSize = 2;
        public HomeBlogController() : this(new PostRepository(), 
            new UserRepository(), new CommentRepository(), new VotingRepository()) { }

        public HomeBlogController(IPostRepository postRepository, 
            IUserRepository userRepositorry,
            ICommentRepository commentRepository,IVotingRepository votingRepository)
        {
            posRepository = postRepository;
            this.userRepository = userRepositorry;
            this.commentRepository = commentRepository;
            this.votingRepository = votingRepository;
        }

        private async Task SetTags()
        {
            ViewBag.Tags =await this.blogService.GetAllTags();
        }
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            await SetTags();

            var blogs = await blogService.GetPageBlogAsync(1,pageSize);

            ViewBag.PreviousPage = 0;
            ViewBag.NextPage = (Decimal.Divide(posRepository.CountPublished,pageSize) > 1) ? 2 : -1;

            return View(blogs);
        }
        [Route("page/{page:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> Page(int page=1)
        {
            await SetTags();

            if(page<2)
            {
                RedirectToAction("Index");
            }
            var blogs = await blogService.GetPageBlogAsync(page,pageSize);

            ViewBag.PreviousPage = page-1;
            ViewBag.NextPage = (Decimal.Divide(posRepository.CountPublished,pageSize) > page) ? page+1 : -1;

            return View("Index",blogs);
        }
   
        // root/posts/post-id
        [HttpGet]
        [Route("posts/{postId}")]
        [AllowAnonymous]
        public async Task<ActionResult> Post(string postId)
        {
            await SetTags();
            var blog = await blogService.GetBlogAsync(postId);

            if(blog == null)
            {
                return HttpNotFound();
            }

            var currentComments = await blogService.GetPostCommentAsync(blog.PostId);

            blog.Comments = currentComments.ToList();
            
            return View(blog);
        }
       

        [HttpPost]
        [Route("posts/{postId}")]
        public async Task<ActionResult> Post(BlogViewModel model, string postId)
        {
            await SetTags();
            var blog = await blogService.GetBlogAsync(postId);

            if(blog == null)
            {
                return HttpNotFound();
            }

            var user = await GetLoggedInUser();
        
            model.UserId = user.Id;
            model.PostId = blog.PostId;

            try
            {
                await commentRepository.CreateAsync(model);

                return RedirectToAction("Post");
            }
            catch(Exception e)
            {
                ModelState.AddModelError("key",e);
                return View(model);
            }

        }
        // root/tags/tag-id
        [Route("tags/{tagId}")]
        [AllowAnonymous]
        public async Task<ActionResult> Tag(string tagId)
        {
            await SetTags();

            var posts = await blogService.GetBlogByTagAsync(tagId);

            if(!posts.Any())
            {
                return HttpNotFound();
            }

            ViewBag.Tag = tagId;

            return View(posts);
        }

       
        private async Task<CMSUser> GetLoggedInUser()
        {
            return await userRepository.GetUserByNameAsync(User.Identity.Name);
        }
    }
}