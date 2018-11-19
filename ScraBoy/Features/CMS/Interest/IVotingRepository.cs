using PagedList;
using ScraBoy.Features.CMS.HomeBlog;
using ScraBoy.Features.CMS.User;
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
        Task<bool> UserHasLiked(string postId,string userId);
        List<CMSUser> UserLiked(string id);
        IPagedList<Voting> GetPagedList(string search,int currentPage,string userId);
    }
}