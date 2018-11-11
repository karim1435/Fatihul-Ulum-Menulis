using ScraBoy.Features.CMS.HomeBlog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.CMS.Comments
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task CreateAsync(BlogViewModel model);
        Task<IEnumerable<Comment>> GetCommentByPostIdAsync(string postId);
    }
}