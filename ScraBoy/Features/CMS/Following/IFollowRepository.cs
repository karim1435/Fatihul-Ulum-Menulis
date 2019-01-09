using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.CMS.Following
{
    public interface IFollowRepository
    {
        Task Create(Follow model, string userId);
        Task<IEnumerable<Follow>> GetFollowerByUser(string userId);
        Task<IEnumerable<Follow>> GetFollowingByUser(string userId);
        Task<Follow> FollowedUser(string followed,string follower);
    }
}