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
        private readonly int pageSize = 10;
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
        public async Task<IEnumerable<Kitab>> FindByChapter(string imam,string chapterId)
        {
            return await this.db.Kitab.Include("Chapter").Include(a => a.Translations.Select(post => post.Language)).Where(a => a.Chapter.SlugUrl == chapterId && a.Chapter.Imam.SlugUrl.Equals(imam)).ToArrayAsync();
        }
        public async Task<IEnumerable<Kitab>> Search(string imam,string chapterId,string name)
        {
            var model = await FindByChapter(imam,chapterId);

            if(!string.IsNullOrEmpty(name))
            {
                return model.Where(a => a.Content.Contains(name) || a.Content.RemoveHarokah().Contains(name));
            }
            return model;
        }
        public async Task<IPagedList> GetPageByChapter(string code,int? number,string imam,string name,string chapterId,int currentPage)
        {
            var model = await Search(imam,chapterId,name);

            foreach(var item in model)
            {
                item.CurrentTranslation = item.Translations.Where(a => a.Language.KeyCode.ToLower().Equals(code.ToLower())).FirstOrDefault();
            }
            if(number.HasValue)
            {
                model = model.Where(a => a.Number == number);
            }

            return model.OrderBy(a => a.Number).ToPagedList(currentPage,pageSize);
        }
        public async Task<IEnumerable<Kitab>> GetAll()
        {
            return await this.db.Kitab.Include("Chapter").OrderBy(a => a.Number).OrderBy(a => a.Number).ToArrayAsync();
        }
        public async Task<IEnumerable<Kitab>> FindByImam(string imam)
        {
            return await this.db.Kitab.Include("Chapter.Imam").Where(a => a.Chapter.Imam.SlugUrl.Equals(imam)).ToArrayAsync();
        }
        public async Task<Kitab> FindByNumberImam(string imam,int number)
        {
            var model = await FindByImam(imam);
            return model.Where(a => a.Number == number).FirstOrDefault();
        }
        public async Task<Kitab> GetById(int id)
        {
            return await this.db.Kitab.Include("Chapter").Where(a => a.Id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Kitab>> FindByContent(string imam,string query)
        {
            var model = await FindByImam(imam);

            if(!string.IsNullOrEmpty(query))
            {
                return model.Where(a => a.Content.Contains(query) || a.Content.RemoveHarokah().Contains(query));

            }
            return model;
        }
        public async Task<bool> HasScrapped(int chapterId,int number)
        {
            var model = this.db.Kitab.Where(a => a.Number == number && a.ChapterId == chapterId);
            return await model.CountAsync() > 0;
        }
        public async Task<Kitab> GetByNumber(int number)
        {
            return await this.db.Kitab.Include("Chapter.Imam").Where(a => a.Number == number).FirstOrDefaultAsync();
        }
        public async Task<IPagedList> GetPagedListKitab(string imam,string name,int currentPage)
        {
            var model = await FindByContent(imam,name);

            return model.OrderBy(a => a.Number).ToPagedList(currentPage,pageSize);
        }
        public async Task GetDataFromWeb(int chapterId,int from,int to)
        {
            for(int number = from; number <= to; number++)
            {
                if(await HasScrapped(chapterId,number))
                    continue;

                string url = string.Format("http://hadithportal.com/hadith-sharh-{0}-31717&book=1",number);
                HadisPortal hp = new HadisPortal(url);

                hp.model.Number = number;
                hp.model.ChapterId = chapterId;

                if(string.IsNullOrEmpty(hp.model.Content))
                    hp.model.Content = "The hadist is empty";

                await this.Create(hp.model);
            }
        }

        public async Task Edit(int id,Kitab model)
        {
            var kitab = await GetById(id);

            kitab.ChapterId = model.ChapterId;
            kitab.Content = model.Content;
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