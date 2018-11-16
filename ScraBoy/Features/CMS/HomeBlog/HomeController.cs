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
            new UserRepository(),new CommentRepository(),new VotingRepository())
        { }

        public HomeBlogController(IPostRepository postRepository,
            IUserRepository userRepositorry,
            ICommentRepository commentRepository,IVotingRepository votingRepository)
        {
            posRepository = postRepository;
            this.userRepository = userRepositorry;
            this.commentRepository = commentRepository;
            this.votingRepository = votingRepository;
        }


        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            await SetViewBag();

            var blogs = await blogService.GetPageBlogAsync(1,pageSize);

            foreach(var blog in blogs)
            {
                blog.Voted = await StatusVote(blog);
            }

            ViewBag.PreviousPage = 0;
            ViewBag.NextPage = (Decimal.Divide(posRepository.CountPublished,pageSize) > 1) ? 2 : -1;

            return View(blogs);
        }
        [Route("page/{page:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> Page(int page = 1)
        {
            await SetViewBag();

            if(page < 2)
            {
                RedirectToAction("Index");
            }
            var blogs = await blogService.GetPageBlogAsync(page,pageSize);

            foreach(var blog in blogs)
            {
                blog.Voted = await StatusVote(blog);
            }

            ViewBag.PreviousPage = page - 1;
            ViewBag.NextPage = (Decimal.Divide(posRepository.CountPublished,pageSize) > page) ? page + 1 : -1;

            return View("Index",blogs);
        }

        // root/posts/post-id
        [HttpGet]
        [Route("posts/{postId}")]
        [AllowAnonymous]
        public async Task<ActionResult> Post(string postId)
        {
            await SetViewBag();

            var post = await this.posRepository.GetAsync(postId);

            if(post==null)
            {
                return HttpNotFound();
            }
               
            var blog = await blogService.GetBlogViewModel(post);

            blog.Voted = await StatusVote(blog);
            var currentComments = await blogService.GetPostCommentAsync(blog.Post.Id);

            this.posRepository.GetCookieView(HttpContext);
            await this.posRepository.UpdateViewCount(postId);

            blog.Comments = currentComments.ToList();



            return View(blog);
        }


        [HttpPost]
        [Route("posts/{postId}")]
        [Authorize]
        public async Task<ActionResult> Post(BlogViewModel model,string postId)
        {
            await SetViewBag();

            var post = await this.posRepository.GetAsync(postId);

            if(post == null)
            {
                return HttpNotFound();
            }

            var blog = await blogService.GetBlogViewModel(post);

            blog.Voted = await StatusVote(blog);

            var user = await GetLoggedInUser();

            model.User.Id = user.Id;
            model.Post.Id = blog.Post.Id;

            try
            {
                await commentRepository.CreateAsync(model);

                return RedirectToAction("Post");
            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty,e.Message);
                return View(model);
            }

        }


        // root/tags/tag-id
        [Route("tags/{tagId}")]
        [AllowAnonymous]
        public async Task<ActionResult> Tag(string tagId)
        {
            await SetViewBag();

            var posts = await blogService.GetBlogByTagAsync(tagId);

            foreach(var blog in posts)
            {
                blog.Voted = await StatusVote(blog);
            }

            if(!posts.Any())
            {
                return HttpNotFound();
            }

            ViewBag.Tag = tagId;

            return View(posts);
        }

        [Route("category/{catId}")]
        [AllowAnonymous]
        public async Task<ActionResult> Category(string catId)
        {
            await SetViewBag();

            var posts = await blogService.GetBlogByCategoryAsync(catId);

            foreach(var blog in posts)
            {
                blog.Voted = await StatusVote(blog);
            }
            if(!posts.Any())
            {
                return HttpNotFound();
            }

            ViewBag.category = catId;

            return View(posts);
        }

        private async Task SetTags()
        {
            ViewBag.Tags = await this.blogService.GetAllTags();
        }

        private async Task RecentComments()
        {
            ViewBag.RecentComments = await this.blogService.GetRecentCommentsAsycn();
        }
        private async Task GetCategories()
        {
            ViewBag.Categories = await this.blogService.GetAllCategories();
        }
        private async Task SetViewBag()
        {
            await SetTags();
            await RecentComments();
            await GetCategories();
        }
        private async Task<bool> StatusVote(BlogViewModel post)
        {
            if(!User.Identity.IsAuthenticated)
            {
                return false;
            }
            var user = await GetLoggedInUser();

            return await this.votingRepository.UserHasLiked(post.Post.Id,user.Id);
        }
        private async Task<CMSUser> GetLoggedInUser()
        {
            return await userRepository.GetUserByNameAsync(User.Identity.Name);
        }
    }
}