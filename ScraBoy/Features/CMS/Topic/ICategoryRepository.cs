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
        Task<IEnumerable<Category>> GetCategoryWithParent();
        Task UpdateCategoryAsync(Category model);
        Task<Category> GetByIdAsync(int id);
    }
}