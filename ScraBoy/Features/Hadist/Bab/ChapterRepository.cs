using PagedList;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.Hadist.Bab
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly CMSContext db;
        private readonly int pageSize = 10;
        public ChapterRepository()
        {
            db = new CMSContext();
        }

        public async Task Create(Chapter model)
        {
            this.db.Chapter.Add(model);
            await this.db.SaveChangesAsync();
        }

        public async Task Delete(Chapter model)
        {
             this.db.Chapter.Remove(model);
            await this.db.SaveChangesAsync();
        }

        public async Task Edit(Chapter model,int id)
        {
            var chapter = await GetOne(id);

            chapter.Name = model.Name;
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Chapter>> GetAll()
        {
            return await this.db.Chapter.ToArrayAsync();
        }
        public async Task<IEnumerable<Chapter>> Find(string name)
        {
            var model = await GetAll();

            if(!string.IsNullOrEmpty(name))
            {
                return model.Where(a => a.Name.ToLower().Contains(name.ToLower()));
            }
            return model;
        }
        public async Task<IPagedList> GetPageChapter(string name, int currenPage)
        {
            var model = await Find(name);
            return model.OrderBy(a => a.Id).ToPagedList(currenPage,pageSize);
        }
        public async Task<Chapter> GetOne(int id)
        {
            var model = await GetAll();
            return model.Where(a => a.Id == id).FirstOrDefault();
        }
    }
}