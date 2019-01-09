using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Following
{
    public class FollowRepository : IFollowRepository
    {
        private readonly IUserRepository userRepository;
        private CMSContext db;
        public FollowRepository()
        {
            this.db = new CMSContext();
            this.userRepository = new UserRepository();
        }
        public async Task<IEnumerable<Follow>> GetAll()
        {
            return await this.db.Follow.ToArrayAsync();
        }
        public async Task<IEnumerable<Follow>> GetFollowerByUser(string userId)
        {
            var model = await GetAll();
            return model.Where(a => a.FollowedId.Equals(userId));
        }
        public async Task<IEnumerable<Follow>> GetFollowingByUser(string userId)
        {
            var model = await GetAll();
            return model.Where(a => a.FollowerId.Equals(userId));
        }
        public async Task Create(Follow model,string userId)
        {
            if(model.FollowedId.Equals(model.FollowerId))
            {
                return;
            }

            var follow = await FollowedUser(model.FollowedId,model.FollowerId);

            if(follow!=null)
                this.db.Follow.Remove(follow);
            else
                this.db.Follow.Add(model);

            await this.db.SaveChangesAsync();
        }
        public async Task<Follow> FollowedUser(string followed,string follower)
        {

            var followers = await GetFollowerByUser(followed);

            var result = followers.Where(a => a.FollowerId.Equals(follower)).FirstOrDefault();

            return result;

        }
    }
}