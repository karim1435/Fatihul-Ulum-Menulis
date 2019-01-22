using PagedList;
using ScraBoy.Features.CMS.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.CMS.Blog
{
    public interface IPostRepository
    {
        IEnumerable<Post> GetPostsByAuthor(string authorId);
        IEnumerable<Post> GetAllPost();
        Task<Post> GetAsync(string id);
        void Edit(string id,Post updatedItem);
        Task Create(Post model);
        void Delete(string id);
        Task<IEnumerable<Post>> GetAllAsync();
        IEnumerable<Post> GetPostByCategories(string category);
        Task<IEnumerable<Post>> GetPostsByAuthorAsync(string userId);
        IEnumerable<Post> GetPostByTagAsync(string tagId);
        IPagedList<Post> GetPagedList(string search,int page, string userId);
        void GetCookieView(HttpContextBase http);
        Task UpdateViewCount(string postId);
        List<Post> GetBlogList(string name,string tagId,string categoryId);

    }
}
