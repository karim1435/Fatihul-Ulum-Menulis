using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.CMS.Topic
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task CreateCategoryAsync(Category model);
        Task<bool> IsExist(string name);
        Task UpdateCategoryAsync(Category model);
        Task<Category> GetByIdAsync(int id);
        IQueryable<Category> GetCategories(string name);
        Task DeleteCategoryAsync(Category model);
        Task<bool> IsExistAsync(string name,int id);
        List<Category> GetAllCategory();
    }

}