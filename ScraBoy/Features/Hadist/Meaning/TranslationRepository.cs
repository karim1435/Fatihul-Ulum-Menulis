using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.Hadist.Meaning
{
    public class TranslationRepository : ITranslationRepository
    {
        private readonly CMSContext db;
        public TranslationRepository()
        {
            db = new CMSContext();
        }
        public async Task Create(Translation model)
        {
            model.TranslatedOn = DateTime.Now;
            model.UpdatedOn = DateTime.Now;
            this.db.Translation.Add(model);
            await this.db.SaveChangesAsync();
        }

        public async Task Delete(Translation model)
        {
            this.db.Translation.Remove(model);
            await this.db.SaveChangesAsync();
        }

        public async Task Edit(Translation model,int id)
        {
            var hadist = await FindById(id);

            hadist.Content = model.Content;
            hadist.LanguageId = model.LanguageId;
            hadist.UpdatedOn = DateTime.Now;

            await this.db.SaveChangesAsync();
        }

        public async Task<Translation> FindById(int Id)
        {
            var model = await GetAll();
            return model.Where(a => a.Id == Id).FirstOrDefault();
        }

        public async Task<IEnumerable<Translation>> GetAll()
        {
            return await this.db.Translation.Include("Kitab.Chapter.Imam").Include("Language").ToArrayAsync();
        }

        public async Task<IEnumerable<Language>> GetAllBahasa()
        {
            return await this.db.Language.ToArrayAsync();
        }
    }
}