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

        public VotingRepository():this(new CMSContext(), new PostRepository(), new UserRepository())
        {
                
        }
        public VotingRepository(CMSContext db, IPostRepository postRepo, IUserRepository userRepository)
        {
            this.db = db;
            this.postRepository = postRepo;
            this.userRepository = userRepository;
        }
        public async Task<IEnumerable<Voting>> GetAllVotingAsync()
        {
            return await db.Voting.OrderByDescending(a=>a.PostedOn).ToArrayAsync();
        }
        public async Task<VoteViewModel> GetVotedPostUser(string id)
        {
            var votes = await db.Voting.Where(a => a.PostId == id).ToArrayAsync();

            VoteViewModel model = new VoteViewModel();

            model.LikedUser = votes.Where(p => p.LikeCount >= 1).Select(a => a.User.DisplayName).ToList();
            model.DislikedUser= votes.Where(p => p.DislikeCount >= 1).Select(a => a.User.DisplayName).ToList();

            model.TotalLike = model.LikedUser.Count();
            model.TotalDislike = model.DislikedUser.Count();

            return model;
        }
        
        public async Task<bool> UserHasVoted(string postId, string userId)
        {
            var postVoted =await db.Voting.Where(a => a.PostId == postId).ToListAsync();

            return postVoted.Where(a => a.UserId == userId).Count()>=1;

        }

        public async Task LikedAsync(Voting model)
        {
            bool hasVoted =await UserHasVoted(model.PostId,model.UserId);

            if(hasVoted)
            {
                return;
            }

            model.LikeCount+=1;

            await SavesAsync(model);
        }

        public async Task DislikeAsync(Voting model)
        {
            bool hasVoted = await UserHasVoted(model.PostId,model.UserId);

            if(hasVoted)
            {
                return;
            }

            model.DislikeCount += 1;

            await SavesAsync(model);
        }
        private async Task SavesAsync(Voting model)
        {
            model.PostedOn = DateTime.Now;

            this.db.Voting.Add(model);
            await this.db.SaveChangesAsync();
        }
    }
}