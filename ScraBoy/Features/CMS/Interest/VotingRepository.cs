using PagedList;
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
        private readonly int pageSize = 10;
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
        public IQueryable<Voting> GetVotings(string name)
        {
            if(!string.IsNullOrWhiteSpace(name))
            {
                return this.db.Voting.Include("User").Include("Post").Where(a=>a.LikeCount==true).
                    Where(a => a.Post.Title.Contains(name) ||
                a.User.UserName.Equals(name));
            }
            return this.db.Voting.Include("User").Include("Post").Where(a => a.LikeCount == true);
        }
        public List<Voting> GetVotingList(string name)
        {
            return GetVotings(name).OrderByDescending(a => a.PostedOn).ToList();
        }

        public IPagedList<Voting> GetPagedList(string search,int page,string userId)
        {
            var model = new List<Voting>();

            if(userId == null)
            {
                model = GetVotingList(search).ToList();
            }
            else
            {
                model = GetVotingList(search).Where(post => post.Post.AuthorId.Equals(userId)).ToList();
            }

            return model.ToPagedList(page,pageSize);
        }
        public List<Voting> UserLiked(string id)
        {
            var votes = db.Voting.Where(a => a.PostId == id).ToList();

            return votes.Where(a => a.LikeCount == true).ToList();
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
                this.db.Voting.Remove(vote);
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

        public IEnumerable<Voting> GetAllVoting()
        {
            return db.Voting.Include("User").Include("Post").OrderByDescending(a => a.PostedOn).ToList();
        }
    }
}