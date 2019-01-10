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
        int CountPublished { get; }
        Task<Post> GetAsync(string id);
        void Edit(string id,Post updatedItem);
        Task Create(Post model);
        void Delete(string id);
        Task<IEnumerable<Post>> GetAllAsync();
        IEnumerable<Post> GetPostByCategories(string category);
        Task<IEnumerable<string>> GetAllCategories();
        Task<IEnumerable<Post>> GetPostsByAuthorAsync(string userId);
        Task<IEnumerable<Post>> GetPublishedPostAsync();
        IEnumerable<Post> GetPostByTagAsync(string tagId);
        Task<IEnumerable<Post>> GetPageAsync(int pageNumber,int pageSize);
        IPagedList<Post> GetPagedList(string search,int page, string userId);
        void GetCookieView(HttpContextBase http);
        Task UpdateViewCount(string postId);
        Task<IEnumerable<Post>> SortByViews();
        List<Post> GetBlogList(string name,string tagId,string categoryId);

    }
}
