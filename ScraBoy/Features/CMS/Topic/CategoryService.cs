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
        private readonly ICategoryRepository categoryRepostory;
        private readonly ModelStateDictionary modelState;
        public CategoryService(ICategoryRepository repo,ModelStateDictionary modelState)
        {
            this.categoryRepostory = repo;
            this.modelState = modelState;
        }
        public async Task<bool> IsExistAsync(string name)
        {
            return await this.categoryRepostory.IsExist(name);
        }
        public async Task<bool> AddAsync(Category model)
        {
            if(await IsExistAsync(model.Name))
            {
                modelState.AddModelError(string.Empty,"category Already Exists");
                return false;
            }
            model.CreatedOn = DateTime.Now;

            await this.categoryRepostory.CreateCategoryAsync(model);

            return true;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await this.categoryRepostory.GetAllCategoriesAsync();
        }
        public async Task<bool> UpdateAsync(Category model)
        {
            var category = await this.categoryRepostory.GetByIdAsync(model.Id);

            if(!modelState.IsValid)
            {
                return false;
            }

            if(model.ParentId == category.Id)
            {
                modelState.AddModelError(string.Empty,"Parent and children is same");
                return false;
            }

            await this.categoryRepostory.UpdateCategoryAsync(model);

            return true;

        }
    }
}