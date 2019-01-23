using PagedList;
using ScraBoy.Features.Data;
using ScraBoy.Features.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ScraBoy.Features.CMS.ModelBinders;

namespace ScraBoy.Features.Hadist.Book
{
    public class KitabRepository : IKitabRepository
    {
        private readonly int pageSize=10;
        private CMSContext db;
        public KitabRepository()
        {
            db = new CMSContext();
        }
        public async Task Create(Kitab model)
        {
            model.CreatedAt = DateTime.Now;
            this.db.Kitab.Add(model);
            await this.db.SaveChangesAsync();
        }
        public async Task<IEnumerable<Kitab>> FindByChapter(int chapterId)
        {
            return await this.db.Kitab.Include("Chapter").Where(a => a.ChapterId == chapterId).ToArrayAsync();
        }
        public async Task<IEnumerable<Kitab>> Search(int chapterId,string name)
        {
            var model = await FindByChapter(chapterId);

            if(!string.IsNullOrEmpty(name))
            {
                return model.Where(a => a.Content.Contains(name) || a.Content.RemoveHarokah().Contains(name));
            }
            return model;
        }
        public async Task<IPagedList> GetPageByChapter(string name, int chapterId,int currentPage)
        {
            var model = await Search(chapterId,name);

            return model.OrderBy(a => a.Number).ToPagedList(currentPage,pageSize);
        }
        public async Task<IEnumerable<Kitab>> GetAll()
        {
            return await this.db.Kitab.Include("Chapter").OrderBy(a=>a.Number).ToArrayAsync();
        }

        public async Task<Kitab> GetById(int id)
        {
            return await this.db.Kitab.Where(a => a.Id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Kitab>> FindByContent(string query)
        {
            var model = await this.GetAll();

            if(!string.IsNullOrEmpty(query))
            {
                return model.Where(a=>a.Content.Contains(query) || a.Content.RemoveHarokah().Contains(query));
            }
            return model;
        } 
        public async Task<Kitab> GetByNumber(int number)
        {
            return await this.db.Kitab.Where(a => a.Number == number).FirstOrDefaultAsync();
        }
        public async Task<IPagedList> GetPagedListKitab(string name,int currentPage)
        {
            var model = await FindByContent(name);

            return model.OrderBy(a => a.Number).ToPagedList(currentPage,pageSize);
        }
        public async Task GetDataFromWeb(string url)
        {
            HadisPortal hp = new HadisPortal(url);

            
            if(hp.model== null)
            {
                return;
            }
            Kitab hadis = new Kitab();

            hadis.Number = hp.model.Number;
            hadis.Content = hp.model.Content;

            var contain =await GetByNumber(hadis.Number);
            if(contain!=null)
            {
                return;
            }
            await this.Create(hadis);
        }

        public async Task Edit(int id,Kitab model)
        {
            var kitab = await GetById(id);

            kitab.ChapterId = model.ChapterId;
            kitab.Content = model.Content;
            kitab.ImamId = model.ImamId;
            kitab.Number = model.Number;

            await this.db.SaveChangesAsync();
        }

        public async Task Delete(Kitab model)
        {
            this.db.Kitab.Remove(model);
            await this.db.SaveChangesAsync();
        }
    }
}