using PagedList;
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
        IPagedList<Comment> GetPagedList(string search,int page,string userId);
        Task CreateAsync(Comment model);
        Task<IEnumerable<Comment>> GetCommentByPostIdAsync(string postId);
        Task<Comment> GetCommentById(int id);
        Task DeleteCommentAsync(Comment comment);
        Task EditAsync(Comment model,int commentId);
        Task ReplyAsync(Comment model);
        Comment GetParentComment(int id);
    }
}