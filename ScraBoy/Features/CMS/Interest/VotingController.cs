using ScraBoy.Features.CMS.Blog;
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
    public class VotingController : Controller
    {
        private readonly IVotingRepository votingRepository;
        private readonly IPostRepository postRepository;
        private readonly IUserRepository userRepository;
        public VotingController():this(new VotingRepository(), new PostRepository(), new UserRepository())
        {
        }
        public VotingController(IVotingRepository votingRepository, IPostRepository postRepository,IUserRepository userRepository)
        {
            this.votingRepository = votingRepository;
            this.postRepository = postRepository;
            this.userRepository = userRepository;
        }
        [Route("")]
        public async Task<ActionResult> Index()
        {
            var votes = await this.votingRepository.GetAllVotingAsync();
            return View(votes);
        }
        [Route("like/{postId}")]
        public async Task<ActionResult> Like(string postId)
        {
            var post =  await postRepository.GetAsync(postId);

            if(post == null)
                return HttpNotFound();

            var user = await GetLoggedInUser();

            var model = new Voting();

            model.UserId = user.Id;
            model.PostId = post.Id;

            await votingRepository.LikedAsync(model);

            return RedirectToAction("Post","HomeBlog",new { postId = postId});
        }

        [Route("dislike/{postId}")]
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

            return RedirectToAction("Post","HomeBlog",new { postId = postId});
        }
        private async Task<CMSUser> GetLoggedInUser()
        {
            return await userRepository.GetUserByNameAsync(User.Identity.Name);
        }
    }
}