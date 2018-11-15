using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Topic
{
    public class CategoryService
    {
        private readonly int pageSize = 10;
        private readonly ICategoryRepository categoryRepostory;
        private readonly ModelStateDictionary modelState;
        public CategoryService(ICategoryRepository repo,ModelStateDictionary modelState)
        {
            this.categoryRepostory = repo;
            this.modelState = modelState;
        }
        public List<Category> GetCategoryList(string name)
        {
            return this.categoryRepostory.GetCategories(name).OrderByDescending(a => a.CreatedOn).ToList();
        }

        public IPagedList<Category> GetPagedList(string search,int currentPage,string userId)
        {
            var model = new List<Category>();

            if(userId == null)
            {
                model = GetCategoryList(search);
            }
            else
            {
                model = GetCategoryList(search).Where(a => a.AuthorId == userId).ToList();
            }
            model = model.OrderByDescending(s => s.CreatedOn).ToList();
            return model.ToPagedList(currentPage,pageSize);
        }
        public async Task<bool> AddAsync(Category model)
        {
            model.CreatedOn = DateTime.Now;

            await this.categoryRepostory.CreateCategoryAsync(model);

            return true;
        }
        public async Task<IEnumerable<Category>> GetByUserId(string userId)
        {
            return await this.categoryRepostory.GetByUser(userId);
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await this.categoryRepostory.GetAllCategoriesAsync();
        }

        public async Task<bool> UpdateAsync(Category model,int id)
        {
            var category = await this.categoryRepostory.GetByIdAsync(id);

            if(!modelState.IsValid)
            {
                return false;
            }
            category.Name = model.Name;
            await this.categoryRepostory.UpdateCategoryAsync(model);

            return true;

        }
        public async Task DeleteCategory(Category model)
        {
            await this.categoryRepostory.DeleteCategoryAsync(model);
        }
        public async Task<Category> GetCategory(int id)
        {
            return await this.categoryRepostory.GetByIdAsync(id);
        }

        public bool GetCategoryByName(string name,string userId)
        {
            var category =this.categoryRepostory.GetCategoryByUser(userId).Where(a => a.Name == name).ToList();
            return category.Count()>0;
        }
        public bool GetExistingCategory(string name,string userId,int categoryId)
        {
            var category = this.categoryRepostory.GetCategoryByUser(userId).Where(a => a.Name == name && a.Id != categoryId).ToList();
            return category.Count()>0;
        }

    }
}
