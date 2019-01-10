using Microsoft.AspNet.Identity;
using ScraBoy.Features.CMS.Gzip;
using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Following
{
    [Authorize]
    public class FollowController : Controller
    {
        private IUserRepository userRepository;
        private IFollowRepository followRepository;
        public FollowController()
        {
            this.userRepository = new UserRepository();
            this.followRepository = new FollowRepository();
        }
        [Route("Follower/{slugUrl}")]
        [CompressContent]
        public async Task<ActionResult> Follower(string slugUrl)
        {
            var user = await userRepository.GetUserBySlug(slugUrl);

            if(user==null)
            {
                return HttpNotFound();
            }

            var followers = await this.followRepository.GetFollowerByUser(user.Id);
            return View(followers);

        }
        [Route("Following/{slugUrl}")]
        [CompressContent]
        public async Task<ActionResult> Following(string slugUrl)
        {
            var user = await userRepository.GetUserBySlug(slugUrl);

            if(user == null)
            {
                return HttpNotFound();
            }

            var following = await this.followRepository.GetFollowingByUser(user.Id);

            return View(following);

        }

        [Route("Follow/{slugUrl}")]
        [CompressContent]
        public async Task<ActionResult> Follow(string slugUrl)
        {
            var user = await userRepository.GetUserBySlug(slugUrl);


            var follower = UserId;

            var model = new Follow();

            model.FollowerId = UserId;
            model.FollowedId = user.Id;
            model.FollowedOn = DateTime.Now;

            await this.followRepository.Create(model,user.Id);

            return RedirectToAction("Profile","HomeBlog",new { userId = user.Id });
        }

        public string UserId
        {
            get { return User.Identity.GetUserId(); }
        }
    }
}