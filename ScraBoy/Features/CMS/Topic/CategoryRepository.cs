using PagedList;
using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Topic
{
    public class CategoryRepository : ICategoryRepository
    {
        private IUserRepository userRepository;
        private readonly CMSContext db;
        public CategoryRepository(CMSContext context)
        {
            this.db = context;
            this.userRepository = new UserRepository();
        }
        public CategoryRepository() : this(new CMSContext())
        {
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await db.Category.OrderBy(a => a.Id).ToArrayAsync();
        }
        public IQueryable<Category> GetCategories(string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                return this.db.Category.Where(m => m.Name.Contains(name));
            }
            return this.db.Category;
        }

        public async Task CreateCategoryAsync(Category model)
        {
            model.CreatedOn = DateTime.Now;
            db.Category.Add(model);
            await db.SaveChangesAsync();
        }
        public async Task<bool> IsExist(string name)
        {
            return await db.Category.Where(a => a.Name == name).CountAsync()>0;
        }
     
        public async Task<Category> GetByIdAsync(int id)
        {
            return await this.db.Category.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateCategoryAsync(Category model)
        {
            await db.SaveChangesAsync();

        }
        public async Task<bool> IsExistAsync(string name, int id)
        {
            return await this.db.Category.Where(a => a.Name==name && a.Id != id).CountAsync()>0;
        }
        public async Task DeleteCategoryAsync(Category model)
        {
            this.db.Category.Remove(model);
            await this.db.SaveChangesAsync();
        }
        public List<Category> GetAllCategory()
        {
            return this.db.Category.ToList();
        }
    }
}