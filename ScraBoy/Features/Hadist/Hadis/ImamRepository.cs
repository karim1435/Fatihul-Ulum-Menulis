using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PagedList;
using ScraBoy.Features.CMS.ModelBinders;

namespace ScraBoy.Features.Hadist.Hadis
{
    public class ImamRepository : ImamRerpository
    {
        private readonly int pageSize = 10;
        private readonly CMSContext DB;
        public ImamRepository()
        {
            DB = new CMSContext();
        }
        public async Task Create(Imam model)
        {
            model.SlugUrl = model.Name.MakeUrlFriednly();
            this.DB.Imam.Add(model);
            await this.DB.SaveChangesAsync();
        }

        public async Task Delete(Imam model)
        {
            this.DB.Imam.Remove(model);
            await this.DB.SaveChangesAsync();
        }

        public async Task Edit(Imam model,int id)
        {
            var imam = await this.FindById(id);

            imam.Name = model.Name;
            imam.SlugUrl = model.Name.MakeUrlFriednly();
            await this.DB.SaveChangesAsync();
        }
        public async Task<IEnumerable<Imam>> SearchByName(string name)
        {
            var model = await this.GetAll();
            if(!string.IsNullOrEmpty(name))
            {
                return model.Where(a => a.Name.ToLower().Contains(name.ToLower()));
            }
            return model;
        }
        public async Task<Imam> FindById(int id)
        {
            return await this.DB.Imam.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Imam> FindByName(string name)
        {
            return await this.DB.Imam.Where(a => a.Name.Equals(name)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Imam>> GetAll()
        {
            return await this.DB.Imam.ToArrayAsync();
        }

        public async Task<IPagedList> GetPageImam(string name,int currentPage)
        {
            var model = await this.SearchByName(name);
            return model.OrderBy(a => a.Id).ToPagedList(currentPage,pageSize);
        }

        public async Task<Imam> FindBySlug(string slugUrl)
        {
            return await this.DB.Imam.Where(a => a.SlugUrl.Equals(slugUrl)).FirstOrDefaultAsync();
        }
    }
}