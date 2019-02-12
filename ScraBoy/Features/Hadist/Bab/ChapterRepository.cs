using PagedList;
using ScraBoy.Features.CMS.ModelBinders;
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
            model.SlugUrl = model.Name.MakeUrlFriednly();
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
            chapter.Number = model.Number;
            chapter.SlugUrl = model.Name.MakeUrlFriednly();
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Chapter>> GetAll()
        {
            return await this.db.Chapter.OrderBy(a => a.Number).Include("Imam").ToArrayAsync();
        }
        public async Task<IEnumerable<Chapter>> FindByImam(string url)
        {
            return await this.db.Chapter.OrderBy(a => a.Number).Include("Imam").Where(a => a.Imam.SlugUrl.Equals(url)).ToArrayAsync();
        }
        public async Task<IEnumerable<Chapter>> Find(string imam,string query)
        {
            var model = await FindByImam(imam);

            if(!string.IsNullOrEmpty(query))
            {
                return model.Where(a => a.Name.ToLower().Contains(query.ToLower()));
            }
            return model;
        }
        public async Task<IPagedList> GetPageChapter(string name,string imam,int currenPage)
        {
            var model = await Find(imam,name);

            return model.OrderBy(a => a.Number).ToPagedList(currenPage,pageSize);
        }
        public async Task<Chapter> GetOne(int id)
        {
            var model = await GetAll();
            return model.Where(a => a.Id == id).FirstOrDefault();
        }

        public async Task<Chapter> FindBySlug(string slugUrl)
        {
            return await this.db.Chapter.Where(a => a.SlugUrl.Equals(slugUrl)).FirstOrDefaultAsync();
        }
    }
}