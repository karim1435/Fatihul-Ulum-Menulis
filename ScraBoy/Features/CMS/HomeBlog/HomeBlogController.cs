using Microsoft.AspNet.Identity;
using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.Comments;
using ScraBoy.Features.CMS.Gzip;
using ScraBoy.Features.CMS.Interest;
using ScraBoy.Features.CMS.ModelBinders;
using ScraBoy.Features.CMS.Tag;
using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace ScraBoy.Features.CMS.HomeBlog
{
    public class HomeBlogController : Controller
    {
        private readonly IPostRepository posRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IVotingRepository votingRepository;
        private BlogService blogService = new BlogService();

        public HomeBlogController() : this(new PostRepository()
            ,new CommentRepository(),new VotingRepository())
        { }

        public HomeBlogController(IPostRepository postRepository,
            ICommentRepository commentRepository,IVotingRepository votingRepository)
        {
            posRepository = postRepository;
            this.commentRepository = commentRepository;
            this.votingRepository = votingRepository;
        }
        [Route("")]
        [CompressContent]
        public async Task<ActionResult> Index()
        {
            await SetViewBag();

            return View("Index");
        }
        [CompressContent]
        [Route("FuPost/{type}")]
        public async Task<ActionResult> FuPost(string type,int? page,string currentFilter)
        {
            ViewBag.searchType = type;

            await SetViewBag();

            int pageNumber = (page ?? 1);

            var blogs = blogService.GetPagedList(type,currentFilter,"","",pageNumber);


            await StatusVote(blogs.ToList());

            return View("FuPost","",blogs);
        }
        [Route("Search/{type}")]
        [CompressContent]
        public async Task<ActionResult> Search(string type,string search)
        {
            await SetViewBag();

            ViewBag.searchType = type;

            ViewBag.Filter = search;

            var blogs = blogService.GetPagedList(type,search,"","",1);


            await StatusVote(blogs.ToList());

            return View("FuPost","",blogs);
        }
        // root/posts/post-id
        [HttpGet]
        [Route("posts/{postId}",Name = "Post")]
        [CompressContent]
        public async Task<ActionResult> Post(string postId)
        {
            await SetViewBag();

            var post = await this.posRepository.GetAsync(postId);

            if(post == null)
            {
                return HttpNotFound();
            }
            var relatedPosts = await this.blogService.RelatedPosts(post.Id,post.CategoryId);
            ViewBag.RelatedPosts = relatedPosts;


            var blog = blogService.GetBlogViewModel(post);
            await RenderStatusVote(blog);

            var currentComments = await blogService.GetPostCommentAsync(post.Id);

            blog.Comments = currentComments.ToList();

            this.posRepository.GetCookieView(HttpContext);
            await this.posRepository.UpdateViewCount(postId);

            return View(blog);
        }
        [HttpPost]
        [Route("posts/{postId}")]
        [Authorize]
        [CompressContent]
        public async Task<ActionResult> Post(BlogViewModel model,string postId)
        {
            await SetViewBag();

            var post = await this.posRepository.GetAsync(postId);

            if(post == null)
            {
                return HttpNotFound();
            }

            var blog = blogService.GetBlogViewModel(post);

            await RenderStatusVote(blog);

            if(string.IsNullOrWhiteSpace(model.NewComment.Content))
            {
                return RedirectToAction("Post","HomeBlog",new { postId = postId });
            }

            model.NewComment.UserId = UserId;
            model.NewComment.PostId = blog.Post.Id;

            try
            {
                await commentRepository.CreateAsync(model.NewComment);

                return RedirectToAction("Post");
            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty,e.Message);
                return View(model);
            }
        }

        [Route("bycategory/{catId}")]
        [CompressContent]
        public async Task<ActionResult> Category(string catId,int? page,string currentFilter)
        {
            await SetViewBag();

            ViewBag.category = catId;

            int pageNumber = (page ?? 1);

            var posts = blogService.GetPagedList("","","",catId,pageNumber);

            if(posts == null)
            {
                return HttpNotFound();
            }

            await StatusVote(posts.ToList());


            return View("Category","",posts);

        }
        [Route("SearchCategory/{catId}")]
        [CompressContent]
        public async Task<ActionResult> SearchCategory(string catId,string search)
        {
            await SetViewBag();

            ViewBag.category = catId;

            ViewBag.Filter = search;

            var blogs = blogService.GetPagedList("",search,"",catId,1);

            await StatusVote(blogs.ToList());

            return View("Category","",blogs);
        }

        // root/tags/tag-id
        [Route("bytags/{tagId}")]
        [CompressContent]
        public async Task<ActionResult> Tag(string tagId,int? page,string currentFilter)
        {

            await SetViewBag();

            ViewBag.tag = tagId;

            int pageNumber = (page ?? 1);

            var posts = blogService.GetPagedList("","",tagId,"",pageNumber);

            if(posts == null)
            {
                return HttpNotFound();
            }

            await StatusVote(posts.ToList());


            return View("Tag","",posts);
        }
        [CompressContent]
        [Route("SearchTag/{tagId}")]
        public async Task<ActionResult> SearchTag(string tagId,string search)
        {
            await SetViewBag();

            ViewBag.tag = tagId;

            ViewBag.Filter = search;

            var blogs = blogService.GetPagedList("",search,tagId,"",1);

            await StatusVote(blogs.ToList());

            return View("Tag","",blogs);
        }


        [Route("votedBy/{postId}")]
        [CompressContent]
        public async Task<ActionResult> ShowWhoVote(string postId)
        {
            await SetViewBag();
            var post = await this.posRepository.GetAsync(postId);

            if(post == null)
            {
                return HttpNotFound();
            }

            var blog = blogService.GetBlogViewModel(post);

            return View(blog);
        }
        [CompressContent]
        public async Task<ActionResult> Info(int? page)
        {
            int pageNumber = (page ?? 1);
            return View(this.blogService.GetPagedListInfo(pageNumber,User.Identity.GetUserId()));
        }
        public async Task<PartialViewResult> NotificationMenu()
        {
            return PartialView(this.blogService.GetNotification(User.Identity.GetUserId()));
        }
        public async Task<PartialViewResult> CategoryMenu()
        {
            return PartialView(this.blogService.GetAllCategories());
        }
        [Route("bestwriter")]
        [AllowAnonymous]
        [CompressContent]
        public async Task<ActionResult> RankingTopUser()
        {
            await SetViewBag();
            var user = await this.blogService.GetTopContributors();
            return View(user);
        }
        public async Task TopContributor()
        {
            ViewBag.TopUser = await this.blogService.GetTopContributors();
        }
        public async Task RecentPost()
        {
            var blog = await this.blogService.MostNewPosts();
            await StatusVote(blog.ToList());

            ViewBag.NewPosts = blog;
        }
        private async Task PopularPostByView()
        {
            var blog = await this.blogService.GetPopularPostByView();
            await StatusVote(blog.ToList());

            ViewBag.PopularView = blog;
        }
        private async Task SetTags()
        {
            ViewBag.Tags = await this.blogService.GetAllTags();
        }
        private async Task MostLikedPost()
        {
            var blog = await this.blogService.MostLiked();
            await StatusVote(blog.ToList());

            ViewBag.TopLikes = blog;
        }
        private async Task RecentComments()
        {
            var model = await this.blogService.GetRecentCommentsAsycn();
            ViewBag.RecentComments = model.OrderByDescending(a => a.Comment.PostedOn);
        }

        private async Task SetViewBag()
        {
            await MostLikedPost();
            await RecentPost();
            await PopularPostByView();
            await SetTags();
            await RecentComments();
            await MostCommented();
            await TopContributor();
        }

        public async Task MostCommented()
        {
            var blog = await blogService.SortByCommented();
            await StatusVote(blog.ToList());

            ViewBag.Commented = blog;

        }
        private async Task StatusVote(List<BlogViewModel> post)
        {
            foreach(var item in post)
            {
                await RenderStatusVote(item);
            }
        }

        private async Task RenderStatusVote(BlogViewModel item)
        {
            item.Voted = !User.Identity.IsAuthenticated ? false :
                                await this.votingRepository.UserHasLiked(item.Post.Id,UserId);
        }

        public string UserId
        {
            get { return User.Identity.GetUserId(); }
        }
    }
}