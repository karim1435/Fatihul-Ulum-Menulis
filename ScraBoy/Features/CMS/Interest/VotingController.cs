using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.Gzip;
using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Interest
{
    [RoutePrefix("voting")]
    [Authorize]
    public class VotingController : Controller
    {
        private readonly IVotingRepository votingRepository;
        private readonly IPostRepository postRepository;
        private readonly IUserRepository userRepository;
        public VotingController() : this(new VotingRepository(),new PostRepository(),new UserRepository())
        {
        }
        public VotingController(IVotingRepository votingRepository,IPostRepository postRepository,IUserRepository userRepository)
        {
            this.votingRepository = votingRepository;
            this.postRepository = postRepository;
            this.userRepository = userRepository;
        }
        [Route("")]
        [CompressContent]
        public async Task<ActionResult> Index(int? page,string currentFilter)
        {
            int pageNumber = (page ?? 1);

            if(!User.IsInRole("author"))
            {
                return View("Index","",this.votingRepository.GetPagedList(currentFilter,pageNumber,null));
            }

            var user = await GetLoggedInUser();

            return View("Index","",this.votingRepository.GetPagedList(currentFilter,pageNumber,user.Id));

        }
        [CompressContent]
        public async Task<ViewResult> Search(string search)
        {
            ViewBag.Filter = search;

            if(!User.IsInRole("author"))
            {
                return View("Index","",this.votingRepository.GetPagedList(search,1,null));
            }

            var user = await GetLoggedInUser();

            return View("Index","",this.votingRepository.GetPagedList(search,1,user.Id));
        }

        [Route("like/{postId}")]
        [Authorize]
        [CompressContent]
        public async Task<ActionResult> Like(string postId)
        {
            var post = await postRepository.GetAsync(postId);

            if(post == null)
                return HttpNotFound();

            var user = await GetLoggedInUser();

            var model = new Voting();

            model.UserId = user.Id;
            model.PostId = post.Id;

            await votingRepository.LikedAsync(model);

            return RedirectToAction("Post","HomeBlog",new { postId=postId});
        }

        [Route("dislike/{postId}")]
        [CompressContent]
        public async Task<ActionResult> Dislike(string postId)
        {
            var post = await postRepository.GetAsync(postId);

            if(post == null)
                return HttpNotFound();

            var user = await GetLoggedInUser();

            var model = new Voting();

            model.UserId = user.Id;
            model.PostId = post.Id;

            await votingRepository.DislikeAsync(model);

            return RedirectToAction("Post","HomeBlog",new { postId = postId });
        }
        private async Task<CMSUser> GetLoggedInUser()
        {
            return await userRepository.GetUserByNameAsync(User.Identity.Name);
        }
    }
}