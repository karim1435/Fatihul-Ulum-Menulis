using ScraBoy.Features.CMS.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraBoy.Features.CMS.Blog
{
    public interface IPostRepository
    {
        int CountPublished { get; }
        Task<Post> GetAsync(string id);
        void Edit(string id,Post updatedItem);
        Task Create(Post model);
        void Delete(string id);
        Task<IEnumerable<Post>> GetAllAsync();
        Task<IEnumerable<Post>> GetPostByCategories(string category);
        Task<IEnumerable<string>> GetAllCategories();
        Task<IEnumerable<Post>> GetPostsByAuthorAsync(string userId);
        Task<IEnumerable<Post>> GetPublishedPostAsync();
        Task<IEnumerable<Post>> GetPostByTagAsync(string tagId);
        Task<IEnumerable<Post>> GetPageAsync(int pageNumber,int pageSize);
        
    }
}
