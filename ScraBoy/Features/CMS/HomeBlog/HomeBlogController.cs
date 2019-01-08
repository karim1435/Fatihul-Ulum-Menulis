using Microsoft.AspNet.Identity;
using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.Comments;
using ScraBoy.Features.CMS.Gzip;
using ScraBoy.Features.CMS.Interest;
using ScraBoy.Features.CMS.ModelBinders;
using ScraBoy.Features.CMS.Tag;
using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Data;
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
        private readonly IUserRepository userRepository;
        private BlogService blogService = new BlogService();

        public HomeBlogController() : this(new PostRepository()
            ,new CommentRepository(),new UserRepository())
        { }

        public HomeBlogController(IPostRepository postRepository,
            ICommentRepository commentRepository,IUserRepository userRepository)
        {
            posRepository = postRepository;
            this.commentRepository = commentRepository;
            this.userRepository = userRepository;
        }
        [Route("")]
        [CompressContent]
        public async Task<ActionResult> Index()
        {
            await SetViewBag();

            await RecentComments();
            await TopContributor();

            return View("Index");
        }
        [Route("FuPost/{type}")]
        [CompressContent]
        public async Task<ActionResult> FuPost(string type,int? page,string currentFilter)
        {
            ViewBag.searchType = type;

            await SetViewBag();

            int pageNumber = (page ?? 1);

            var blogs = blogService.GetPagedList(type,currentFilter,"","",pageNumber);

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

            this.posRepository.GetCookieView(HttpContext);
            await this.posRepository.UpdateViewCount(postId);

            return View(post);
        }
        [HttpPost]
        [Route("posts/{postId}")]
        [Authorize]
        [CompressContent]
        public async Task<ActionResult> Post(Post model,string postId)
        {
            await SetViewBag();

            var post = await this.posRepository.GetAsync(postId);

            if(post == null)
            {
                return HttpNotFound();
            }

            if(string.IsNullOrWhiteSpace(model.NewComment.Content))
            {
                return RedirectToAction("Post","HomeBlog",new { postId = postId });
            }

            model.NewComment.UserId = UserId;
            model.NewComment.PostId = post.Id;

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

            return View("Tag","",posts);
        }
        [Route("SearchTag/{tagId}")]
        [CompressContent]
        public async Task<ActionResult> SearchTag(string tagId,string search)
        {
            await SetViewBag();

            ViewBag.tag = tagId;

            ViewBag.Filter = search;

            var blogs = blogService.GetPagedList("",search,tagId,"",1);

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
            return View(post);
        }
        [CompressContent]
        public async Task<ActionResult> Info(int? page)
        {
            int pageNumber = (page ?? 1);
            return View(this.blogService.GetPagedListInfo(pageNumber,User.Identity.GetUserId()));
        }
        public async Task<PartialViewResult> NotificationMenu()
        {
            return PartialView(this.blogService.GetNotification(UserId));
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
            var user = await this.blogService.GetTopContributors();
            return View(user);
        }
        public async Task TopContributor()
        {
            ViewBag.TopUser = await this.blogService.GetTopContributors();
        }
        private async Task RecentComments()
        {
            var model = await this.blogService.GetRecentCommentsAsycn();
            ViewBag.RecentComments = model;
        }

        private async Task SetViewBag()
        {
            var blog = await this.blogService.GetAllPost();

            ViewBag.TopLikes = blog.Where(a => a.TotalVote >= 1).OrderByDescending(a => a.TotalVote); ;
            ViewBag.NewPosts = blog.OrderByDescending(a => a.Published); ;
            ViewBag.PopularView = blog.Where(a => a.TotalViews >= 1).OrderByDescending(a => a.TotalViews);
            ViewBag.Commented = blog.Where(a => a.TotalComment >= 1).OrderByDescending(a => a.TotalComment);
        }
        [Route("profile/{userId}")]
        [AllowAnonymous]
        [CompressContent]
        public async Task<ActionResult> Profile(string userId)
        {
            var user = await userRepository.GetUserBySlug(userId);

            if(user == null)
            {
                return HttpNotFound();
            }

            var profile = await blogService.GetProfileModel(userId);

            return View(profile);
        }

        public string UserId
        {
            get { return User.Identity.GetUserId(); }
        }
    }
}