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
    public class HomeBlogController : Controller
    {
        private readonly IPostRepository posRepository;
        private readonly IUserRepository userRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IVotingRepository votingRepository;

        private BlogService blogService = new BlogService();

        private readonly int pageSize = 5;
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
        public async Task<ActionResult> Index(int? page,string currentFilter)
        {
            await SetViewBag();
            int pageNumber = (page ?? 1);

            var blogs = blogService.GetPagedList(currentFilter,"","",pageNumber);

        
            foreach(var blog in blogs)
            {
                blog.Voted = await StatusVote(blog);
            }
            return View("Index","",blogs);
        }
        public async Task<ActionResult> Search(string search)
        {
            await SetViewBag();

            ViewBag.Filter = search;

            var blogs = blogService.GetPagedList(search,"","",1);
            if(blogs.Count() <= 0)
            {
                return View("~/Views/HomeBlog/_NotFoundPage.cshtml");
            }

            foreach(var blog in blogs)
            {
                blog.Voted = await StatusVote(blog);
            }
            return View("Index","",blogs);
        }
        // root/posts/post-id
        [HttpGet]
        [Route("posts/{postId}")]
        public async Task<ActionResult> Post(string postId)
        {
            await SetViewBag();

            var post = await this.posRepository.GetAsync(postId);

            if(post==null)
            {
                return HttpNotFound();
            }
               
            var blog = blogService.GetBlogViewModel(post);

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

            var blog = blogService.GetBlogViewModel(post);

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
        public async Task<ActionResult> Tag(int? page, string tagId)
        {
            int pageNumber = (page ?? 1);
            await SetViewBag();

            var posts = blogService.GetPagedList("",tagId,"",pageNumber);

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
        public async Task<ActionResult> Category(string catId)
        {
            await SetViewBag();

            var posts = blogService.GetPagedList("","",catId,1);

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
        public async Task RecentPost()
        {
            ViewBag.NewPosts = await this.blogService.MostNewPosts();
        }
        private async Task PopularPostByView()
        {
            ViewBag.PopularView = await this.blogService.GetPopularPostByView();
        }
        private async Task SetTags()
        {
            ViewBag.Tags = await this.blogService.GetAllTags();
        }
        private async Task MostLikedPost()
        {
            ViewBag.TopLikes = await this.blogService.MostLiked();
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
            await MostLikedPost();
            await RecentPost();
            await PopularPostByView();
            await SetTags();
            await RecentComments();
            await GetCategories();
            await MostCommented();
        }
        public async Task MostCommented()
        {
            ViewBag.Commented = await blogService.SortByCommented(); 
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