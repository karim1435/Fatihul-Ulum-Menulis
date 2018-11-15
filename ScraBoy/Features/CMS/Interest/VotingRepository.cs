using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.HomeBlog;
using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Data;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.CMS.Interest
{
    public class VotingRepository : IVotingRepository
    {
        private CMSContext db;
        private readonly IPostRepository postRepository;
        private IUserRepository userRepository;

        public VotingRepository() : this(new CMSContext(),new PostRepository(),new UserRepository())
        {

        }
        public VotingRepository(CMSContext db,IPostRepository postRepo,IUserRepository userRepository)
        {
            this.db = db;
            this.postRepository = postRepo;
            this.userRepository = userRepository;
        }
        public async Task<IEnumerable<Voting>> GetAllVotingAsync()
        {
            return await db.Voting.OrderByDescending(a => a.PostedOn).ToArrayAsync();
        }
        public async Task<VoteViewModel> GetVotedPostUser(string id)
        {
            var votes = await db.Voting.Where(a => a.PostId == id).ToArrayAsync();

            VoteViewModel model = new VoteViewModel();

            model.LikedUser = votes.Where(p => p.LikeCount ==true).Select(a => a.User.UserName).ToList();

            model.TotalLike = model.LikedUser.Count();

            return model;
        }

        public async Task<Voting> UserHasVoted(string postId,string userId)
        {
            var postVoted = db.Voting.Where(a => a.PostId == postId);

            return await postVoted.Where(a => a.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<bool> UserHasLiked(string postId, string userId)
        {
            var vote = await UserHasVoted(postId,userId);

            if(vote == null)
                return false;

            return vote.LikeCount;
        }

        public async Task LikedAsync(Voting model)
        {
            var vote = await UserHasVoted(model.PostId,model.UserId);

            if(vote!=null)
            {
                vote.LikeCount = !vote.LikeCount;
            }
            if(vote == null)
            {
                model.LikeCount = true;
                model.PostedOn = DateTime.Now;
                this.db.Voting.Add(model);   
            }

            await this.db.SaveChangesAsync();

        }

        public async Task DislikeAsync(Voting model)
        {
            var vote = await UserHasVoted(model.PostId,model.UserId);

            if(vote != null)
            {
                vote.DislikeCount = !vote.LikeCount;
            }
            if(vote == null)
            {
                model.DislikeCount = true;
                model.PostedOn = DateTime.Now;
                this.db.Voting.Add(model);
            }

            await this.db.SaveChangesAsync();

        }

    }
}