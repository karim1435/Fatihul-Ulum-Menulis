using ScraBoy.Features.CMS.HomeBlog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.CMS.Interest
{ 
    public interface IVotingRepository
    {
        Task<IEnumerable<Voting>> GetAllVotingAsync();
        Task LikedAsync(Voting model);
        Task<Voting> UserHasVoted(string postId, string userId);
        Task DislikeAsync(Voting model);
        Task<VoteViewModel> GetVotedPostUser(string id);
        Task<bool> UserHasLiked(string postId,string userId);
    }
}