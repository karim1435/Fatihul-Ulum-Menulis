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
        private readonly CMSContext db;
        public CategoryRepository(CMSContext context)
        {
            this.db = context;
        }
        public CategoryRepository() : this(new CMSContext())
        {
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await db.Category.OrderBy(a => a.Id).ToArrayAsync();
        }
        public async Task<IEnumerable<Category>> GetCategoryWithParent()
        {
            using(var db = new CMSContext())
            {
                return db.Category.Include(x => x.Children)
                .AsEnumerable()
                .Where(x => x.Parent == null)
                .ToList();
            }
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

      
    }
}